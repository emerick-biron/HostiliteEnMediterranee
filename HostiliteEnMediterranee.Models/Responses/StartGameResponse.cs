using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Models.Responses;

public class StartGameResponse(Guid gameId, List<ShipDto> playerShips)
{
    public Guid GameId { get; set; } = gameId;
    public List<ShipDto> PlayerShips { get; set; } = playerShips;
}