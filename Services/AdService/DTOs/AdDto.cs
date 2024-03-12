using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AdService.Models;

namespace AdService.DTOs
{
    public class AdDto
    {
        public int IdAnnonce { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime PublicationDate { get; set; }
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
        public string City { get; set; }
    }
}