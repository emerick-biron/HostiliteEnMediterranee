using HostiliteEnMediterranee.Models.Dto;
using HostiliteEnMediterranee.Server.Entities;

namespace HostiliteEnMediterranee.Server.Mappers;

public static class ShipMapperExtensions
{
    public static ShipDto ToDto(this Ship ship)
    {
        var coordinatesDto = ship.Coordinates
            .Select(c => new CoordinatesDto(c.Row, c.Column))
            .ToList();

        return new ShipDto(ship.Type, coordinatesDto);
    }
}