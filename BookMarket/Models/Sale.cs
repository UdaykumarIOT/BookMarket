using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookMarket.Models
{
    public class Sale
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TitleId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(TitleId))]
        [ValidateNever]
        public Title? Title { get; set; }
    }
}
