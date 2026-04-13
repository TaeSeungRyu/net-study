using MemberApi.Data;
using MemberApi.Models.Entities;
using MongoDB.Driver;

namespace MemberApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MongoDbContext _db;

        public UserRepository(MongoDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<UserWithRoles>> ListWithRolesAsync(int page, int size, CancellationToken ct = default)
        {
            var skip = (page - 1) * size;

            return await _db.Users.Aggregate()
                .Lookup<User, AuthCode, UserWithRoles>(
                    foreignCollection: _db.AuthCodes,
                    localField: u => u.Role,
                    foreignField: a => a.Code,
                    @as: x => x.Roles
                )
                .Skip(skip)
                .Limit(size)
                .ToListAsync(ct);
        }

        public Task<User?> GetByIdAsync(string id, CancellationToken ct = default)
            => _db.Users.Find(x => x.Id == id).FirstOrDefaultAsync(ct)!;

        public Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
            => _db.Users.Find(x => x.Username == username).FirstOrDefaultAsync(ct)!;

        public async Task<IReadOnlyList<AuthCode>> GetRolesAsync(IEnumerable<string> codes, CancellationToken ct = default)
        {
            var list = codes.ToList();
            if (list.Count == 0) return Array.Empty<AuthCode>();
            return await _db.AuthCodes.Find(x => list.Contains(x.Code)).ToListAsync(ct);
        }

        public Task InsertAsync(User user, CancellationToken ct = default)
            => _db.Users.InsertOneAsync(user, cancellationToken: ct);

        public Task ReplaceAsync(User user, CancellationToken ct = default)
            => _db.Users.ReplaceOneAsync(x => x.Id == user.Id, user, cancellationToken: ct);

        public async Task<bool> DeleteAsync(string id, CancellationToken ct = default)
        {
            var result = await _db.Users.DeleteOneAsync(x => x.Id == id, ct);
            return result.DeletedCount > 0;
        }
    }
}
