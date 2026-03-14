using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemberApi.Models;
using MemberApi.Services;

namespace MemberApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("list")]
        [Authorize(Roles = "admin")] //테스트
        public async Task<ApiResponse<List<UserResponse>>> List(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10
        )
        {
            var users = await _userService.List(page, size);
            return new ApiResponse<List<UserResponse>>(true, users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> Find(string id)
        {
            var user = await _userService.Find(id);
            if (user == null)
                return NotFound(new ApiResponse<UserResponse>(false, null, "User not found"));
            return Ok(new ApiResponse<UserResponse>(true, user));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserResponse>>> Create([FromBody] User user)
        {
            try
            {
                var createdUser = await _userService.Create(user);
                return Ok(new ApiResponse<UserResponse>(true, createdUser));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<UserResponse>(false, null, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> Update(string id, [FromBody] User user)
        {
            try
            {
                await _userService.Update(id, user);
                return Ok(new ApiResponse<UserResponse>(true, null, "User updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<UserResponse>(false, null, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userService.Delete(id);
                return Ok(new ApiResponse<UserResponse>(true, null, "User deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<UserResponse>(false, null, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<UserResponse>(false, null, ex.Message));
            }
        }        
    }
}