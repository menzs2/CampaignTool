using Shared.DTO;
using System.Net.Http.Json;

namespace Frontend
{
    public class AuthenticationService : BaseDataService
    {
        public AuthenticationService(HttpClient httpClient) : base(httpClient){}

        private readonly string baseRoute = "api/auth";

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{HttpClient.BaseAddress}{baseRoute}/login", new { username, password });
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegisterAsync(RegisterRequestWithRoleDto registerRequest)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{HttpClient.BaseAddress}{baseRoute}/register", registerRequest);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration Error: {ex.Message}");
                return false;
            }
        }
    }
}
