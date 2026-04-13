using MemberApi.Config;
using MemberApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MemberApi.Data
{
    /// <summary>
    /// MongoDB 컬렉션 접근 지점. 컬렉션 이름을 한 곳에서 관리한다.
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient client, IOptions<MongoDbSettings> options)
        {
            var settings = options.Value;
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("user");

        public IMongoCollection<AuthCode> AuthCodes => _database.GetCollection<AuthCode>("auth");
    }
}
