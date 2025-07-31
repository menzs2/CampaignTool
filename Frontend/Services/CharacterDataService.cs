using System.Net.Http.Json;
using System.Text.Json;
using Shared;

namespace Frontend;

public class CharacterDataService : BaseDataService
{
    public CharacterDataService(HttpClient httpClient) : base(httpClient) { }

    public async Task<List<CharacterDto>?> GetCharacterDtosAsync()
    {
        try
        {
            var response = await HttpClient.GetAsync("http://localhost:5043/api/character");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var characters = JsonSerializer.Deserialize<List<CharacterDto>>(json, SerializerOptions);
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
            var response = await HttpClient.GetAsync($"http://localhost:5043/api/character/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var character = JsonSerializer.Deserialize<List<CharacterDto>>(json, SerializerOptions);
            return character?.FirstOrDefault();
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
            var response = await HttpClient.GetAsync($"http://localhost:5043/api/character/campaign/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var characters = JsonSerializer.Deserialize<List<CharacterDto>>(json, SerializerOptions);
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
            var response = await HttpClient.GetAsync($"http://localhost:5043/api/character/connected/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var characters = JsonSerializer.Deserialize<List<CharacterDto>>(json, SerializerOptions);
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
            using var respone = await HttpClient.PostAsJsonAsync("http://localhost:5043/api/character", content);
            respone.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task PutCharacterAsyinc(long id, CharacterDto character)
    {
        var content = JsonContent.Create(character);
        try
        {
            using var respone = await HttpClient.PutAsJsonAsync($"http://localhost:5043/api/character/{id}", content);
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
            using var respone = await HttpClient.DeleteAsync($"http://localhost:5043/api/character/{id}");
            respone.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
