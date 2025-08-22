using Shared.DTO;
using System.Net.Http.Json;
using Microsoft.JSInterop;
namespace Frontend
{
    public class AuthenticationService : BaseDataService
    {
        public AuthenticationService(HttpClient httpClient, IJSRuntime jsRuntime) : base(httpClient, jsRuntime) { }

        private readonly string baseRoute = "api/auth";

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
        
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{HttpClient.BaseAddress}{baseRoute}/login", loginRequest);
                response.EnsureSuccessStatusCode();
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                return loginResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Error: {ex.Message}");
                return null;
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
