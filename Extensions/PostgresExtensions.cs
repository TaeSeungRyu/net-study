using MemberApi.Config;
using Microsoft.Extensions.Options;
using Npgsql;

namespace MemberApi.Extensions
{
    public static class PostgresExtensions
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services)
        {
            services.AddScoped<NpgsqlConnection>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<PostgresSettings>>().Value;
                return new NpgsqlConnection(settings.ConnectionString);;
            });

            return services;
        }
    }
}