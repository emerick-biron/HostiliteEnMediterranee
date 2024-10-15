using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Server.Entities;

public class Game
{
    private readonly List<CoordinatesDto> _aiPossibleShots = [];
    public readonly Guid Id = Guid.NewGuid();
    public readonly List<Player> Players = [];

    public Game(Player player1, Player player2)
    {
        Players.Add(player1);
        Players.Add(player2);

        var possibleShots = new List<CoordinatesDto>();
        for (var row = 0; row < Player.GridSize; row++)
        {
            for (var col = 0; col < Player.GridSize; col++)
            {
                possibleShots.Add(new CoordinatesDto(row, col));
            }
        }

        _aiPossibleShots.AddRange(possibleShots.OrderBy(_ => Random.Shared.Next()).ToList());
    }

    public Player? Winner { get; private set; }

    private int CurrentPlayerIndex { get; set; } = 0;
    public GameStatus Status { get; private set; } = GameStatus.NotStarted;

    public void Start()
    {
        foreach (var player in Players)
        {
            player.GenerateRandomGrid(Ship.Ships);
        }

        Status = GameStatus.InProgress;
    }

    public Player GetCurrentPlayer()
    {
        return Players[CurrentPlayerIndex];
    }

    public Player GetOpponentPlayer()
    {
        return Players[(CurrentPlayerIndex + 1) % 2];
    }

    public void NextTurn()
    {
        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % 2;
    }
}