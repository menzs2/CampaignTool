using System.Net.Http.Json;
using System.Text.Json;
using Shared;

namespace Frontend;

public class ConnectionDataService : BaseDataService
{
    public ConnectionDataService(HttpClient httpClient) : base(httpClient) { }

    #region Connections

    public async Task<List<ConnectionDto>?> GetConnectionsAsync()
    {
        try
        {
            using var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}/api/connection");
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
            using var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}/api/connection/{id}");
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
            using var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}/api/connection/campaign/{id}");
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
            using var response = await HttpClient.PostAsync($"{HttpClient.BaseAddress}/api/connection", content);
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
            using var response = await HttpClient.PutAsync($"{HttpClient.BaseAddress}/api/connection/{id}", content);
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
            using var response = await HttpClient.DeleteAsync($"{HttpClient.BaseAddress}/api/connection/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    #endregion
}
