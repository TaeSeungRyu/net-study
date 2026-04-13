using MemberApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Post> Posts => Set<Post>();
    }
}
