namespace HostiliteEnMediterranee.Models;

public class PlayerDto(string name, char[][] grid)
{
    public string Name { get; set; } = name;
    public char[][] Grid { get; set; } = grid;
}