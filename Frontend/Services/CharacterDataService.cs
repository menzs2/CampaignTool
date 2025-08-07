using System.Net.Http.Json;
using Shared;

namespace Frontend;

public class CharacterDataService : BaseDataService
{
    public CharacterDataService(HttpClient httpClient) : base(httpClient) { }

    public async Task<List<CharacterDto>?> GetCharacterDtosAsync()
    {
        try
        {
            var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}api/character");
            response.EnsureSuccessStatusCode();
            var characters = await response.Content.ReadFromJsonAsync<List<CharacterDto>>();
            return characters;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }

    public async Task<CharacterDto?> GetCharacterDtosAsync(long id)
    {
        try
        {
            var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}api/character/{id}");
            response.EnsureSuccessStatusCode();
            var characters = await response.Content.ReadFromJsonAsync<List<CharacterDto>>();
            return characters?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }

    public async Task<List<CharacterDto>?> GetCharacterDtosByCampaignAsync(long id)
    {
        try
        {
            var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}api/character/campaign/{id}");
            response.EnsureSuccessStatusCode();
            var characters = await response.Content.ReadFromJsonAsync<List<CharacterDto>>();
            return characters;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }

    public async Task<List<CharacterDto>?> GetConnectedCharacterDtosAsync(long id)
    {
        try
        {
            var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}api/character/connected/{id}");
            response.EnsureSuccessStatusCode();
            var characters = await response.Content.ReadFromJsonAsync<List<CharacterDto>>();
            return characters;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }

    public async Task PostCharacterAsync(CharacterDto character)
    {
        var content = JsonContent.Create(character);
        try
        {
            using var respone = await HttpClient.PostAsJsonAsync($"{HttpClient.BaseAddress}api/character", content);
            respone.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task PutCharacterAsync(long id, CharacterDto character)
    {
        var content = JsonContent.Create(character);
        try
        {
            using var respone = await HttpClient.PutAsJsonAsync($"{HttpClient.BaseAddress}api/character/{id}", content);
            respone.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task DeleteCharacterAsync(long id)
    {
        try
        {
            using var respone = await HttpClient.DeleteAsync($"{HttpClient.BaseAddress}api/character/{id}");
            respone.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
