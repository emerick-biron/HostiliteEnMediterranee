namespace HostiliteEnMediterranee.Server.Entities;

public class Position(int x, int y, bool isHit = false)
{
    public int X { get; private set;} = x;
    public int Y { get; private set;} = y;
    public bool IsHit { get; set; } = isHit;
}