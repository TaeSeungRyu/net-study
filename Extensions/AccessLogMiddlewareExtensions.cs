using MemberApi.Middleware;

namespace MemberApi.Extensions
{
    public static class AccessLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseAccessLog(this IApplicationBuilder app)
            => app.UseMiddleware<AccessLogMiddleware>();

        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
            => app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
