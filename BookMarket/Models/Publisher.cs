using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookMarket.Models
{
    public class Publisher
    {
        [Key]
        public Guid PubId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Publisher Name")]
        public string PubName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        public virtual ICollection<Title> Titles { get; set; } = new List<Title>();
    }
}
