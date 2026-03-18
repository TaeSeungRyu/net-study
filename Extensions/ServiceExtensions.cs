using MemberApi.Services;
using MemberApi.Security;

namespace MemberApi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<AuthCodeService>();
            services.AddScoped<JwtTokenService>();

            return services;
        }
    }
}