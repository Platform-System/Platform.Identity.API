using Microsoft.EntityFrameworkCore;
using Platform.Identity.API.Infrastructure.Persistence.Models;
using Platform.Infrastructure.Data;

namespace Platform.Identity.API.Infrastructure.Data
{
    public class IdentityDbContext : BaseDbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options, string? currentUserId = null) : base(options, currentUserId)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasIndex(u => u.IdentityId).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
