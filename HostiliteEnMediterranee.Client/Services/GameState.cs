using HostiliteEnMediterranee.Client.Entities;
using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Client.Services
{
    public class GameState
    {
        public char[,] PlayerGrid { get; private set; }
        public bool?[,] OpponentGrid { get; private set; }
        public int GridSize { get; private set; }
        public Guid GameId { get; set; }

        public GameStatusDto GameStatus { get; set; }
        public string Winner { get; set; }

        public List<Ship> ShipList { get; private set; }

        public bool UseGRPC { get; private set; } = true;
        public GameState(int gridSize = 10)
        {
            this.GridSize = gridSize;
            this.Winner = "";
            PlayerGrid = new char[gridSize, gridSize];
            OpponentGrid = new bool?[gridSize, gridSize];
            InitializeGrids();
        }

        private void InitializeGrids()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    PlayerGrid[i, j] = '/';
                    OpponentGrid[i, j] = null;
                }
            }
        }

        public void UpdatePlayerShips(List<ShipDto> playerShips)
        {
            ShipList = new List<Ship>();
            foreach (var ship in playerShips)
            {
                ShipList.Add(new Ship(ship.Model, ship.Coordinates));
            }
            foreach (var ship in playerShips)
            {
                foreach (var coordinate in ship.Coordinates)
                {
                    if (IsWithinBounds(coordinate.Row, coordinate.Column))
                    {
                        PlayerGrid[coordinate.Row, coordinate.Column] = ship.Model;
                    }
                }
            }
            ConsoleLogPlayerGrid();
        }

        public void UpdatePlayerGrid(int row, int col)
        {
            if (IsWithinBounds(row, col))
            {
                if (PlayerGrid[row, col] == '/')
                {
                    PlayerGrid[row, col] = 'O';
                    return;
                }
                char shipModel = PlayerGrid[row, col];
                Ship ship = ShipList.Find(s => s.Model == shipModel);
                ship.HitCoordinates.Add(new CoordinatesDto(row, col));
                if (ship.HitCoordinates.Count == ship.Size)
                {
                    ship.IsSinked = true;
                }
                PlayerGrid[row, col] = 'X';
            }
        }

        public void UpdateOpponentGrid(int row, int col, bool? value)
        {
            if (IsWithinBounds(row, col))
            {
                OpponentGrid[row, col] = value;
            }
        }

        private bool IsWithinBounds(int row, int col)
        {
            return row >= 0 && row < GridSize && col >= 0 && col < GridSize;
        }

        public void ConsoleLogPlayerGrid()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Console.Write(PlayerGrid[row, col] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}