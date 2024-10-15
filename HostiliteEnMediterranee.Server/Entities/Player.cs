namespace HostiliteEnMediterranee.Server.Entities;

public class Player
{
    public const int GridSize = 10;

    public Player(string name)
    {
        Name = name;
        Grid = new char[GridSize][];
        for (var i = 0; i < GridSize; i++)
        {
            Grid[i] = new char[GridSize];
            for (var j = 0; j < GridSize; j++)
            {
                Grid[i][j] = '\0';
            }
        }
    }

    public string Name { get; private set; }
    public char[][] Grid { get; }

    public void GenerateRandomGrid(List<Ship> ships)
    {
        foreach (var ship in ships)
        {
            var placed = false;

            do
            {
                var direction = (Direction)Random.Shared.Next(2);
                var row = Random.Shared.Next(10);
                var col = Random.Shared.Next(10);

                if (CanPlaceShip(row, col, ship.Size, direction))
                {
                    PlaceShip(row, col, ship, direction);
                    placed = true;
                }
            } while (!placed);
        }
    }

    private bool CanPlaceShip(int row, int col, int size, Direction direction)
    {
        if (direction == Direction.Horizontal)
        {
            if (col + size > GridSize) return false;

            for (var i = 0; i < size; i++)
            {
                if (Grid[row][col + i] != '\0') return false;
            }
        }
        else
        {
            if (row + size > GridSize) return false;

            for (var i = 0; i < size; i++)
            {
                if (Grid[row + i][col] != '\0') return false;
            }
        }

        return true;
    }

    private void PlaceShip(int row, int col, Ship ship, Direction direction)
    {
        if (direction == Direction.Horizontal)
        {
            for (var i = 0; i < ship.Size; i++)
            {
                Grid[row][col + i] = ship.Type;
            }
        }
        else
        {
            for (var i = 0; i < ship.Size; i++)
            {
                Grid[row + i][col] = ship.Type;
            }
        }
    }
}