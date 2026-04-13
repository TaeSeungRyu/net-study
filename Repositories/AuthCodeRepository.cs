using MemberApi.Data;
using MemberApi.Models.Common;
using MemberApi.Models.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MemberApi.Repositories
{
    public class AuthCodeRepository : IAuthCodeRepository
    {
        private readonly MongoDbContext _db;

        public AuthCodeRepository(MongoDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<AuthCode>> ListAsync(string? name, int page, int size, CancellationToken ct = default)
        {
            var filter = BuildFilter(name);

            var total = await _db.AuthCodes.CountDocumentsAsync(filter, cancellationToken: ct);

            var items = await _db.AuthCodes
                .Find(filter)
                .Skip((page - 1) * size)
                .Limit(size)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync(ct);

            return new PagedResult<AuthCode>
            {
                Items = items,
                Page = page,
                Size = size,
                TotalCount = total
            };
        }

        public Task<AuthCode?> GetByIdAsync(string id, CancellationToken ct = default)
            => _db.AuthCodes.Find(x => x.Id == id).FirstOrDefaultAsync(ct)!;

        public Task<AuthCode?> GetByCodeAsync(string code, CancellationToken ct = default)
            => _db.AuthCodes.Find(x => x.Code == code).FirstOrDefaultAsync(ct)!;

        public Task InsertAsync(AuthCode entity, CancellationToken ct = default)
            => _db.AuthCodes.InsertOneAsync(entity, cancellationToken: ct);

        public Task<AuthCode?> UpdateAsync(string id, string? name, string? desc, CancellationToken ct = default)
        {
            var update = Builders<AuthCode>.Update
                .Set(x => x.Name, name)
                .Set(x => x.Desc, desc)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            return _db.AuthCodes.FindOneAndUpdateAsync(
                Builders<AuthCode>.Filter.Eq(x => x.Id, id),
                update,
                new FindOneAndUpdateOptions<AuthCode> { ReturnDocument = ReturnDocument.After },
                ct
            )!;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken ct = default)
        {
            var result = await _db.AuthCodes.DeleteOneAsync(x => x.Id == id, ct);
            return result.DeletedCount > 0;
        }

        private static FilterDefinition<AuthCode> BuildFilter(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Builders<AuthCode>.Filter.Empty;
            }

            return Builders<AuthCode>.Filter.Regex(
                x => x.Name,
                new BsonRegularExpression(name, "i")
            );
        }
    }
}
