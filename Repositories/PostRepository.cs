using MemberApi.Data;
using MemberApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberApi.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _db;

        public PostRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<Post>> GetAllAsync(CancellationToken ct = default)
            => await _db.Posts.AsNoTracking().ToListAsync(ct);

        public Task<Post?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.Posts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<int> InsertAsync(Post post, CancellationToken ct = default)
        {
            _db.Posts.Add(post);
            await _db.SaveChangesAsync(ct);
            return post.Id;
        }

        public async Task<bool> UpdateAsync(Post post, CancellationToken ct = default)
        {
            var affected = await _db.Posts
                .Where(x => x.Id == post.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Title, post.Title)
                    .SetProperty(x => x.Content, post.Content),
                    ct);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var affected = await _db.Posts
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(ct);
            return affected > 0;
        }
    }
}
