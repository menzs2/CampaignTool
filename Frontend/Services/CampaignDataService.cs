using System.Text.Json;
using Shared;

namespace Frontend;

public class CampaignDataService : BaseDataService
{
    public CampaignDataService(HttpClient httpClient) : base(httpClient) { }

    public async Task<List<CampaignDto>?> GetCampaigns()
    {
        try
        {
            using var response = await HttpClient.GetAsync("http://localhost:5043/api/campaign");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var campaigns = JsonSerializer.Deserialize<List<CampaignDto>>(json, SerializerOptions);
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
            var response = await HttpClient.GetAsync("http://localhost:5043/api/campaign");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var campaigns = JsonSerializer.Deserialize<List<CampaignDto>>(json, SerializerOptions);
            return campaigns;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }
}
