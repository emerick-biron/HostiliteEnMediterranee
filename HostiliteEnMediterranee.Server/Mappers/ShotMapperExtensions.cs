using HostiliteEnMediterranee.Models.Dto;
using HostiliteEnMediterranee.Server.Entities;

namespace HostiliteEnMediterranee.Server.Mappers;

public static class ShotMapperExtensions
{
    public static ShotDto ToDto(this Shot shot)
    {
        return new ShotDto(
            Shooter: shot.Shooter.Name,
            TargetPlayer: shot.TargetPlayer.Name,
            TargetCoordinates: new CoordinatesDto(shot.TargetCoordinates.Row, shot.TargetCoordinates.Column),
            HitShip: shot.Result.HitShip?.ToDto()
        );
    }
}