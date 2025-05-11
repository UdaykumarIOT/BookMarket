using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookMarket.Models;

namespace BookMarket.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var adminUser = new ApplicationUser
            {
                Id = "100",
                UserName = "admin@bookmarket.com",
                NormalizedUserName = "ADMIN@BOOKMARKET.COM",
                Email = "admin@bookmarket.com",
                NormalizedEmail = "ADMIN@BOOKMARKET.COM",
                EmailConfirmed = true,
                SecurityStamp = "12345678-90ab-cdef-1234-567890abcdef",
            };

            var hasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@2025");

            builder.HasData(adminUser);
        }
    }
}
