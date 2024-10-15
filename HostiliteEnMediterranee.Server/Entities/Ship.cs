namespace HostiliteEnMediterranee.Server.Entities;

public class Ship(char type, int size)
{
    public static readonly List<Ship> Ships =
    [
        new Ship('A', 5), // carrier
        new Ship('B', 4), // battleship
        new Ship('C', 3), // cruiser
        new Ship('D', 3), // submarine
        new Ship('E', 2), // destroyer
    ];

    public char Type { get; } = type;
    public int Size { get; } = size;
}