using Microsoft.EntityFrameworkCore;
using Platform.BuildingBlocks.Abstractions;
using Platform.Identity.API.Domain;
using Platform.Identity.API.Infrastructure.Persistence.Models;
using Platform.Infrastructure.Data;

namespace Platform.Identity.API.Infrastructure.Data
{
    public class IdentityDbContext : BaseDbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options, IDateTimeProvider dateTimeProvider, string? currentUserId = null) : base(options, dateTimeProvider, currentUserId)
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
