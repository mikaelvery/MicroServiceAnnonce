using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AdService.Models;

namespace AdService.DTOs
{
    public class CreateAdDto
    {
        
        public int IdAnnonce { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int NumberOfRooms { get; set; }

        [Required]
        public string City { get; set; }
    }
}
