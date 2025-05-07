using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookMarket.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; }

        public ApplicationUser()
        {
            CreatedAt = DateTime.UtcNow;  
        }
    }
}
