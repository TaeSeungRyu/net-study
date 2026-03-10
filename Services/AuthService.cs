using MemberApi.Models;
using MemberApi.Security;
using MongoDB.Driver;

namespace MemberApi.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<User> _users;

        public AuthService(IMongoClient client)
        {
            var database = client.GetDatabase("appdb");
            _users = database.GetCollection<User>("user");
        }

        public async Task<User?> ValidateUser(string username, string password)
        {
            // Console.WriteLine("Hello World");
            // Console.WriteLine($"값: {username} {password}");
            var user = await _users
                .Find(x => x.username == username)
                .FirstOrDefaultAsync();
            if (user == null)
                return null;

            if (!PasswordUtil.ComparePassword(password, user.password))
                return null;

            return user;
        }
    }
}