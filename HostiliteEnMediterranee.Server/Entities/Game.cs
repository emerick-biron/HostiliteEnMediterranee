namespace HostiliteEnMediterranee.Server.Entities;

public class Game
{
    public Game(Player player1, Player player2)
    {
        Players.Add(player1);
        Players.Add(player2);
    }

    public Guid Id { get; } = Guid.NewGuid();
    public List<Player> Players { get; } = [];
    private int CurrentPlayerIndex { get; set; } = 0;
    public GameStatus Status { get; private set; } = GameStatus.NotStarted;

    public void Start()
    {
        Status = GameStatus.InProgress;
    }
}