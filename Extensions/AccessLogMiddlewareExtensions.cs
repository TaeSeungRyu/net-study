using MemberApi.Middleware;
namespace MemberApi.Extensions
{
    public static class AccessLogMiddlewareExtensions
    {
        //this IApplicationBuilder app => 이 문법이 붙으면 IApplicationBuilder 타입의 확장 메서드가 된다는 뜻
        public static IApplicationBuilder UseAccessLog(this IApplicationBuilder app)
        {
            //AccessLogMiddleware를 미들웨어 파이프라인에 추가하는 역할
            return app.UseMiddleware<AccessLogMiddleware>();
        }
    }
}