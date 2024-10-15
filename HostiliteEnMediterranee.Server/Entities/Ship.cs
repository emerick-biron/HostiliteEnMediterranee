namespace HostiliteEnMediterranee.Server.Entities;

public class Ship(char model, int size)
{
    public char Model { get; private set; } = model;
    public int Size { get; private set; } = size;
    public bool IsDestroyed { get; set; } = false;
    public List<Position> Positions { get; private set; } = [];
    public Direction Direction { get; private set; }

    public void Place(Position startPosition, Direction direction)
    {
        Direction = direction;
        
        startPosition.IsHit = false;
        Positions.Add(startPosition);
        
        for (var i = 1; i < Size; i++)
        {
            var position = Direction == Direction.Horizontal
                ? new Position(startPosition.X + i, startPosition.Y)
                : new Position(startPosition.X, startPosition.Y + i);
            Positions.Add(position);
        }
    }
}