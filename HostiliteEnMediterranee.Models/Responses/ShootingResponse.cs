using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Models.Responses;

public class ShootingResponse(
    GameStatusDto gameStatus,
    string? winnerName,
    ShootingStatusDto shootingStatus,
    List<CoordinatesDto>? opponentShoots)
{
    public GameStatusDto GameStatus { get; set; } = gameStatus;
    public string? WinnerName { get; set; } = winnerName;
    public ShootingStatusDto ShootingStatus { get; set; } = shootingStatus;
    public List<CoordinatesDto>? OpponentShoots { get; set; } = opponentShoots;
}