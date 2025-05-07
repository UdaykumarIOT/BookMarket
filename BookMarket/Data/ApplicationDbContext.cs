using BookMarket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookMarket.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            base.OnModelCreating(modelBuilder);

            var admin = new IdentityRole
            {
                Id = "1",
                Name = "admin",
                NormalizedName = "ADMIN"
            };

            var user = new IdentityRole
            {
                Id = "2",
                Name = "user",
                NormalizedName = "USER"
            };

            var seller = new IdentityRole
            {
                Id = "3",
                Name = "seller",
                NormalizedName = "SELLER"
            };

            modelBuilder.Entity<IdentityRole>().HasData(admin, user, seller);
        }
    }
}
