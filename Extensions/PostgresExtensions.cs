using MemberApi.Config;
using MemberApi.Data;
using Microsoft.EntityFrameworkCore;

namespace MemberApi.Extensions
{
    public static class PostgresExtensions
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PostgresSettings>(configuration.GetSection("Postgres"));

            var pg = configuration.GetSection("Postgres").Get<PostgresSettings>()
                ?? throw new InvalidOperationException("Postgres 설정이 없습니다.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(pg.ConnectionString));

            return services;
        }
    }
}
