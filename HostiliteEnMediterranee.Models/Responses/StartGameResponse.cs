using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Models.Responses;

public record StartGameResponse(Guid GameId, List<ShipDto> PlayerShips);