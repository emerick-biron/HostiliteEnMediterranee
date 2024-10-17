namespace HostiliteEnMediterranee.Models.Dto;

public record ShotDto(string Shooter, string TargetPlayer, CoordinatesDto TargetCoordinates, ShipDto? HitShip);