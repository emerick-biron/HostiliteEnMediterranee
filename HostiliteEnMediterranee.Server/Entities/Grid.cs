namespace HostiliteEnMediterranee.Server.Entities;

public class Grid
{
    public char[][] Cells { get; private set; }
    public List<Ship> Ships { get; private set; }

    public Grid(int size = 10)
    {
        // Init cells
        Cells = new char[size][];
        for (var i = 0; i < size; i++)
        {
            Cells[i] = new char[size];
            for (var j = 0; j < size; j++)
            {
                Cells[i][j] = '\0';
            }
        }

        Ships =
        [
            new Ship('A', 5), // carrier
            new Ship('B', 4), // battleship
            new Ship('C', 3), // cruiser
            new Ship('D', 3), // submarine
            new Ship('E', 2), // destroyer
        ];
    }

    public void PlaceShip(Ship ship, Position startPosition, Direction direction)
    {
        if (!Ships.Contains(ship))
        {
            throw new ArgumentException("Ship is not in this grid");
        }

        if (startPosition.X < 0 || startPosition.Y < 0)
        {
            throw new ArgumentException("Starting position is out of grid bounds on the negative side");
        }

        if (direction == Direction.Horizontal && startPosition.X + ship.Size > Cells.Length)
        {
            throw new ArgumentException("Ship placement exceeds the grid width");
        }

        if (direction == Direction.Vertical && startPosition.Y + ship.Size > Cells[0].Length)
        {
            throw new ArgumentException("Ship placement exceeds the grid height");
        }

        for (var i = 0; i < ship.Size; i++)
        {
            var x = direction == Direction.Horizontal ? startPosition.X + i : startPosition.X;
            var y = direction == Direction.Vertical ? startPosition.Y + i : startPosition.Y;

            if (Cells[y][x] != '\0')
            {
                throw new ArgumentException($"Ship already present in x={x}, y={y}");
            }
        }

        for (var i = 0; i < ship.Size; i++)
        {
            var x = direction == Direction.Horizontal ? startPosition.X + i : startPosition.X;
            var y = direction == Direction.Vertical ? startPosition.Y + i : startPosition.Y;
            
            Cells[y][x] = ship.Model;
        }
        
        ship.Place(startPosition, direction);
    }
}