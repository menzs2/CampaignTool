using Blazored.SessionStorage;
using Frontend;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5043") });
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<CampaignDataService>();
builder.Services.AddScoped<CharacterDataService>();
builder.Services.AddScoped<ConnectionDataService>();
builder.Services.AddScoped<OrganisationDataService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddBlazoredSessionStorage();

await builder.Build().RunAsync();
