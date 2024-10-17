using HostiliteEnMediterranee.Server.Exceptions;

namespace HostiliteEnMediterranee.Server.Entities;

// ReSharper disable once InconsistentNaming
public class DumbAIPlayer : AIPlayer
{
    private readonly List<Coordinates> _possibleShots;

    public DumbAIPlayer(string name) : base(name)
    {
        _possibleShots = [];
        for (var row = 0; row < GridSize; row++)
        for (var col = 0; col < GridSize; col++)
            _possibleShots.Add(new Coordinates(row, col));

        _possibleShots = _possibleShots.OrderBy(_ => Random.Shared.Next()).ToList();
    }

    public override Coordinates GetNextShot()
    {
        if (_possibleShots.Count == 0) throw new NoMorePossibleShotsException("No more possible shots");

        var nextShot = _possibleShots.First();
        _possibleShots.RemoveAt(0);
        return nextShot;
    }

    public override void UndoShot(Shot shot)
    {
        if (!_possibleShots.Contains(shot.TargetCoordinates))
        {
            _possibleShots.Add(shot.TargetCoordinates);
        }
    }
}