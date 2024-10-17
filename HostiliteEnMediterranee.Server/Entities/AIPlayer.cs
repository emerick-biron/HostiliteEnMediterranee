namespace HostiliteEnMediterranee.Server.Entities;

// ReSharper disable once InconsistentNaming
public abstract class AIPlayer(string name) : Player(name)
{
    public enum Level
    {
        Dumb,
        Medium,
    }

    // ReSharper disable once InconsistentNaming
    internal static AIPlayer CreateAIPlayer(string name, Level aiLevel)
    {
        return aiLevel switch
        {
            Level.Dumb => new DumbAIPlayer(name),
            Level.Medium => new MediumAIPlayer(name),
            _ => throw new ArgumentOutOfRangeException(nameof(aiLevel), aiLevel, null)
        };
    }

    public abstract Coordinates GetNextShot();

    public abstract void UndoShot(Shot shot);
}