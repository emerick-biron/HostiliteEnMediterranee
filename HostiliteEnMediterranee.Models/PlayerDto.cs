namespace HostiliteEnMediterranee.Models;

public class PlayerDto
{
    public string Name { get; set; }
    public char[][] Grid { get; set; }

    public PlayerDto(string name, char[][] grid)
    {
        Name = name;
        Grid = grid;
    }
}