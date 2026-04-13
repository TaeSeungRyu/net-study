using MemberApi.Models.Entities;

namespace MemberApi.Repositories
{
    public interface IUserRepository
    {
        Task<IReadOnlyList<UserWithRoles>> ListWithRolesAsync(int page, int size, CancellationToken ct = default);
        Task<User?> GetByIdAsync(string id, CancellationToken ct = default);
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task<IReadOnlyList<AuthCode>> GetRolesAsync(IEnumerable<string> codes, CancellationToken ct = default);
        Task InsertAsync(User user, CancellationToken ct = default);
        Task ReplaceAsync(User user, CancellationToken ct = default);
        Task<bool> DeleteAsync(string id, CancellationToken ct = default);
    }
}
