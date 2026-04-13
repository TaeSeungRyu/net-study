using MemberApi.Exceptions;
using MemberApi.Models.Dtos;
using MemberApi.Repositories;
using MemberApi.Security;

namespace MemberApi.Services
{
    public class AuthService
    {
        private readonly IUserRepository _users;
        private readonly JwtTokenService _jwt;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository users,
            JwtTokenService jwt,
            ILogger<AuthService> logger)
        {
            _users = users;
            _jwt = jwt;
            _logger = logger;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            var user = await _users.GetByUsernameAsync(request.Username, ct);
            if (user is null || !PasswordUtil.VerifyPassword(request.Password, user.Password))
            {
                _logger.LogInformation("Login failed for {Username}", request.Username);
                throw new UnauthorizedException("아이디 또는 비밀번호가 올바르지 않습니다.");
            }

            var token = _jwt.GenerateToken(user);

            return new LoginResponse
            {
                Token = token,
                User = new UserSummary
                {
                    Id = user.Id ?? string.Empty,
                    Username = user.Username,
                    Name = user.Name
                }
            };
        }
    }
}
