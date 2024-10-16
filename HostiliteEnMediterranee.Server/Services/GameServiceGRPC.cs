using Grpc.Core;
using HostiliteEnMediterranee.Proto;
using HostiliteEnMediterranee.Server.Entities;
using HostiliteEnMediterranee.Server.Exceptions;
using HostiliteEnMediterranee.Server.Mappers;
using HostiliteEnMediterranee.Server.Repositories;

namespace HostiliteEnMediterranee.Server.Services;

// ReSharper disable once InconsistentNaming
public class GameServiceGRPC(GameRepository gameRepository, ILogger<GameService> logger)
    : Proto.GameService.GameServiceBase
{
    public override Task<StartGameResponse> StartGame(Empty request, ServerCallContext context)
    {
        var player = new Player("Player");
        var ia = new AIPlayer("IA");
        player.GenerateRandomGrid(Ship.Ships);
        ia.GenerateRandomGrid(Ship.Ships);
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
                    var ship = playerShips.FirstOrDefault(s => s.Model[0] == cell);
                    var coordinates = new CoordinatesDto
                    {
                        Row = row,
                        Column = col
                    };
                    if (ship == null)
                    {
                        ship = new ShipDto
                        {
                            Model = cell.ToString(),
                            Coordinates = { coordinates }
                        };
                        playerShips.Add(ship);
                    }
                    else
                    {
                        ship.Coordinates.Add(coordinates);
                    }
                }
            }
        }

        logger.LogInformation("Game started:\n{GameInfo}", game.ToString());

        return Task.FromResult(new StartGameResponse
        {
            GameId = game.Id.ToString(),
            PlayerShips = { playerShips }
        });
    }

    public override Task<ShootingResponse> Shoot(ShootingRequest request, ServerCallContext context)
    {
        var gameId = Guid.Parse(request.GameId);
        var game = gameRepository.FindById(gameId) ?? throw new GameNotFoundException("Game not found");

        logger.LogInformation("Shooting attempt by {PlayerName} in game {GameId}", game.CurrentPlayer.Name, gameId);

        var shootCoordinates = request.ShootCoordinates;
        var shotResult = game.CurrentPlayerShot(shootCoordinates.Row, shootCoordinates.Column);

        logger.LogInformation("Shot at ({Row}, {Col}) was a {Result}", shootCoordinates.Row, shootCoordinates.Column,
            shotResult.HasHit ? "hit" : "miss");

        if (shotResult.HasHit)
        {
            logger.LogInformation("Current game status:\n{GameInfo}", game.ToString());
            return Task.FromResult(new ShootingResponse
            {
                GameStatus = game.Status.ToProto(),
                WinnerName = game.Winner?.Name ?? string.Empty,
                HasHit = shotResult.HasHit,
                OpponentShoots = { }
            });
        }

        var opponentShots = new List<CoordinatesDto>();

        if (game.CurrentPlayer is AIPlayer aiPlayer)
        {
            ShotResult aiShotResult;
            do
            {
                var shot = aiPlayer.GetNextShot();
                aiShotResult = game.CurrentPlayerShot(shot.Row, shot.Column);
                opponentShots.Add(new CoordinatesDto
                {
                    Row = shot.Row,
                    Column = shot.Column
                });
                logger.LogInformation("AI shot at ({Row}, {Col}) was a {Result}", shot.Row, shot.Column,
                    aiShotResult.HasHit ? "hit" : "miss");
            } while (aiShotResult.HasHit && game.Status != GameStatus.Over);
        }

        logger.LogInformation("Current game status:\n{GameInfo}", game.ToString());
        return Task.FromResult(new ShootingResponse
        {
            GameStatus = game.Status.ToProto(),
            WinnerName = game.Winner?.Name,
            HasHit = shotResult.HasHit,
            OpponentShoots = { opponentShots }
        });
    }
}