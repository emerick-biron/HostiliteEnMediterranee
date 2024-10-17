namespace HostiliteEnMediterranee.Server.Entities;

// ReSharper disable once InconsistentNaming
public abstract class AIPlayer(string name) : Player(name)
{
    public abstract Coordinates GetNextShot();

    public abstract void UndoShot(Shot shot);
}