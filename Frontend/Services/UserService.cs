using Microsoft.JSInterop;
using Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Frontend
{
    public class UserService : BaseDataService
    {
        private readonly string baseRoute = "api/user";

        public UserService(HttpClient httpClient, IJSRuntime jsRuntime) : base(httpClient, jsRuntime)
        {
        }

        public async Task<List<UserDto>?> GetUsersAsync()
        {
            try
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
                var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}");
                response.EnsureSuccessStatusCode();
                var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task<UserDto?> GetUserAsync(long id)
        {
            try
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
                var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/{id}");
                response.EnsureSuccessStatusCode();
                var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
                return users?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }


        public async Task PostUserAsync(UserDto user)
        {
            var content = JsonContent.Create(user);
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

        public async Task PutUserAsync(long id, UserDto user)
        {
            var content = JsonContent.Create(user);
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

        public async Task DeleteUserAsync(long id)
        {
            try
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
                var response = await HttpClient.DeleteAsync($"{HttpClient.BaseAddress}{baseRoute}/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
