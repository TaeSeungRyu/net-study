using System.Diagnostics;

namespace MemberApi.Middleware
{
    public class AccessLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AccessLogMiddleware> _logger;

        public AccessLogMiddleware(RequestDelegate next, ILogger<AccessLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            var request = context.Request;
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = request.Headers.UserAgent.ToString();

            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation(
                    "{Method} {Path}{QueryString} from {Ip} -> {StatusCode} in {ElapsedMs}ms ({UserAgent})",
                    request.Method,
                    request.Path,
                    request.QueryString,
                    ip,
                    context.Response.StatusCode,
                    sw.ElapsedMilliseconds,
                    userAgent
                );
            }
        }
    }
}
