namespace HostiliteEnMediterranee.Models.Dto;

public class ShipDto(char model, List<CoordinatesDto> coordinates)
{
    public char Model { get; set; } = model;
    public List<CoordinatesDto> Coordinates { get; set; } = coordinates;
}