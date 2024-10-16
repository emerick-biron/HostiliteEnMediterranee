using HostiliteEnMediterranee.Models.Dto;
using HostiliteEnMediterranee.Models.Requests;
using HostiliteEnMediterranee.Models.Responses;
using HostiliteEnMediterranee.Server.Entities;
using HostiliteEnMediterranee.Server.Exceptions;
using HostiliteEnMediterranee.Server.Mappers;
using HostiliteEnMediterranee.Server.Repositories;

namespace HostiliteEnMediterranee.Server.Services;

public class GameService(GameRepository gameRepository, ILogger<GameService> logger)
{
    public StartGameResponse StartGame()
    {
        var player = new Player("Player");
        var ia = new AIPlayer("IA");
        var game = new Game(player, ia);
        gameRepository.Save(game);
        game.Start();

        var playerShips = new List<ShipDto>();
        for (var row = 0; row < Player.GridSize; row++)
        {
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
        }

        logger.LogInformation("Game started:\n{GameInfo}", game.ToString());

        return new StartGameResponse(
            game.Id,
            playerShips
        );
    }

    public ShootingResponse Shoot(Guid gameId, ShootingRequest shootingRequest)
    {
        var game = gameRepository.FindById(gameId) ?? throw new GameNotFoundException("Game not found");

        logger.LogInformation("Shooting attempt by {PlayerName} in game {GameId}", game.CurrentPlayer.Name, gameId);

        var shootCoordinates = shootingRequest.ShootCoordinates;
        var playerHit = game.CurrentPlayerShot(shootCoordinates.Row, shootCoordinates.Column);

        logger.LogInformation("Shot at ({Row}, {Col}) was a {Result}", shootCoordinates.Row, shootCoordinates.Column,
            playerHit ? "hit" : "miss");

        if (playerHit)
        {
            logger.LogInformation("Current game status:\n{GameInfo}", game.ToString());
            return new ShootingResponse(
                GameStatus: game.Status.ToDto(),
                WinnerName: game.Winner?.Name,
                HasHit: playerHit,
                OpponentShoots: []
            );
        }

        var opponentShots = new List<CoordinatesDto>();

        if (game.CurrentPlayer is AIPlayer aiPlayer)
        {
            bool aiHit;
            do
            {
                var shot = aiPlayer.GetNextShot();
                aiHit = game.CurrentPlayerShot(shot.Row, shot.Column);
                opponentShots.Add(new CoordinatesDto(shot.Row, shot.Column));
                logger.LogInformation("AI shot at ({Row}, {Col}) was a {Result}", shot.Row, shot.Column,
                    aiHit ? "hit" : "miss");
            } while (aiHit && game.Status != GameStatus.Over);
        }

        logger.LogInformation("Current game status:\n{GameInfo}", game.ToString());
        return new ShootingResponse(
            GameStatus: game.Status.ToDto(),
            WinnerName: game.Winner?.Name,
            HasHit: playerHit,
            OpponentShoots: opponentShots
        );
    }
}