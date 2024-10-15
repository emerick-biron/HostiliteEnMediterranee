using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Models.Requests;

public record ShootingRequest(Guid GameId, CoordinatesDto ShootCoordinates);