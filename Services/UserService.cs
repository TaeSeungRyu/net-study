using MemberApi.Models;
using MemberApi.Security;
using MongoDB.Driver;

namespace MemberApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Auth> _auths;

        public UserService(IMongoClient client)
        {
            var database = client.GetDatabase("appdb");
            _users = database.GetCollection<User>("user");
            _auths = database.GetCollection<Auth>("auth");
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

            List<Auth> authList = new();

            if (user.role != null && user.role.Count > 0)
            {
                authList = await _auths
                    .Find(x => user.role.Contains(x.code))
                    .ToListAsync();
            }

            return new UserResponse
            {
                id = user.id!,
                username = user.username,
                name = user.name,
                email = user.email,
                phone = user.phone,
                auth = authList
            };
        }

        public async Task<UserResponse> Create(User user)
        {

            if (string.IsNullOrWhiteSpace(user.username))
                throw new ArgumentException("아이디는 필수입니다.");

            if (string.IsNullOrWhiteSpace(user.password))
                throw new ArgumentException("비밀번호는 필수입니다.");

            var existingUser = await _users
                .Find(x => x.username == user.username)
                .FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new InvalidOperationException("이미 존재하는 사용자 이름입니다.");
            }

            user.password = PasswordUtil.HashPassword(user.password);

            await _users.InsertOneAsync(user);
            return new UserResponse
            {
                id = user.id!,
                username = user.username,
                name = user.name,
                email = user.email,
                phone = user.phone
            };
        }

        public async Task Update(string id, User user)
        {
            var existingUser = await _users
                .Find(x => x.id == id)
                .FirstOrDefaultAsync();
            if (existingUser == null)
                throw new KeyNotFoundException("사용자를 찾을 수 없습니다.");
            if (!string.IsNullOrWhiteSpace(user.password))
            {
                user.password = PasswordUtil.HashPassword(user.password);
            }
            else
            {
                user.password = existingUser.password;
            }
            user.username = existingUser.username; // 아이디는 변경 불가
            user.id = id;
            user.updatedAt = DateTime.UtcNow;
            await _users.ReplaceOneAsync(x => x.id == id, user);
        }
        
        public async Task Delete(string id)
        {
            var existingUser = await _users
                .Find(x => x.id == id)
                .FirstOrDefaultAsync();

            if (existingUser == null)
                throw new KeyNotFoundException("사용자를 찾을 수 없습니다.");

            var result = await _users.DeleteOneAsync(x => x.id == id);
            if (result.DeletedCount == 0){
                throw new InvalidOperationException("사용자 삭제에 실패했습니다.");
            }
        }        
    }
}