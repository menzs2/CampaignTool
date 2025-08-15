using Shared;
using System.Net.Http.Json;

namespace Frontend;

public class ConnectionDataService : BaseDataService
{
    public ConnectionDataService(HttpClient httpClient) : base(httpClient) { }

    private readonly string baseRoute = "api/connection";

    #region Connections

    public async Task<List<ConnectionDto>?> GetConnectionsAsync()
    {
        try
        {
            using var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}");
            response.EnsureSuccessStatusCode();
            var connections = await response.Content.ReadFromJsonAsync<List<ConnectionDto>>();
            return connections;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }

    public async Task<ConnectionDto?> GetConnectionAsync(long id)
    {
        try
        {
            using var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/{id}");
            response.EnsureSuccessStatusCode();
            var connection = await response.Content.ReadFromJsonAsync<List<ConnectionDto>>();
            return connection?.FirstOrDefault(); ;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }

    public async Task<List<ConnectionDto>?> GetConnectionsByCampaignAsync(long id)
    {
        try
        {
            using var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/campaign/{id}");
            response.EnsureSuccessStatusCode();
            var connections = await response.Content.ReadFromJsonAsync<List<ConnectionDto>>();
            return connections;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }

    public async Task PostConnectionAsync(ConnectionDto connection)
    {
        var content = JsonContent.Create(connection);
        try
        {
            using var response = await HttpClient.PostAsync($"{HttpClient.BaseAddress}{baseRoute}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task PutConnectionAsync(long id, ConnectionDto connection)
    {
        var content = JsonContent.Create(connection);
        try
        {
            using var response = await HttpClient.PutAsync($"{HttpClient.BaseAddress}{baseRoute}/{id}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task DeleteConnectionAsync(long id)
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

    #endregion

    #region Character to Character connections

    public async Task<List<CharCharConnectionDto>?> GetCharToCharConnectionsAsync()
    {
        try
        {
            using var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/charchar");
            response.EnsureSuccessStatusCode();
            var connections = await response.Content.ReadFromJsonAsync<List<CharCharConnectionDto>>();
            return connections;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }

    public async Task<CharCharConnectionDto?> GetCharToCharConnectionAsync(long id)
    {
        try
        {
            using var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/charchar/{id}");
            response.EnsureSuccessStatusCode();
            var connection = await response.Content.ReadFromJsonAsync<List<CharCharConnectionDto>>();
            return connection?.FirstOrDefault(); ;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }

    public async Task<List<CharCharConnectionDto>?> GetCharToCharConnectionsByCampaignAsync(long id)
    {
        try
        {
            using var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/charchar/campaign/{id}");
            response.EnsureSuccessStatusCode();
            var connections = await response.Content.ReadFromJsonAsync<List<CharCharConnectionDto>>();
            return connections;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }

    public async Task PostCharToCharConnectionAsync(CharCharConnectionDto connection)
    {
        var content = JsonContent.Create(connection);
        try
        {
            using var response = await HttpClient.PostAsync($"{HttpClient.BaseAddress}{baseRoute}/charchar", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task PutCharToCharConnectionAsync(long id, CharCharConnectionDto connection)
    {
        var content = JsonContent.Create(connection);
        try
        {
            using var response = await HttpClient.PutAsync($"{HttpClient.BaseAddress}{baseRoute}/charchar/{id}", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task DeleteCharToCharConnectionAsync(long id)
    {
        try
        {
            using var response = await HttpClient.DeleteAsync($"{HttpClient.BaseAddress}{baseRoute}/charchar/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    #endregion
}
