using HostiliteEnMediterranee.Client.Entities;
using HostiliteEnMediterranee.Models.Dto;
using HostiliteEnMediterranee.Models.Requests;

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
        public List<Ship> PlayerShips { get; private set; }
        public List<Ship> OpponentShips { get; private set; }
        public List<Move> MovesHistory { get; private set; }


        private readonly GameApiService _gameApiService;
        public GameState(GameApiService gameApiService)
        {
            _gameApiService = gameApiService;
        }

        public async Task StartNewGameAsync(AILevelDto level)
        {
            GridSize = 10;
            PlayerGrid = new char[GridSize, GridSize];
            OpponentGrid = new bool?[GridSize, GridSize];
            InitializeGrids();

            var response = await _gameApiService.StartGameAsync(level);
            Id = response.GameId;
            InitShips(response.PlayerShips);

            MovesHistory  = new List<Move>();
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

        private void InitShips(List<ShipDto> playerShips)
        {
            PlayerShips = new List<Ship>();
            OpponentShips = new List<Ship>();
            foreach (var ship in playerShips)
            {
                var newPlayerShip = new Ship(ship.Model, ship.Coordinates);
                PlayerShips.Add(newPlayerShip);

                var newOpponentShip = new Ship(ship.Model, ship.Coordinates.Count);
                OpponentShips.Add(newOpponentShip);
                foreach (var coordinate in newPlayerShip.Coordinates)
                {
                    if (IsWithinBounds(coordinate.Row, coordinate.Column))
                    {
                        PlayerGrid[coordinate.Row, coordinate.Column] = newPlayerShip.Model;
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
            MovesHistory.Add(new Move
            {
                Player = "Player",
                Row = row,
                Column = col,
                IsHit = sendShootResponse.HasHit,
                AdditionalInfo = sendShootResponse.OpponentShipSunk != null ? $" - Sunk {sendShootResponse.OpponentShipSunk.Model}" : ""
            });

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
                var ship = OpponentShips.Find(s => s.Model == sendShootResponse.OpponentShipSunk.Model);
                ship.UpdtateOpponentCoordinates(sendShootResponse.OpponentShipSunk.Coordinates);
            }
        }

        public async Task UndoLastMoveAsync()
        {
            if (MovesHistory.Count == 0)
            {
                return;
            }

            var awaitResponse = await _gameApiService.UndoLastPlayerTurnAsync(Id);
            
            foreach (var shot in awaitResponse.UndoneShots)
            {
                if (shot.Shooter == "Player")
                {
                    OpponentGrid[shot.TargetCoordinates.Row, shot.TargetCoordinates.Column] = null;
                    if (shot.HitShip != null)
                    {
                        OpponentShips.RemoveAll(ship => ship.Model == shot.HitShip.Model);
                    }
                }
                else
                {
                    if (shot.HitShip != null)
                    {
                        var ship = PlayerShips.Find(ship => ship.Model == shot.HitShip.Model);
                        var hitCoord = ship.HitCoordinates.Find(coord => coord.Row == shot.TargetCoordinates.Row && coord.Column == shot.TargetCoordinates.Column);
                        ship.HitCoordinates.Remove(hitCoord);
                        ship.IsSinked = false;
                        PlayerGrid[shot.TargetCoordinates.Row, shot.TargetCoordinates.Column] = ship.Model;
                    }
                    else
                    {
                        PlayerGrid[shot.TargetCoordinates.Row, shot.TargetCoordinates.Column] = '/';
                    }
                }
                MovesHistory.RemoveAt(MovesHistory.Count - 1);
            }
        }

        public void UpdatePlayerGrid(int row, int col)
        {
            if (IsWithinBounds(row, col))
            {
                Ship? ship = null;
                if (PlayerGrid[row, col] == '/')
                {
                    PlayerGrid[row, col] = 'O';
                }
                else
                {
                    char shipModel = PlayerGrid[row, col];
                    ship = PlayerShips.Find(s => s.Model == shipModel);
                    ship.HitCoordinates.Add(new CoordinatesDto(row, col));
                    if (ship.HitCoordinates.Count == ship.Size)
                    {
                        ship.IsSinked = true;
                    }
                    PlayerGrid[row, col] = 'X';
                }
                MovesHistory.Add(new Move
                {
                    Player = "Opponent",
                    Row = row,
                    Column = col,
                    IsHit = PlayerGrid[row, col] == 'X',
                    AdditionalInfo = ship != null && ship.IsSinked ? $" - Sunk {ship.Model}" : ""
                });
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
            foreach (var ship in OpponentShips)
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

        public List<Ship> GetUnsunkPlayerShips() => PlayerShips.Where(ship => !ship.IsSinked).ToList();
        public List<Ship> GetUnsunkOpponentShips() => OpponentShips.Where(ship => !ship.IsSinked).ToList();


    }
}
