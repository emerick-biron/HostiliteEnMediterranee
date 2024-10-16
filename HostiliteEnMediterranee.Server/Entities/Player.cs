using HostiliteEnMediterranee.Server.Exceptions;

namespace HostiliteEnMediterranee.Server.Entities;

public class Player
{
    public const int GridSize = 10;
    public readonly List<Ship> Ships = [];

    public Player(string name)
    {
        Name = name;
        Grid = new char[GridSize, GridSize];
        for (var row = 0; row < GridSize; row++)
        {
            for (var col = 0; col < GridSize; col++)
            {
                Grid[row, col] = '\0';
            }
        }
    }

    public string Name { get; private set; }
    public char[,] Grid { get; }

    public void GenerateRandomGrid(List<Ship> ships)
    {
        foreach (var ship in ships)
        {
            var placed = false;

            do
            {
                var direction = (Direction)Random.Shared.Next(2);
                var row = Random.Shared.Next(GridSize);
                var col = Random.Shared.Next(GridSize);

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
            if (col + size > GridSize)
            {
                return false;
            }

            for (var i = 0; i < size; i++)
            {
                if (Grid[row, col + i] != '\0')
                {
                    return false;
                }
            }
        }
        else
        {
            if (row + size > GridSize)
            {
                return false;
            }

            for (var i = 0; i < size; i++)
            {
                if (Grid[row + i, col] != '\0')
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void PlaceShip(int row, int col, Ship ship, Direction direction)
    {
        List<Coordinates> shipCoordinates = [];
        if (direction == Direction.Horizontal)
        {
            for (var i = 0; i < ship.Size; i++)
            {
                Grid[row, col + i] = ship.Type;
                shipCoordinates.Add(new Coordinates(row, col + i));
            }
        }
        else
        {
            for (var i = 0; i < ship.Size; i++)
            {
                Grid[row + i, col] = ship.Type;
                shipCoordinates.Add(new Coordinates(row + i, col));
            }
        }

        ship.Place(shipCoordinates);
        Ships.Add(ship);
    }

    public bool HasLost()
    {
        for (var row = 0; row < GridSize; row++)
        {
            for (var col = 0; col < GridSize; col++)
            {
                var cell = Grid[row, col];
                if (cell != '\0' && cell != 'O' && cell != 'X')
                {
                    return false;
                }
            }
        }

        return true;
    }

    private Ship GetShip(char type)
    {
        var ship = Ships.FirstOrDefault(s => s.Type == type);
        if (ship == null)
        {
            throw new InvalidOperationException($"No ship found for type {type}");
        }

        return ship;
    }

    public ShotResult ReceiveShot(int row, int col)
    {
        switch (Grid[row, col])
        {
            case 'X':
            case 'O':
                throw new CellAlreadyShotException($"Cell[{row}, {col}] has already been shot");
            case '\0':
                Grid[row, col] = 'O';
                return new ShotResult(null, false);
            default:
                var shipType = Grid[row, col];
                Grid[row, col] = 'X';
                var ship = GetShip(shipType);
                var isSunk = ship.Coordinates.All(coord => Grid[coord.Row, coord.Column] == 'X');
                return new ShotResult(ship, isSunk);
        }
    }
}