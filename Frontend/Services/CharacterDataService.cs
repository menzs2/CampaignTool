using Shared;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
namespace Frontend;

public class CharacterDataService : BaseDataService
{
    public CharacterDataService(HttpClient httpClient, IJSRuntime jsRuntime) : base(httpClient, jsRuntime) { }
    private readonly string baseRoute = "api/character";

    public async Task<List<CharacterDto>?> GetCharacterDtosAsync()
    {
        try
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
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
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
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
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
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
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
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
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
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
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
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
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
            using var response = await HttpClient.DeleteAsync($"{HttpClient.BaseAddress}{baseRoute}/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
