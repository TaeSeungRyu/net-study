using MemberApi.Models;
using MongoDB.Driver;

namespace MemberApi.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<User> _users;

        public AuthService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("user");
        }

        public async Task<User?> ValidateUser(string username, string password)
        {

            Console.WriteLine("Hello World");
            Console.WriteLine($"값: {username} {password}");


            var user = await _users
                .Find(x => x.Username == username)
                .FirstOrDefaultAsync();

            if (user == null)
                return null;

            if (user.Password != password)
                return null;

            return user;
        }
    }
}