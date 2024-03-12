using System;
using System.Threading.Tasks;
using Events;
using MassTransit;
using MongoDB.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using ArchiveService.Services;
using AdService.DTOs;
using ArchiveService.Models;

namespace ArchiveService.Consumers
{
    public class AdDeletedConsumer : IConsumer<AdDeleted>
    {
        private readonly ILogger<AdDeletedConsumer> _logger;
        private readonly AdServiceHttpClient _adServiceHttpClient;

        public AdDeletedConsumer(ILogger<AdDeletedConsumer> logger, AdServiceHttpClient adServiceHttpClient)
        {
            _logger = logger;
            _adServiceHttpClient = adServiceHttpClient;
        }

        public async Task Consume(ConsumeContext<AdDeleted> adDeleted)
        {
            try
            {
                Console.WriteLine($"Consuming ad deleted {adDeleted.Message.Id}");
                
                // Converti le GUID en string
                var adIdAsString = adDeleted.Message.Id;
                // Récupérer l'annonce depuis le service Ad
                var adDto = await _adServiceHttpClient.GetAdById(adIdAsString);

                if (adDto == null)
                {
                    _logger.LogWarning("Ad not found for deletion.");
                    return;
                }

                // Archiver l'annonce dans MongoDB
                await ArchiveInMongoDB(adDto);

                // Supprimer l'annonce de la base de données principale (MSSQL)
                await _adServiceHttpClient.DeleteAd(adIdAsString);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing AdDeleted message: {ex.Message}");
            }
        }

        private async Task ArchiveInMongoDB(AdDto adDto)
        {
            try
            {
                // Archiver dans MongoDB
                var archivedAd = new ArchivedAd
                {
                    IdAnnonce = adDto.IdAnnonce,
                    Title = adDto.Title,
                };

                await archivedAd.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error archiving ad in MongoDB: {ex.Message}");
                throw;
            }
        }
    }
}
