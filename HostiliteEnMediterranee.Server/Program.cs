using HostiliteEnMediterranee.Server.Repositories;
using HostiliteEnMediterranee.Server.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<GameRepository>();
builder.Services.AddTransient<GameService>();

var app = builder.Build();


app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapPost("/games/start", ([FromServices] GameService gameService) =>
{
    var response = gameService.StartGame();
    return Results.Ok(response);
});

app.UsePathBase("/api");
app.Run();