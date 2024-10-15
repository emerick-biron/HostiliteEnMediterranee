using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Models.Requests;

public class ShootingRequest(Guid gameId, CoordinatesDto shootCoordinates)
{
    public Guid GameId { get; set; } = gameId;
    public CoordinatesDto ShootCoordinates { get; set; } = shootCoordinates;
}