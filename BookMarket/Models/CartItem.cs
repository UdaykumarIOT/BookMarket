using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMarket.Models
{
    public class CartItem
    {
        [Key]
        public Guid CartItemId { get; set; }
        public Guid TitleId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [ForeignKey(nameof(TitleId))]
        [ValidateNever]
        public Title? Title { get; set; }
    }
}
