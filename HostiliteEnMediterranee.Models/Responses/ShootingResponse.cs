using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Models.Responses;

public record ShootingResponse(
    GameStatusDto GameStatus,
    string? WinnerName,
    bool HasHit,
    List<CoordinatesDto> OpponentShoots,
    ShipDto? OpponentShipSunk
);