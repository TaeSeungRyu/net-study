using MemberApi.Models;
using MemberApi.Security;
using MongoDB.Driver;

namespace MemberApi.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<User> _users;
        private readonly JwtTokenService _jwtService;

        public AuthService(IMongoClient client)
        {
            var database = client.GetDatabase("appdb");
            _users = database.GetCollection<User>("user");
            _jwtService = new JwtTokenService();
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

        public async Task<object?> GenerateToken(string username, string password)
        {
            var user = await ValidateUser(username, password);
            if (user == null)
                return null;
                
            var token = _jwtService.GenerateToken(user);
            return new
            {
                token,
                user = new
                {
                    user.id,
                    user.username,
                    user.name
                }
            };
        }
    }
}