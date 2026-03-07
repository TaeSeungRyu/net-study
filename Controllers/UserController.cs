using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MemberApi.Models;

namespace MemberApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;

        public UserController(IMongoClient client)
        {
            var database = client.GetDatabase("appdb");
            _users = database.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

    }
}