using System.Collections.Generic;
using System.Net.Http;
using MongoDB.Entities;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AdService.DTOs;
using System.Net;
using ArchiveService.Models;

namespace ArchiveService.Services
{
    public class AdServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<AdServiceHttpClient> _logger;

        public AdServiceHttpClient(HttpClient httpClient, IConfiguration config, ILogger<AdServiceHttpClient> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        public async Task<List<Ad>> GetAdsForSearchDb()
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:7001/api/ads");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Ad>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des annonces depuis le service distant.");
                throw;
            }
        }


        public async Task DeleteAd(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/ad/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to delete ad with id {id}. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error during HTTP request to delete ad.");
                throw;
            }
        }

        public async Task<AdDto> GetAdById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ad/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var ad = await response.Content.ReadFromJsonAsync<AdDto>();
                    return ad;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw new HttpRequestException($"Unexpected status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error during HTTP request to the Ad service.");
                throw;
            }
        }
    }
}
