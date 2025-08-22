using Shared;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace Frontend
{
    public class OrganisationDataService : BaseDataService
    {
        public OrganisationDataService(HttpClient httpClient, IJSRuntime jsRuntime) : base(httpClient, jsRuntime) { }
        private readonly string baseRoute = "api/organisation";

        public async Task<List<OrganisationDto>?> GetOrganisationDtosAsync()
        {
            try
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
                var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}");
                response.EnsureSuccessStatusCode();
                var Organisations = await response.Content.ReadFromJsonAsync<List<OrganisationDto>>();
                return Organisations;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task<OrganisationDto?> GetOrganisationDtosAsync(long id)
        {
            try
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
                var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/{id}");
                response.EnsureSuccessStatusCode();
                var Organisations = await response.Content.ReadFromJsonAsync<List<OrganisationDto>>();
                return Organisations?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task<List<OrganisationDto>?> GetOrganisationDtosByCampaignAsync(long id)
        {
            try
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
                var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/campaign/{id}");
                response.EnsureSuccessStatusCode();
                var Organisations = await response.Content.ReadFromJsonAsync<List<OrganisationDto>>();
                return Organisations;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task<List<OrganisationDto>?> GetConnectedOrganisationDtosAsync(long id)
        {
            try
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken"));
                var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}{baseRoute}/connected/{id}");
                response.EnsureSuccessStatusCode();
                var Organisations = await response.Content.ReadFromJsonAsync<List<OrganisationDto>>();
                return Organisations;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task PostOrganisationAsync(OrganisationDto Organisation)
        {
            var content = JsonContent.Create(Organisation);
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

        public async Task PutOrganisationAsyinc(long id, OrganisationDto Organisation)
        {
            var content = JsonContent.Create(Organisation);
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

        public async Task DeleteOrganisationAsync(long id)
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

}
