using Shared;
using System.Net.Http.Json;

namespace Frontend
{
    public class CampaignDataService : BaseDataService
    {
        public CampaignDataService(HttpClient httpClient) : base(httpClient) { }
        private readonly string baseRoute = "api/campaign";

        public async Task<List<CampaignDto>?> GetCampaigns()
        {
            try
            {
                using var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}");
                response.EnsureSuccessStatusCode();
                var campaigns = await response.Content.ReadFromJsonAsync<List<CampaignDto>>();
                return campaigns;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task<List<CampaignDto>?> GetCampaigns(int userID)
        {
            try
            {
                var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/{userID}");
                response.EnsureSuccessStatusCode();
                var campaigns = await response.Content.ReadFromJsonAsync<List<CampaignDto>>();
                return campaigns;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task<CampaignDto?> GetCampaignByIdAsync(long id)
        {
            try
            {
                using var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/{id}");
                response.EnsureSuccessStatusCode();
                var campaign = await response.Content.ReadFromJsonAsync<CampaignDto>();
                return campaign;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task<CampaignDto?> AddCampaignAsync(CampaignDto campaignDto)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{HttpClient.BaseAddress}{baseRoute}", campaignDto);
                response.EnsureSuccessStatusCode();
                var newCampaign = await response.Content.ReadFromJsonAsync<CampaignDto>();
                return newCampaign;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task<CampaignDto?> UpdateCampaignAsync(CampaignDto campaignDto)
        {
            try
            {
                var response = await HttpClient.PutAsJsonAsync($"{HttpClient.BaseAddress}{baseRoute}/{campaignDto.Id}", campaignDto);
                response.EnsureSuccessStatusCode();
                var updatedCampaign = await response.Content.ReadFromJsonAsync<CampaignDto>();
                return updatedCampaign;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> DeleteCampaignAsync(long id)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"{HttpClient.BaseAddress}{baseRoute}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return false;
        }
    }
}
