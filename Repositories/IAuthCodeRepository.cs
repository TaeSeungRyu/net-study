using MemberApi.Models.Common;
using MemberApi.Models.Entities;

namespace MemberApi.Repositories
{
    public interface IAuthCodeRepository
    {
        Task<PagedResult<AuthCode>> ListAsync(string? name, int page, int size, CancellationToken ct = default);
        Task<AuthCode?> GetByIdAsync(string id, CancellationToken ct = default);
        Task<AuthCode?> GetByCodeAsync(string code, CancellationToken ct = default);
        Task InsertAsync(AuthCode entity, CancellationToken ct = default);
        Task<AuthCode?> UpdateAsync(string id, string? name, string? desc, CancellationToken ct = default);
        Task<bool> DeleteAsync(string id, CancellationToken ct = default);
    }
}
