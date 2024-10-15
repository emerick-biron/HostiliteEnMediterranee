using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Models.Requests;

public class ShootingRequest(Guid GameId, CoordinatesDto ShootCoordinates);