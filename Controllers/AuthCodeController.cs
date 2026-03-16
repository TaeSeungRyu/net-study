using MemberApi.Models;
using MemberApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemberApi.Controllers
{
    [ApiController]
    [Route("api/auth-codes")]
    public class AuthCodeController : ControllerBase
    {
        private readonly AuthCodeService _service;

        public AuthCodeController(AuthCodeService service)
        {
            _service = service;
        }

        // 목록 조회
        [HttpGet]
        public async Task<IActionResult> List(
            [FromQuery] string? name,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10
        )
        {
            var result = await _service.List(name, page, size);
            return Ok(new ApiResponse<List<Auth>>(true, result));
        }

        // 단일 조회
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var auth = await _service.Get(id);

            if (auth == null)
                return NotFound();
            return Ok(new ApiResponse<Auth>(true, auth));
        }
    }
}