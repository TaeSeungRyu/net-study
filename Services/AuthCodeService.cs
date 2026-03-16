using MemberApi.Models;
using MongoDB.Driver;

namespace MemberApi.Services
{
    public class AuthCodeService
    {
        private readonly IMongoCollection<Auth> _auths;

        public AuthCodeService(IMongoClient client)
        {
            var database = client.GetDatabase("appdb");
            _auths = database.GetCollection<Auth>("auth");
        }

        // 목록 조회 (페이징 + 이름 검색)
        public async Task<List<Auth>> List(string? name, int page, int size)
        {
            var skip = (page - 1) * size;
            var filter = Builders<Auth>.Filter.Empty;
            if (!string.IsNullOrWhiteSpace(name))
            {
                filter = Builders<Auth>.Filter.Regex(
                    x => x.name,
                    new MongoDB.Bson.BsonRegularExpression(name, "i")
                );
            }
            var result = await _auths
                .Find(filter)
                .Skip(skip)
                .Limit(size)
                .SortByDescending(x => x.createdAt)
                .ToListAsync();

            return result;
        }

        public async Task<PagedResult<Auth>> PagedList(string? name, int page, int size)
        {
            var filter = Builders<Auth>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(name))
            {
                filter = Builders<Auth>.Filter.Regex(x => x.name, new MongoDB.Bson.BsonRegularExpression(name, "i"));
            }

            var total = await _auths.CountDocumentsAsync(filter);

            var items = await _auths
                .Find(filter)
                .Skip((page - 1) * size)
                .Limit(size)
                .ToListAsync();

            return new PagedResult<Auth>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalCount = (int)total
            };
        }        

        // 단일 조회
        public async Task<Auth?> Get(string id)
        {
            return await _auths
                .Find(x => x.id == id)
                .FirstOrDefaultAsync();
        }
    }
}