using System.Net;
using System.Text.Json;
using MemberApi.Exceptions;
using MemberApi.Models.Common;

namespace MemberApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleAsync(context, ex);
            }
        }

        private async Task HandleAsync(HttpContext context, Exception ex)
        {
            var (statusCode, message, logLevel) = ex switch
            {
                ValidationException => (HttpStatusCode.BadRequest, ex.Message, LogLevel.Information),
                UnauthorizedException => (HttpStatusCode.Unauthorized, ex.Message, LogLevel.Information),
                NotFoundException => (HttpStatusCode.NotFound, ex.Message, LogLevel.Information),
                ConflictException => (HttpStatusCode.Conflict, ex.Message, LogLevel.Information),
                _ => (HttpStatusCode.InternalServerError, "서버 오류가 발생했습니다.", LogLevel.Error)
            };

            var traceId = context.TraceIdentifier;

            using (_logger.BeginScope(new Dictionary<string, object> { ["TraceId"] = traceId }))
            {
                _logger.Log(logLevel, ex, "Request failed: {Method} {Path} (TraceId={TraceId})",
                    context.Request.Method, context.Request.Path, traceId);
            }

            if (context.Response.HasStarted)
            {
                // Response already started - cannot rewrite. Only log.
                return;
            }

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            context.Response.Headers["X-Trace-Id"] = traceId;

            var body = JsonSerializer.Serialize(
                new ApiResponse<object>(false, new { traceId }, message),
                JsonOptions
            );

            await context.Response.WriteAsync(body);
        }
    }
}
