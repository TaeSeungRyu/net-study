using MemberApi.Exceptions;
using MemberApi.Models.Common;
using MemberApi.Models.Dtos;
using MemberApi.Models.Entities;
using MemberApi.Repositories;

namespace MemberApi.Services
{
    public class PostService
    {
        private const int MaxPageSize = 100;

        private readonly IPostRepository _repo;

        public PostService(IPostRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<PostResponse>> GetAllAsync(int page, int size, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (size < 1) size = 10;
            if (size > MaxPageSize) size = MaxPageSize;

            var result = await _repo.GetPagedAsync(page, size, ct);
            return new PagedResult<PostResponse>
            {
                Items = result.Items.Select(ToResponse).ToList(),
                Page = result.Page,
                Size = result.Size,
                TotalCount = result.TotalCount
            };
        }

        public async Task<PostResponse> GetAsync(int id, CancellationToken ct = default)
        {
            var post = await _repo.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("게시글을 찾을 수 없습니다.");
            return ToResponse(post);
        }

        public async Task<int> CreateAsync(CreatePostRequest request, CancellationToken ct = default)
        {
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content
            };
            return await _repo.InsertAsync(post, ct);
        }

        public async Task UpdateAsync(int id, UpdatePostRequest request, CancellationToken ct = default)
        {
            var updated = await _repo.UpdateAsync(new Post
            {
                Id = id,
                Title = request.Title,
                Content = request.Content
            }, ct);

            if (!updated)
                throw new NotFoundException("게시글을 찾을 수 없습니다.");
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var deleted = await _repo.DeleteAsync(id, ct);
            if (!deleted)
                throw new NotFoundException("게시글을 찾을 수 없습니다.");
        }

        private static PostResponse ToResponse(Post p)
            => new()
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content
            };
    }
}
