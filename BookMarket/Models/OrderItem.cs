using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookMarket.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid TitleId { get; set; }

        [Required]
        public int Quantity { get; set; }
        public string? UserId { get; set; } 

        [ForeignKey(nameof(TitleId))]
        [ValidateNever]
        public Title? Title { get; set; }

        [ForeignKey(nameof(UserId))]
        [ValidateNever]
        public ApplicationUser? User { get; set; }  
    }
}
