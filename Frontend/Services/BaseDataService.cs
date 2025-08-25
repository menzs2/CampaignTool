using Microsoft.JSInterop;
using System.Text.Json;
namespace Frontend
{
    public abstract class BaseDataService
    {
        protected HttpClient HttpClient { get; set; }
        protected IJSRuntime JSRuntime { get; set; }
        protected JsonSerializerOptions SerializerOptions { get; set; }

        public BaseDataService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            HttpClient = httpClient;
            JSRuntime = jsRuntime;
            SerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
    }
}
