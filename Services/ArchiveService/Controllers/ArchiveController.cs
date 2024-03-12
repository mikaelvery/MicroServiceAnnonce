using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using ArchiveService.Services;
using AdService.DTOs;
using ArchiveService.Models;

namespace ArchiveService.Controllers
{
    [ApiController]
    [Route("api/archive")]
    public class ArchiveController : ControllerBase
    {
        private readonly AdServiceHttpClient _adServiceHttpClient;

        public ArchiveController(AdServiceHttpClient adServiceHttpClient)
        {
            _adServiceHttpClient = adServiceHttpClient;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> ArchiveAd(int id)
        {
            try
            {
                // Récup de l'annonce depuis le service Ad
                var adDto = await _adServiceHttpClient.GetAdById(id);

                if (adDto == null)
                    return NotFound("Ad not found");

                // Archiver l'annonce dans MongoDB
                await ArchiveInMongoDB(adDto);

                // Supprimer l'annonce de la base de données principale
                await _adServiceHttpClient.DeleteAd(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
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
                Console.WriteLine($"Error archiving ad in MongoDB: {ex.Message}");
                throw;
            }
        }
    }
}
