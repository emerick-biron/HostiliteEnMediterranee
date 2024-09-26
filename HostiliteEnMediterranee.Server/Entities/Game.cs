namespace HostiliteEnMediterranee.Server.Entities;

public class Game
{
    public Guid Id { get; } = Guid.NewGuid();
    public List<Player> Players { get; } = [];
    private int currentPlayerIndex { get; set; } = 0;
    public GameStatus Status { get; private set; } = GameStatus.NotStarted;

    public Game()
    {
    }
}