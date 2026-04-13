using MemberApi.Models.Common;
using MemberApi.Models.Dtos;
using MemberApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemberApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<PostResponse>>>> GetAll(CancellationToken ct)
        {
            var posts = await _postService.GetAllAsync(ct);
            return Ok(ApiResponse<List<PostResponse>>.Ok(posts));
        }

        [HttpGet("{id:int}")]
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
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id, CancellationToken ct)
        {
            await _postService.DeleteAsync(id, ct);
            return Ok(ApiResponse<object>.Ok(null, "Post deleted successfully"));
        }
    }
}
