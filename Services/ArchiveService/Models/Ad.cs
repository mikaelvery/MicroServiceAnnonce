using MongoDB.Entities;
using System;

namespace ArchiveService.Models
{

    public class Ad : Entity
    {
        public int IdAnnonce { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
        public string City { get; set; }
    }
}
