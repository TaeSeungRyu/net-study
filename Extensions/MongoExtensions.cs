using MongoDB.Driver;

namespace MemberApi.Extensions
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(
                new MongoClient("mongodb://root:rootpassword@localhost:27017/appdb?authSource=admin")
            );

            return services;
        }
    }
}