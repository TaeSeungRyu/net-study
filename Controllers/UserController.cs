using MemberApi.Models.Common;
using MemberApi.Models.Dtos;
using MemberApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemberApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<UserResponse>>>> List(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            CancellationToken ct = default)
        {
            var users = await _userService.ListAsync(page, size, ct);
            return Ok(ApiResponse<List<UserResponse>>.Ok(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> Get(string id, CancellationToken ct)
        {
            var user = await _userService.GetAsync(id, ct);
            return Ok(ApiResponse<UserResponse>.Ok(user));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserResponse>>> Create(
            [FromBody] CreateUserRequest request,
            CancellationToken ct)
        {
            var created = await _userService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, ApiResponse<UserResponse>.Ok(created));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(
            string id,
            [FromBody] UpdateUserRequest request,
            CancellationToken ct)
        {
            await _userService.UpdateAsync(id, request, ct);
            return Ok(ApiResponse<object>.Ok(null, "User updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(string id, CancellationToken ct)
        {
            await _userService.DeleteAsync(id, ct);
            return Ok(ApiResponse<object>.Ok(null, "User deleted successfully"));
        }
    }
}
