using MemberApi.Models;
using MongoDB.Driver;

namespace MemberApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoClient client)
        {
            var database = client.GetDatabase("appdb");
            _users = database.GetCollection<User>("user");
        }

        public async Task<List<UserResponse>> List(int page, int size)
        {
            var skip = (page - 1) * size;
            var users = await _users
                .Find(_ => true)
                .Skip(skip)
                .Limit(size)
                .ToListAsync();
            return users.Select(u => new UserResponse
            {
                id = u.id!,
                username = u.username,
                name = u.name,
                email = u.email,
                phone = u.phone
            }).ToList();
        }

        public async Task<UserResponse?> Find(string id)
        {
            var user = await _users
                .Find(x => x.id == id)
                .FirstOrDefaultAsync();
            if (user == null)
                return null;
            return new UserResponse
            {
                id = user.id!,
                username = user.username,
                name = user.name,
                email = user.email,
                phone = user.phone
            };
        }
    }
}