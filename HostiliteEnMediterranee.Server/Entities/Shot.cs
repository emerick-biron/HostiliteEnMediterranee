namespace HostiliteEnMediterranee.Server.Entities;

public record Shot(Player Shooter, Player TargetPlayer, Coordinates TargetCoordinates, ShotResult Result);