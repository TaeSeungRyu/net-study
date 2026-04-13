using MemberApi.Models.Common;
using MemberApi.Models.Dtos;
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<AuthCodeResponse>>>> List(
            [FromQuery] string? name,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            CancellationToken ct = default)
        {
            var result = await _service.ListAsync(name, page, size, ct);
            return Ok(ApiResponse<PagedResult<AuthCodeResponse>>.Ok(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AuthCodeResponse>>> Get(string id, CancellationToken ct)
        {
            var item = await _service.GetAsync(id, ct);
            return Ok(ApiResponse<AuthCodeResponse>.Ok(item));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AuthCodeResponse>>> Create(
            [FromBody] CreateAuthCodeRequest request,
            CancellationToken ct)
        {
            var created = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, ApiResponse<AuthCodeResponse>.Ok(created));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<AuthCodeResponse>>> Update(
            string id,
            [FromBody] UpdateAuthCodeRequest request,
            CancellationToken ct)
        {
            var updated = await _service.UpdateAsync(id, request, ct);
            return Ok(ApiResponse<AuthCodeResponse>.Ok(updated));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(string id, CancellationToken ct)
        {
            await _service.DeleteAsync(id, ct);
            return Ok(ApiResponse<object>.Ok(null, "Auth code deleted successfully"));
        }
    }
}
