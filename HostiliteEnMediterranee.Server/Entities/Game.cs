namespace HostiliteEnMediterranee.Server.Entities;

public class Game
{
    public readonly Guid Id = Guid.NewGuid();
    public readonly List<Player> Players = [];

    public Game(Player player1, Player player2)
    {
        Players.Add(player1);
        Players.Add(player2);
    }

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