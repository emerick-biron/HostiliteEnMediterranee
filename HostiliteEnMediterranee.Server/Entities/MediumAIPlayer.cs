namespace HostiliteEnMediterranee.Server.Entities;

// ReSharper disable once InconsistentNaming
public class MediumAIPlayer(string name) : AIPlayer(name)
{
    private readonly List<Shot> _shotHistory = new();
    private Direction? _currentDirection;
    private bool _isChasingShip;
    private Coordinates? _lastHit;
    private bool _triedReverseDirection;

    public override Coordinates GetNextShot()
    {
        if (_isChasingShip && _lastHit != null && _currentDirection != null)
        {
            var nextTarget = GetNextInDirection(_lastHit, _currentDirection.Value, _triedReverseDirection);

            if (IsValidTarget(nextTarget))
                return nextTarget;

            if (!_triedReverseDirection)
            {
                _triedReverseDirection = true;
                nextTarget = GetNextInDirection(_lastHit, _currentDirection.Value, _triedReverseDirection);

                if (IsValidTarget(nextTarget))
                    return nextTarget;
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
            var availableTargets = GetAdjacentCoordinates(lastHit)
                .Where(IsValidTarget)
                .ToList();

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
            _triedReverseDirection = !_triedReverseDirection || (_currentDirection = null) != null;
            _isChasingShip = _currentDirection != null;
        }

        return result;
    }

    public override void UndoShot(Shot shot)
    {
        _shotHistory.Remove(shot);
    }

    private bool IsValidTarget(Coordinates coordinates)
    {
        return coordinates.Row >= 0 && coordinates.Row < GridSize &&
               coordinates.Column >= 0 && coordinates.Column < GridSize &&
               _shotHistory.All(s => s.TargetCoordinates != coordinates);
    }

    private Coordinates GetRandomTarget()
    {
        return Enumerable.Range(0, GridSize)
            .SelectMany(row => Enumerable.Range(0, GridSize).Select(col => new Coordinates(row, col)))
            .Where(IsValidTarget)
            .OrderBy(_ => Random.Shared.Next())
            .First();
    }

    private Coordinates GetNextInDirection(Coordinates lastHit, Direction direction, bool reverse)
    {
        return direction switch
        {
            Direction.Horizontal => reverse
                ? new Coordinates(lastHit.Row, lastHit.Column + 1)
                : new Coordinates(lastHit.Row, lastHit.Column - 1),
            _ => reverse
                ? new Coordinates(lastHit.Row + 1, lastHit.Column)
                : new Coordinates(lastHit.Row - 1, lastHit.Column)
        };
    }

    private Direction GuessDirection()
    {
        return (Direction)Random.Shared.Next(2);
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
}