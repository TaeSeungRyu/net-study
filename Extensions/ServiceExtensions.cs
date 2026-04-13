using MemberApi.Config;
using MemberApi.Repositories;
using MemberApi.Security;
using MemberApi.Services;

namespace MemberApi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthCodeRepository, AuthCodeRepository>();
            services.AddScoped<IPostRepository, PostRepository>();

            // Services
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<AuthCodeService>();
            services.AddScoped<PostService>();
            services.AddScoped<JwtTokenService>();

            return services;
        }
    }
}
