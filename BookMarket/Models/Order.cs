using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMarket.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public List<CartItem> Items { get; set; } = new();

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
