namespace HostiliteEnMediterranee.Server.Entities;

public class Ship(char type, int size)
{
    public readonly List<Coordinates> Coordinates = [];

    public char Type { get; } = type;
    public int Size { get; } = size;
    public bool IsPlaced => Coordinates.Count != 0;

    public static List<Ship> GetDefaultShips() =>
    [
        new Ship('A', 5), // Carrier
        new Ship('B', 4), // Battleship
        new Ship('C', 3), // Cruiser
        new Ship('D', 3), // Submarine
        new Ship('E', 2)
    ];

    public void Place(List<Coordinates> coordinates)
    {
        Coordinates.Clear();
        Coordinates.AddRange(coordinates);
    }
}