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

    private int CurrentPlayerIndex { get; set; }
    public GameStatus Status { get; private set; } = GameStatus.NotStarted;
    public Player NextPlayer => Players[(CurrentPlayerIndex + 1) % 2];
    public Player CurrentPlayer => Players[CurrentPlayerIndex];
    public Player? Winner { get; private set; }

    private void SwitchCurrentPlayer()
    {
        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % 2;
    }

    public void Start()
    {
        if (Status != GameStatus.NotStarted) return;

        foreach (var player in Players) player.GenerateRandomGrid(Ship.Ships);

        Status = GameStatus.InProgress;
    }

    public bool CurrentPlayerShot(int row, int col)
    {
        var hit = NextPlayer.ReceiveShot(row, col);
        HandlePostShot(hit);
        return hit;
    }

    private void HandlePostShot(bool hit)
    {
        if (NextPlayer.HasLost())
        {
            Status = GameStatus.Over;
            Winner = CurrentPlayer;
        }
        else if (!hit)
        {
            SwitchCurrentPlayer();
        }
    }
}