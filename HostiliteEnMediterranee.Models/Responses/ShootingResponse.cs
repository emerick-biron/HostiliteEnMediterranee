using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Models.Responses;

public record ShootingResponse(
    GameStatusDto GameStatus,
    string? WinnerName,
    ShootingStatusDto ShootingStatus,
    List<CoordinatesDto>? OpponentShoots
);