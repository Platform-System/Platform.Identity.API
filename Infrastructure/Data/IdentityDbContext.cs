using Microsoft.EntityFrameworkCore;
using Platform.BuildingBlocks.Abstractions;
using Platform.Identity.API.Infrastructure.Persistence.Models;
using Platform.Infrastructure.Data;

namespace Platform.Identity.API.Infrastructure.Data
{
    public class IdentityDbContext : BaseDbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options, ICurrentUserProvider? currentUserProvider = null) : base(options, currentUserProvider)
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
