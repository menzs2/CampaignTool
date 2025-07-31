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
}
