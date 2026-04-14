using MemberApi.Models.Common;
using MemberApi.Models.Entities;

namespace MemberApi.Repositories
{
    public interface IPostRepository
    {
        Task<PagedResult<Post>> GetPagedAsync(int page, int size, CancellationToken ct = default);
        Task<Post?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<int> InsertAsync(Post post, CancellationToken ct = default);
        Task<bool> UpdateAsync(Post post, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
