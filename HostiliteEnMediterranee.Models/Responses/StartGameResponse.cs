using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Models.Responses;

public class StartGameResponse(Guid GameId, List<ShipDto> PlayerShips);