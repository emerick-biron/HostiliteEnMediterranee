using HostiliteEnMediterranee.Client;
using HostiliteEnMediterranee.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://159.31.77.255:8080/") });
builder.Services.AddScoped<GameApiService>();
builder.Services.AddSingleton<GameState>();


await builder.Build().RunAsync();