namespace HostiliteEnMediterranee.Models.Dto;

public class CoordinatesDto(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
}