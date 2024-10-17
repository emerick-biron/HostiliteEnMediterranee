namespace HostiliteEnMediterranee.Server.Entities;

// ReSharper disable once InconsistentNaming
public class MediumAIPlayer(string name) : AIPlayer(name)
{
    private readonly List<Shot> _shotHistory = [];


    public override Coordinates GetNextShot()
    {
        throw new NotImplementedException();
    }

    public override ShotResult Shoot(int row, int col, Player targetPlayer)
    {
        var result = base.Shoot(row, col, targetPlayer);
        _shotHistory.Add(new Shot(this, targetPlayer, new Coordinates(row, col), result));
        return result;
    }

    public override void UndoShot(Shot shot)
    {
        _shotHistory.Remove(shot);
    }
}