using MemberApi.Exceptions;
using MemberApi.Models.Common;
using MemberApi.Models.Dtos;
using MemberApi.Models.Entities;
using MemberApi.Repositories;

namespace MemberApi.Services
{
    public class AuthCodeService
    {
        private readonly IAuthCodeRepository _repo;

        public AuthCodeService(IAuthCodeRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<AuthCodeResponse>> ListAsync(string? name, int page, int size, CancellationToken ct = default)
        {
            var result = await _repo.ListAsync(name, page, size, ct);
            return new PagedResult<AuthCodeResponse>
            {
                Items = result.Items.Select(ToResponse).ToList(),
                Page = result.Page,
                Size = result.Size,
                TotalCount = result.TotalCount
            };
        }

        public async Task<AuthCodeResponse> GetAsync(string id, CancellationToken ct = default)
        {
            var item = await _repo.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("권한 코드를 찾을 수 없습니다.");
            return ToResponse(item);
        }

        public async Task<AuthCodeResponse> CreateAsync(CreateAuthCodeRequest request, CancellationToken ct = default)
        {
            var existing = await _repo.GetByCodeAsync(request.Code, ct);
            if (existing is not null)
                throw new ConflictException("이미 존재하는 코드입니다.");

            var now = DateTime.UtcNow;
            var entity = new AuthCode
            {
                Code = request.Code,
                Name = request.Name,
                Desc = request.Desc,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _repo.InsertAsync(entity, ct);
            return ToResponse(entity);
        }

        public async Task<AuthCodeResponse> UpdateAsync(string id, UpdateAuthCodeRequest request, CancellationToken ct = default)
        {
            var updated = await _repo.UpdateAsync(id, request.Name, request.Desc, ct)
                ?? throw new NotFoundException("권한 코드를 찾을 수 없습니다.");
            return ToResponse(updated);
        }

        public async Task DeleteAsync(string id, CancellationToken ct = default)
        {
            var deleted = await _repo.DeleteAsync(id, ct);
            if (!deleted)
                throw new NotFoundException("권한 코드를 찾을 수 없습니다.");
        }

        private static AuthCodeResponse ToResponse(AuthCode a)
            => new()
            {
                Id = a.Id ?? string.Empty,
                Code = a.Code,
                Name = a.Name,
                Desc = a.Desc,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            };
    }
}
