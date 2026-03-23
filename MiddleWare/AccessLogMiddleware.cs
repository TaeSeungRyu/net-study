namespace MemberApi.Middleware;
public class AccessLogMiddleware
{
    private readonly RequestDelegate _next;

    public AccessLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var method = context.Request.Method;
        var path = context.Request.Path;
        var queryString = context.Request.QueryString;
        var ip = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var requestedAt = DateTime.Now;

        Console.WriteLine(
            $"[Access] Time: {requestedAt:yyyy-MM-dd HH:mm:ss}, IP: {ip}, Method: {method}, Path: {path}{queryString}, UserAgent: {userAgent}"
        );

        await _next(context);
    }
}