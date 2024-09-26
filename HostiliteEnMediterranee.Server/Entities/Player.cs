namespace HostiliteEnMediterranee.Server.Entities;

public class Player(string name)
{
    public string Name { get; private set; } = name;
    public Grid Grid { get; private set; } = new();
}