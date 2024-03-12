using MongoDB.Driver;
using MongoDB.Entities;
using ArchiveService.Services;
using ArchiveService.Models;
using AdService.DTOs;

namespace ArchiveService.Data
{
    public class DbInitializer
    {
        // initialise la base de données MongoDB
        public static async Task InitDb(WebApplication app)
        {
            // Initialisation de la connex a Mongo avec nom de la BDD et les parametre de connexion
            await DB.InitAsync("ArchivedAd", MongoClientSettings
                .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

            // Création des index pour la recherche sur les propriétés de l'entité Ad
            await DB.Index<Ad>()
                .Key(x => x.Title, KeyType.Text)
                .Key(x => x.Status, KeyType.Text)
                .Key(x => x.City, KeyType.Text)
                .CreateAsync();

            // Compte le nombre d'annonces dans la base de données
            var count = await DB.CountAsync<Ad>();

            // Utilisation du scope pour accéder aux services
            using var scope = app.Services.CreateScope();

            var httpClient = scope.ServiceProvider.GetRequiredService<AdServiceHttpClient>();
                try
                {
                // Récupère mes annonces à partir du AdService 
                var adDtos = await httpClient.GetAdsForSearchDb();
                Console.WriteLine(adDtos.Count + " annonces retournées depuis le service d'annonce");

                // Si des annonces sont disponibles je les sauvegarde dans la base de donnée Mongo
                if (adDtos.Count > 0)
                {
                    var archivedAds = adDtos.Select(adDto => new ArchivedAd
                    {
                        IdAnnonce = adDto.IdAnnonce,
                        Title = adDto.Title,
                        Status = adDto.Status,
                        PublicationDate = adDto.PublicationDate,
                        UpdatedAt = adDto.UpdatedAt,
                        Price = adDto.Price,
                        NumberOfRooms = adDto.NumberOfRooms,
                        City = adDto.City
                    }).ToList();

                    await DB.SaveAsync(archivedAds);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"erreur lors de la récupération des annonces: {ex.Message}");   
            }
        }
    }
}

