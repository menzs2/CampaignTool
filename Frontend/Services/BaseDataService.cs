using System.Net.Http.Json;
using System.Text.Json;
using Shared;

namespace Frontend;

public abstract class BaseDataService
{
    protected HttpClient HttpClient { get; set; }

    protected JsonSerializerOptions SerializerOptions {get; set;}

    public BaseDataService(HttpClient httpClient)
    {
        HttpClient = httpClient;
        SerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
    }
}
