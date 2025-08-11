using System.Net.Http.Json;
using Shared;

namespace Frontend;

public class CharacterDataService : BaseDataService
{
    public CharacterDataService(HttpClient httpClient) : base(httpClient) { }
    private readonly string baseRoute = "api/character";


    public async Task<List<CharacterDto>?> GetCharacterDtosAsync()
    {
        try
        {
            var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}");
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
            var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/{id}");
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
            var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/campaign/{id}");
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
            var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/connected/{id}");
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
            using var response = await HttpClient.PostAsJsonAsync($"{HttpClient.BaseAddress}{baseRoute}", content);
            response.EnsureSuccessStatusCode();
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
            using var response = await HttpClient.PutAsJsonAsync($"{HttpClient.BaseAddress}{baseRoute}/{id}", content);
            response.EnsureSuccessStatusCode();
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
            using var response = await HttpClient.DeleteAsync($"{HttpClient.BaseAddress}{baseRoute}/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
