using System.Net.Http.Json;
using Shared;

namespace Frontend
{
    public class OrganisationDataService : BaseDataService
    {
        public OrganisationDataService(HttpClient httpClient) : base(httpClient) { }

        public async Task<List<OrganisationDto>?> GetOrganisationDtosAsync()
        {
            try
            {
                var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}api/organisation");
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
                var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}api/Oorganisation/{id}");
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
                var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}api/organisation/campaign/{id}");
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
                var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}api/organisation/connected/{id}");
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
                using var respone = await HttpClient.PostAsJsonAsync($"{HttpClient.BaseAddress}api/organisation", content);
                respone.EnsureSuccessStatusCode();
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
                using var respone = await HttpClient.PutAsJsonAsync($"{HttpClient.BaseAddress}api/organisation/{id}", content);
                respone.EnsureSuccessStatusCode();
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
                using var respone = await HttpClient.DeleteAsync($"{HttpClient.BaseAddress}api/organisation/{id}");
                respone.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

}
