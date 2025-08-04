using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.SessionStorage;
using Frontend;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5043") });
builder.Services.AddScoped<CampaignDataService>();
builder.Services.AddScoped<CharacterDataService>();
builder.Services.AddScoped<ConnectionDataService>();
builder.Services.AddBlazoredSessionStorage();

await builder.Build().RunAsync();
