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
                Id = u.Id!,
                Username = u.Username,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone
            }).ToList();
        }

        public async Task<UserResponse?> Find(string id)
        {
            var user = await _users
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (user == null)
                return null;
            return new UserResponse
            {
                Id = user.Id!,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            };
        }
    }
}