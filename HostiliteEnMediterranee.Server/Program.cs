using HostiliteEnMediterranee.Models.Requests;
using HostiliteEnMediterranee.Server.Repositories;
using HostiliteEnMediterranee.Server.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<GameRepository>();
builder.Services.AddTransient<GameService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();
app.UsePathBase("/api");

app.Use(async (context, next) =>
{
    var logger = app.Logger;
    logger.LogInformation("Request received: {Method} {BASE}{Path}", context.Request.Method, context.Request.PathBase,
        context.Request.Path);
    await next.Invoke();
    logger.LogInformation("Response status: {StatusCode}", context.Response.StatusCode);
});


app.MapPost("/games/start", ([FromServices] GameService gameService) =>
{
    var response = gameService.StartGame();
    return Results.Ok(response);
});

app.MapPost("/games/{gameId:guid}/shoot", (
    [FromServices] GameService gameService,
    [FromRoute] Guid gameId,
    [FromBody] ShootingRequest request
) =>
{
    var response = gameService.Shoot(gameId, request);
    return Results.Ok(response);
});

app.Run();