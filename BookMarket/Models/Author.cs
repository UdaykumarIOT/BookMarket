using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookMarket.Models
{
    public class Author
    {
        [Key]
        public Guid AuthId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Author Name")]
        public string AuthName { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        public virtual ICollection<Title> Titles { get; set; } = new List<Title>();
    }
}
