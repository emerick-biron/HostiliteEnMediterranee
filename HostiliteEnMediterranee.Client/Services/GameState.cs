using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Client.Services
{
    public class GameState
    {
        public char[,] PlayerGrid { get; private set; }
        public bool?[,] OpponentGrid { get; private set; }
        public int GridSize;

        public GameState(int gridSize = 10)
        {
            this.GridSize = gridSize;
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
            foreach (var ship in playerShips)
            {
                foreach (var coordinate in ship.Coordinates)
                {
                    if (IsWithinBounds(coordinate.X, coordinate.Y))
                    {
                        PlayerGrid[coordinate.X, coordinate.Y] = ship.Model;
                    }
                }
            }
        }

        public void UpdatePlayerGrid(int x, int y, char value)
        {
            if (IsWithinBounds(x, y))
            {
                PlayerGrid[x, y] = value;
            }
        }

        public void UpdateOpponentGrid(int x, int y, bool? value)
        {
            if (IsWithinBounds(x, y))
            {
                OpponentGrid[x, y] = value;
            }
        }

        private bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < GridSize && y >= 0 && y < GridSize;
        }
    }
}