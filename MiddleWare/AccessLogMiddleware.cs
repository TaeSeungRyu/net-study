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

        //요청 전 로그 출력
        Console.WriteLine(
            $"[Access] Time: {requestedAt:yyyy-MM-dd HH:mm:ss}, IP: {ip}, Method: {method}, Path: {path}{queryString}, UserAgent: {userAgent}"
        );

        //다음 미들웨어로 요청을 전달
        await _next(context);

        //응답 후 로그 출력
        var statusCode = context.Response.StatusCode;
        var respondedAt = DateTime.Now;
        Console.WriteLine(
            $"[Response] Time: {respondedAt:yyyy-MM-dd HH:mm:ss}, StatusCode: {statusCode}, Path: {path}{queryString}"
        );        
    }
}