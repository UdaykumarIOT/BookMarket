using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookMarket.Models
{
    public class Title
    {
        [Key]
        public Guid TitleId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Book Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Book Type")]
        public string type { get; set; }

        [Required]
        [Display(Name = "Author")]
        public Guid AuthId { get; set; }

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string? Notes { get; set; }

        [Display(Name = "Book Image")]
        [ValidateNever]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Book Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Publisher")]
        public Guid PubId { get; set; }

        [Display(Name = "Publish Date")]
        public DateOnly? PubDate { get; set; }

        [ForeignKey(nameof(PubId))]
        [ValidateNever]
        public Publisher? Publisher { get; set; }

        [ForeignKey(nameof(AuthId))]
        [ValidateNever]
        public Author? Author { get; set; }
    }
}
