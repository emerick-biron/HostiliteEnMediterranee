using HostiliteEnMediterranee.Models.Dto;
using HostiliteEnMediterranee.Models.Requests;
using HostiliteEnMediterranee.Models.Responses;
using HostiliteEnMediterranee.Server.Entities;
using HostiliteEnMediterranee.Server.Exceptions;
using HostiliteEnMediterranee.Server.Mappers;
using HostiliteEnMediterranee.Server.Repositories;

namespace HostiliteEnMediterranee.Server.Services;

public class GameService(GameRepository gameRepository)
{
    public StartGameResponse StartGame()
    {
        var player = new Player("Player");
        var ia = new Player("IA");
        var game = new Game(player, ia);
        gameRepository.Save(game);
        game.Start();

        var playerShips = new List<ShipDto>();
        for (var row = 0; row < Player.GridSize; row++)
        for (var col = 0; col < Player.GridSize; col++)
        {
            var cell = player.Grid[row, col];
            if (cell != '\0')
            {
                var ship = playerShips.FirstOrDefault(s => s.Model == cell);
                if (ship == null)
                {
                    ship = new ShipDto(cell, [new CoordinatesDto(row, col)]);
                    playerShips.Add(ship);
                }
                else
                {
                    ship.Coordinates.Add(new CoordinatesDto(row, col));
                }
            }
        }

        return new StartGameResponse(
            game.Id,
            playerShips
        );
    }

    public ShootingResponse Attack(ShootingRequest shootingRequest)
    {
        var game = gameRepository.FindById(shootingRequest.GameId) ?? throw new GameNotFoundException("Game not found");

        var shootCoordinates = shootingRequest.ShootCoordinates;
        var playerHit = game.CurrentPlayerShot(shootCoordinates.Row, shootCoordinates.Column);

        // if (playerHit)
        // {
        return new ShootingResponse(
            GameStatus: game.Status.ToDto(),
            WinnerName: game.Winner?.Name,
            HasHit: playerHit,
            OpponentShoots: []
        );
        // }
    }
}