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

    #endregion
}
