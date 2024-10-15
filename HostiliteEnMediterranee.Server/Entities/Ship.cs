namespace HostiliteEnMediterranee.Server.Entities;

public class Ship(char type, int size)
{
    public static readonly List<Ship> Ships =
    [
        new('A', 5), // carrier
        new('B', 4), // battleship
        new('C', 3), // cruiser
        new('D', 3), // submarine
        new('E', 2) // destroyer
    ];

    public char Type { get; } = type;
    public int Size { get; } = size;
}