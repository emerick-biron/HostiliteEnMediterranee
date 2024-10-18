namespace HostiliteEnMediterranee.Server.Entities;

// ReSharper disable once InconsistentNaming
public class MediumAIPlayer(string name) : AIPlayer(name)
{
    private readonly List<Shot> _shotHistory = [];
    private Direction? _currentDirection;
    private bool _isChasingShip;
    private Coordinates? _lastHit;
    private bool _triedReverseDirection;

    public override Coordinates GetNextShot()
    {
        if (_lastHit != null && _currentDirection != null && _isChasingShip)
        {
            var nextTarget = GetNextInDirection(_lastHit, _currentDirection.Value, _triedReverseDirection);
            if (IsValidTarget(nextTarget))
            {
                return nextTarget;
            }

            if (!_triedReverseDirection)
            {
                _triedReverseDirection = true;
                return GetNextInDirection(_lastHit, _currentDirection.Value, _triedReverseDirection);
            }

            _currentDirection = null;
            _triedReverseDirection = false;
        }

        var lastHit = _shotHistory
            .Where(s => s.Result.HitShip != null && !s.Result.HasSunk)
            .Select(s => s.TargetCoordinates)
            .LastOrDefault();

        if (lastHit != null)
        {
            var possibleTargets = GetAdjacentCoordinates(lastHit);
            var availableTargets = possibleTargets.Where(IsValidTarget).ToList();

            if (availableTargets.Any())
            {
                _lastHit = lastHit;
                _currentDirection = GuessDirection();
                _isChasingShip = true;
                _triedReverseDirection = false;
                return availableTargets.OrderBy(_ => Random.Shared.Next()).First();
            }
        }

        _isChasingShip = false;
        return GetRandomTarget();
    }

    private IEnumerable<Coordinates> GetAdjacentCoordinates(Coordinates lastHit)
    {
        var possibleCoordinates = new List<Coordinates>
        {
            new Coordinates(lastHit.Row - 1, lastHit.Column),
            new Coordinates(lastHit.Row + 1, lastHit.Column),
            new Coordinates(lastHit.Row, lastHit.Column - 1),
            new Coordinates(lastHit.Row, lastHit.Column + 1)
        };

        return possibleCoordinates;
    }

    public override ShotResult Shoot(int row, int col, Player targetPlayer)
    {
        var result = base.Shoot(row, col, targetPlayer);
        _shotHistory.Add(new Shot(this, targetPlayer, new Coordinates(row, col), result));

        if (result.HitShip != null)
        {
            _lastHit = new Coordinates(row, col);

            if (_currentDirection == null)
            {
                _currentDirection = GuessDirection();
                _triedReverseDirection = false;
            }
        }
        else
        {
            if (!_triedReverseDirection)
            {
                _triedReverseDirection = true;
            }
            else
            {
                _currentDirection = null;
                _isChasingShip = false;
            }
        }

        return result;
    }

    public override void UndoShot(Shot shot)
    {
        _shotHistory.Remove(shot);
    }

    private bool IsValidTarget(Coordinates coordinates)
    {
        if (coordinates.Row < 0 || coordinates.Row >= GridSize || coordinates.Column < 0 ||
            coordinates.Column >= GridSize)
            return false;

        return _shotHistory.All(s => s.TargetCoordinates != coordinates);
    }

    private Coordinates GetRandomTarget()
    {
        var allPossibleTargets = new List<Coordinates>();
        for (var row = 0; row < GridSize; row++)
        {
            for (var col = 0; col < GridSize; col++)
            {
                var coordinates = new Coordinates(row, col);
                if (IsValidTarget(coordinates))
                {
                    allPossibleTargets.Add(coordinates);
                }
            }
        }

        return allPossibleTargets.OrderBy(_ => Random.Shared.Next()).First();
    }

    private Coordinates GetNextInDirection(Coordinates lastHit, Direction direction, bool reverse)
    {
        if (direction == Direction.Horizontal)
        {
            var left = new Coordinates(lastHit.Row, lastHit.Column - 1);
            var right = new Coordinates(lastHit.Row, lastHit.Column + 1);

            return reverse ? right : left;
        }

        var up = new Coordinates(lastHit.Row - 1, lastHit.Column);
        var down = new Coordinates(lastHit.Row + 1, lastHit.Column);

        return reverse ? down : up;
    }

    private Direction GuessDirection()
    {
        return (Direction)Random.Shared.Next(2);
    }
}