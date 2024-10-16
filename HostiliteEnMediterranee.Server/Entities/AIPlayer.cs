using HostiliteEnMediterranee.Server.Exceptions;

namespace HostiliteEnMediterranee.Server.Entities;

// ReSharper disable once InconsistentNaming
public class AIPlayer : Player
{
    private readonly List<Coordinates> _possibleShots;

    public AIPlayer(string name) : base(name)
    {
        _possibleShots = [];
        for (var row = 0; row < GridSize; row++)
        for (var col = 0; col < GridSize; col++)
            _possibleShots.Add(new Coordinates(row, col));

        _possibleShots = _possibleShots.OrderBy(_ => Random.Shared.Next()).ToList();
    }

    public Coordinates GetNextShot()
    {
        if (_possibleShots.Count == 0) throw new NoMorePossibleShotsException("No more possible shots");

        var nextShot = _possibleShots.First();
        _possibleShots.RemoveAt(0);
        return nextShot;
    }
}