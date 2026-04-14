using MemberApi.Models.Common;
using MemberApi.Models.Dtos;
using MemberApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemberApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PagedResult<PostResponse>>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            CancellationToken ct = default)
        {
            var result = await _postService.GetAllAsync(page, size, ct);
            return Ok(ApiResponse<PagedResult<PostResponse>>.Ok(result));
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PostResponse>>> Get(int id, CancellationToken ct)
        {
            var post = await _postService.GetAsync(id, ct);
            return Ok(ApiResponse<PostResponse>.Ok(post));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Create(
            [FromBody] CreatePostRequest request,
            CancellationToken ct)
        {
            var id = await _postService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(Get), new { id }, ApiResponse<object>.Ok(new { id }));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(
            int id,
            [FromBody] UpdatePostRequest request,
            CancellationToken ct)
        {
            await _postService.UpdateAsync(id, request, ct);
            return Ok(ApiResponse<object>.Ok(null, "Post updated successfully"));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id, CancellationToken ct)
        {
            await _postService.DeleteAsync(id, ct);
            return Ok(ApiResponse<object>.Ok(null, "Post deleted successfully"));
        }
    }
}
