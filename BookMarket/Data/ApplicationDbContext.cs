using BookMarket.Models;
using Microsoft.EntityFrameworkCore;
using BookMarket.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BookMarket.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Title> Titles { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply Configurations
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

            // Define relationships for Title, Author, and Publisher
            modelBuilder.Entity<Title>()
                .HasOne(t => t.Publisher)
                .WithMany(p => p.Titles)
                .HasForeignKey(t => t.PubId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Title>()
                .HasOne(t => t.Author)
                .WithMany(a => a.Titles)
                .HasForeignKey(t => t.AuthId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
