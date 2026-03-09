using MemberApi.Models;
using MemberApi.Security;
using MemberApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemberApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class LoginController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtTokenService _jwtService;

        public LoginController(
            AuthService authService,
            JwtTokenService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {

            Console.WriteLine("Hello World");
            Console.WriteLine($"값: {req.username} {req.password}");
            var user = await _authService.ValidateUser(
                req.username,
                req.password
            );
            if (user == null)
            {
                return Unauthorized(
                    new ApiResponse<object>(
                        false,
                        null,
                        "아이디 또는 비밀번호가 올바르지 않습니다."
                    )
                );
            }
            var token = _jwtService.GenerateToken(user);
            return Ok(
                new ApiResponse<object>(
                    true,
                    new
                    {
                        token,
                        user = new
                        {
                            user.id,
                            user.username,
                            user.name
                        }
                    }
                )
            );
        }
    }
}