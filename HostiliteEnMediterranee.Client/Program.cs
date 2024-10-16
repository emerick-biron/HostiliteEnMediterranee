using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using HostiliteEnMediterranee.Client;
using HostiliteEnMediterranee.Client.Services;
using HostiliteEnMediterranee.Proto;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://159.31.77.255:8080/") });
builder.Services.AddScoped(sp =>
{
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var channel = GrpcChannel.ForAddress("http://159.31.77.255:8081/", new GrpcChannelOptions { HttpClient = httpClient });
    return new GameService.GameServiceClient(channel);
});
builder.Services.AddScoped<GameApiService>();
builder.Services.AddSingleton<GameState>();


await builder.Build().RunAsync();