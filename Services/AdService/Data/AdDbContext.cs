using System;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using AdService.Models;

namespace AdService.Data
{
    public class ImmoIaDbContext : DbContext
    {
        public ImmoIaDbContext(DbContextOptions<ImmoIaDbContext> options) : base(options) { }

        public DbSet<Ad> Ads { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Add in-memory outbox
            builder.AddInboxStateEntity();
            builder.AddOutboxMessageEntity();
            builder.AddOutboxStateEntity();

            // Test de HAS nokey 
            // builder.Entity<Ad>().HasNoKey();
            // // Crée un seed de data
            builder.Entity<Ad>().HasData(new Ad
            {
                IdAnnonce = -1,
                Title = "Appartement centre ville",
                Status = "Disponible",
                PublicationDate = DateTime.Now,
                Price = 300000m,
                NumberOfRooms = 3,
                City = "Montpellier"
            });

        }
    }
}