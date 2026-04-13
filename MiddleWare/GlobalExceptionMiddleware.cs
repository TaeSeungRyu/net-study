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
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
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

            _logger.Log(logLevel, ex, "Request failed: {Path}", context.Request.Path);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var body = JsonSerializer.Serialize(
                ApiResponse<object>.Fail(message),
                JsonOptions
            );

            await context.Response.WriteAsync(body);
        }
    }
}
