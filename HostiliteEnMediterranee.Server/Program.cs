using FluentValidation;
using HostiliteEnMediterranee.Models.Requests;
using HostiliteEnMediterranee.Models.Responses;
using HostiliteEnMediterranee.Server.Repositories;
using HostiliteEnMediterranee.Server.Services;
using HostiliteEnMediterranee.Server.Validators;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
builder.Services.AddValidatorsFromAssemblyContaining<ShootingRequestValidator>();
builder.Services.AddGrpc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    var logger = app.Logger;
    logger.LogInformation("Request received: {Method} {BASE}{Path}", context.Request.Method, context.Request.PathBase,
        context.Request.Path);
    await next.Invoke();
    logger.LogInformation("Response status: {StatusCode}", context.Response.StatusCode);
});

app.UseCors();
app.UsePathBase("/api");

app.MapPost("/games/start", (
    [FromServices] GameService gameService,
    [FromBody] StartGameRequest request,
    [FromServices] IValidator<StartGameRequest> validator
) =>
{
    var validationResult = validator.Validate(request);

    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
    }

    var response = gameService.StartGame(request);
    return Results.Ok(response);
});

app.MapPost("/games/{gameId:guid}/shoots", (
        [FromServices] GameService gameService,
        [FromRoute] Guid gameId,
        [FromBody] ShootingRequest request,
        [FromServices] IValidator<ShootingRequest> validator
    ) =>
    {
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }


        var response = gameService.Shoot(gameId, request);
        return Results.Ok(response);
    })
    .Produces<ShootingResponse>();

app.MapPost("/games/{gameId:guid}/turns/undo", (
        [FromServices] GameService gameService,
        [FromRoute] Guid gameId
    ) =>
    {
        var response = gameService.UndoLastPlayerTurn(gameId);
        return Results.Ok(response);
    })
    .Produces<UndoLastPlayerTurnResponse>();

app.MapGrpcService<GameServiceGRPC>();

app.Run();