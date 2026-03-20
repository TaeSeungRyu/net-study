using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MemberApi.Config;

namespace MemberApi.Extensions
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            return services;
        }
    }
}