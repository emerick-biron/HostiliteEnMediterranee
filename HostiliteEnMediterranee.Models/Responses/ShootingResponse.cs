using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Models.Responses;

public class ShootingResponse(
    GameStatusDto GameStatus,
    string? WinnerName,
    ShootingStatusDto ShootingStatus,
    List<CoordinatesDto>? OpponentShoots
);