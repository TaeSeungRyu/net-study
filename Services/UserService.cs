using MemberApi.Exceptions;
using MemberApi.Models.Dtos;
using MemberApi.Models.Entities;
using MemberApi.Repositories;
using MemberApi.Security;

namespace MemberApi.Services
{
    public class UserService
    {
        private readonly IUserRepository _users;

        public UserService(IUserRepository users)
        {
            _users = users;
        }

        public async Task<List<UserResponse>> ListAsync(int page, int size, CancellationToken ct = default)
        {
            var items = await _users.ListWithRolesAsync(page, size, ct);

            return items.Select(ToResponse).ToList();
        }

        public async Task<UserResponse> GetAsync(string id, CancellationToken ct = default)
        {
            var user = await _users.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("사용자를 찾을 수 없습니다.");

            var roles = user.Role is { Count: > 0 }
                ? await _users.GetRolesAsync(user.Role, ct)
                : Array.Empty<AuthCode>();

            return ToResponse(user, roles);
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ValidationException("아이디는 필수입니다.");
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ValidationException("비밀번호는 필수입니다.");

            var existing = await _users.GetByUsernameAsync(request.Username, ct);
            if (existing is not null)
                throw new ConflictException("이미 존재하는 사용자 이름입니다.");

            var now = DateTime.UtcNow;
            var user = new User
            {
                Username = request.Username,
                Password = PasswordUtil.HashPassword(request.Password),
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                ProfileImage = request.ProfileImage,
                Role = request.Role,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _users.InsertAsync(user, ct);

            return ToResponse(user, Array.Empty<AuthCode>());
        }

        public async Task UpdateAsync(string id, UpdateUserRequest request, CancellationToken ct = default)
        {
            var user = await _users.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("사용자를 찾을 수 없습니다.");

            user.Name = request.Name ?? user.Name;
            user.Email = request.Email ?? user.Email;
            user.Phone = request.Phone ?? user.Phone;
            user.ProfileImage = request.ProfileImage ?? user.ProfileImage;
            user.Role = request.Role ?? user.Role;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.Password = PasswordUtil.HashPassword(request.Password);
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _users.ReplaceAsync(user, ct);
        }

        public async Task DeleteAsync(string id, CancellationToken ct = default)
        {
            var deleted = await _users.DeleteAsync(id, ct);
            if (!deleted)
                throw new NotFoundException("사용자를 찾을 수 없습니다.");
        }

        private static UserResponse ToResponse(UserWithRoles u)
            => new()
            {
                Id = u.Id ?? string.Empty,
                Username = u.Username,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                ProfileImage = u.ProfileImage,
                Roles = (u.Roles ?? new List<AuthCode>()).Select(ToAuthDto).ToList()
            };

        private static UserResponse ToResponse(User u, IReadOnlyList<AuthCode> roles)
            => new()
            {
                Id = u.Id ?? string.Empty,
                Username = u.Username,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                ProfileImage = u.ProfileImage,
                Roles = roles.Select(ToAuthDto).ToList()
            };

        private static AuthCodeResponse ToAuthDto(AuthCode a)
            => new()
            {
                Id = a.Id ?? string.Empty,
                Code = a.Code,
                Name = a.Name,
                Desc = a.Desc,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            };
    }
}
