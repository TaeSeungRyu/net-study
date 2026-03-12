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
            var user = await _authService.GenerateToken(
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
            return Ok(
                new ApiResponse<object>(
                    true,
                    user
                )
            );
        }
    }
}