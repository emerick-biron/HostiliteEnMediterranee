namespace HostiliteEnMediterranee.Models;

public class GameStateDto(Guid gameId, GameStatusDto gameStatus, PlayerDto currentPlayer, PlayerDto opponentPlayer)
{
    public Guid GameId { get; set; } = gameId;
    public GameStatusDto GameStatus { get; set; } = gameStatus;
    public PlayerDto CurrentPlayer { get; set; } = currentPlayer;
    public PlayerDto OpponentPlayer { get; set; } = opponentPlayer;
}