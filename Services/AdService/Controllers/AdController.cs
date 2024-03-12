using AdService.Data;
using AdService.DTOs;
using AdService.Models;
using AdService.Helpers;
using AutoMapper;
using Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdService.Controllers
{
    [Route("api/[controller]")]
    public class AdController : Controller
    {
        private readonly ImmoIaDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        // Construct
        public AdController(ImmoIaDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _context = dbContext;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        // Récupérer toutes les annonces
        [HttpGet]
        public async Task<ActionResult<List<AdDto>>> GetAllAds()
        {
            // Récupere toutes les annonces triée par date de mise à jour
            var ads = await _context.Ads
                .OrderBy(a => a.UpdatedAt)
                .ToListAsync();

            // Mapp les annonces vers AdDto et renvoyer la liste
            return _mapper.Map<List<AdDto>>(ads);
        }

        // Récupere une annonce par son id
        [HttpGet("{id}")]
        public async Task<ActionResult<AdDto>> GetAdById(int id)
        {
            // Trouve l'annonce par son IdAnnonce
            var findAd = await _context.Ads.FirstOrDefaultAsync(ad => ad.IdAnnonce == id);

            // Si l'annonce n'est pas trouvée retourne NotFound
            if (findAd == null)
                return NotFound();

            // Mapp l'annonce trouvée vers AdDto et renvoye le résultat
            return _mapper.Map<AdDto>(findAd);
        }

        // Mett a jour une annonce
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAd(int id, UpdateAdDto updateAdDto)
        {
            // Trouve l'annonce à mettre à jour par son identifiant
            var ad = await _context.Ads.FirstOrDefaultAsync(x => x.IdAnnonce == id);

            // Si l'annonce n'est pas trouvée retourne NotFound
            if (ad == null)
                return NotFound();
                
            // MAJ des propriétés de l'annonce avec les nouvelles valeurs
            ad.Title = updateAdDto.Title ?? ad.Title;
            ad.Status = updateAdDto.Status ?? ad.Status;
            ad.Price = updateAdDto.Price == 0 ? ad.Price : updateAdDto.Price;
            ad.UpdatedAt = DateTime.UtcNow;

            // Publie un évent AdUpdated via MassTransit
            await _publishEndpoint.Publish(_mapper.Map<AdUpdated>(ad));

            // Sauvegarde les modifications dans la base de données
            var result = await _context.SaveChangesAsync() > 0;

            // Retourne une réponse OK si les modifications sont enregistrées avec succès sinon BadRequest
            if (result)
                return Ok();

            return BadRequest("Problem saving changes");
        }

        // Supprimer une annonce
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAd(int id)
        {
            try
            {
                // Trouve l'annonce a supprimer par son id
                var ad = await _context.Ads.FindAsync(id);

                // Si l'anonce n'est pas trouvée retourne  NotFound
                if (ad == null)
                    return NotFound();

                // Supprime l'annonce de la base de données
                _context.Ads.Remove(ad);

                // Affiche l'ID de l'annonce  avant la publication
                Console.WriteLine($"Deleting Ad with ID: {id}");

                // Publie un event AdDeleted via MassTransit
                await _publishEndpoint.Publish<AdDeleted>(new { IdAnnonce = id.ToString() });
                // Sauvegarde des modif dans la base de données
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                    return BadRequest("Could not update DB");

                return Ok();
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Exception: {ex.Message}");
                throw; 
            }
        }
    }
}
