using HostiliteEnMediterranee.Client.Entities;
using HostiliteEnMediterranee.Models.Dto;
using HostiliteEnMediterranee.Models.Requests;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace HostiliteEnMediterranee.Client.Services
{
    public class GameState
    {
        public Guid Id { get; private set; }
        public int GridSize { get; private set; }
        public int Difficulty { get; private set; }
        public char[,] PlayerGrid { get; private set; }
        public bool?[,] OpponentGrid { get; private set; }
        public GameStatusDto GameStatus { get; set; }
        public bool PlayerWin { get; set; }
        public List<Ship> PlayerShips { get; private set; } = new List<Ship>();
        public List<Ship> OpponentSunkShips { get; private set; } = new List<Ship>();

        private readonly GameApiService _gameApiService;
        public GameState(GameApiService gameApiService)
        {
            _gameApiService = gameApiService;
        }

        public async Task StartNewGameAsync(int gridSize = 10, int difficulty = 1, List<Ship> playerships = null)
        {
            GridSize = gridSize;
            Difficulty = difficulty;
            PlayerGrid = new char[gridSize, gridSize];
            OpponentGrid = new bool?[gridSize, gridSize];
            InitializeGrids();

            var response = await _gameApiService.StartGameAsync();
            Id = response.GameId;
            InitPlayerShips(response.PlayerShips);
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

        private void InitPlayerShips(List<ShipDto> playerShips)
        {
            foreach (var ship in playerShips)
            {
                var newShip = new Ship(ship.Model, ship.Coordinates);
                PlayerShips.Add(newShip);
                foreach (var coordinate in newShip.Coordinates)
                {
                    if (IsWithinBounds(coordinate.Row, coordinate.Column))
                    {
                        PlayerGrid[coordinate.Row, coordinate.Column] = newShip.Model;
                    }
                }
            }
        }

        public async Task SendShootAsync(int row, int col)
        {
            if (OpponentGrid[row, col] != null)
            {
                return;
            }
            var sendShootResponse = await _gameApiService.SendShootAsync(new ShootingRequest(new CoordinatesDto(row, col)), Id);
            GameStatus = sendShootResponse.GameStatus;
            UpdateOpponentGrid(row, col, sendShootResponse.HasHit);
            foreach (CoordinatesDto opponentShoot in sendShootResponse.OpponentShoots)
            {
                UpdatePlayerGrid(opponentShoot.Row, opponentShoot.Column);
            }
            if (sendShootResponse.GameStatus == GameStatusDto.Over)
            {
                PlayerWin = sendShootResponse.WinnerName == "Player";
            }
            if (sendShootResponse.OpponentShipSunk != null)
            {
                OpponentSunkShips.Add(new Ship(sendShootResponse.OpponentShipSunk.Model, sendShootResponse.OpponentShipSunk.Coordinates));
            }
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
                Ship ship = PlayerShips.Find(s => s.Model == shipModel);
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

        public bool CellIsSunk(int row, int col)
        {
            foreach (var ship in OpponentSunkShips)
            {
                foreach (var coord in ship.Coordinates)
                {
                    if (coord.Row == row && coord.Column == col) return true;
                }
            }
            return false;
        }

        private bool IsWithinBounds(int row, int col)
        {
            return row >= 0 && row < GridSize && col >= 0 && col < GridSize;
        }

    }
}
