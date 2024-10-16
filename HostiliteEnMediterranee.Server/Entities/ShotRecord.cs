namespace HostiliteEnMediterranee.Server.Entities;

public record ShotRecord(Player Shooter, Coordinates TargetCoordinates, ShotResult Result);