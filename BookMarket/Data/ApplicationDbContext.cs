using BookMarket.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMarket.Data
{
    public class ApplicationDbContext:DbContext
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
                .HasForeignKey(t => t.PubId);
            modelBuilder.Entity<Title>()
                .HasOne(t => t.Author)
                .WithMany(a => a.Titles)
                .HasForeignKey(t => t.AuthId);
        }
    }
}
