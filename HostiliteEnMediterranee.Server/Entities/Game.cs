using System.Text;

namespace HostiliteEnMediterranee.Server.Entities;

public class Game
{
    public readonly Guid Id = Guid.NewGuid();
    public readonly List<Player> Players = [];
    private readonly Stack<Shot> ShotHistory = new();

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

        // todo check players' ships
        Status = GameStatus.InProgress;
    }

    public ShotResult CurrentPlayerShot(int row, int col)
    {
        var shotResult = NextPlayer.ReceiveShot(row, col);
        ShotHistory.Push(new Shot(CurrentPlayer, NextPlayer, new Coordinates(row, col), shotResult));
        if (NextPlayer.HasLost())
        {
            Status = GameStatus.Over;
            Winner = CurrentPlayer;
        }
        else if (shotResult.HitShip == null)
        {
            SwitchCurrentPlayer();
        }

        return shotResult;
    }

    public override string ToString()
    {
        var gameInfo = new StringBuilder();
        gameInfo.AppendLine($"Game ID: {Id}");
        gameInfo.AppendLine($"Status: {Status}");
        gameInfo.AppendLine($"Current Player: {CurrentPlayer.Name}");

        if (Winner != null)
        {
            gameInfo.AppendLine($"Winner: {Winner.Name}");
        }
        else
        {
            gameInfo.AppendLine("Players:");
        }

        foreach (var player in Players)
        {
            gameInfo.AppendLine($"Player: {player.Name}");
            gameInfo.AppendLine("Grid:");
            for (var row = 0; row < Player.GridSize; row++)
            {
                for (var col = 0; col < Player.GridSize; col++)
                {
                    gameInfo.Append(player.Grid[row, col] == '\0' ? ". " : $"{player.Grid[row, col]} ");
                }

                gameInfo.AppendLine();
            }
        }

        return gameInfo.ToString();
    }

    public Shot? UndoLastShot()
    {
        if (ShotHistory.Count == 0)
        {
            return null;
        }

        var lastShot = ShotHistory.Pop();
        var (shooter, targetPlayer, targetCoordinates, shotResult) = lastShot;

        if (shotResult.HitShip == null)
        {
            targetPlayer.Grid[targetCoordinates.Row, targetCoordinates.Column] = '\0';
        }
        else
        {
            targetPlayer.Grid[targetCoordinates.Row, targetCoordinates.Column] = shotResult.HitShip.Type;
        }

        if (shooter is AIPlayer aiPlayer)
        {
            aiPlayer.AddShot(targetCoordinates);
        }

        if (shotResult.HitShip == null)
        {
            SwitchCurrentPlayer();
        }

        return lastShot;
    }
}