using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdService.Models
{
    public class Ad
    {
        [Key]
        public int IdAnnonce { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int NumberOfRooms { get; set; }

        [Required]
        public string City { get; set; }
    }
}