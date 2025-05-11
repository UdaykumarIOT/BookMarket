using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;

namespace BookMarket.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            // Assign the admin user to the 'admin' role.
            builder.HasData(new IdentityUserRole<string>
            {
                UserId = "100",  // Admin User ID
                RoleId = "1"     // Admin Role ID
            });
        }
    }
}
