namespace HostiliteEnMediterranee.Models;

public class GameStateDto
{
    public Guid GameId { get; set; }
    public GameStatusDto GameStatus { get; set; }
    public PlayerDto CurrentPlayer { get; set; }
    public PlayerDto OpponentPlayer { get; set; }
}