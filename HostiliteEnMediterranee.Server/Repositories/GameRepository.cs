using HostiliteEnMediterranee.Server.Entities;

namespace HostiliteEnMediterranee.Server.Repositories;

public class GameRepository
{
    private readonly Dictionary<Guid, Game> _games = new();

    public Game SaveGame(Game game)
    {
        _games[game.Id] = game;
        return game;
    }

    public Game? FindById(Guid gameId)
    {
        _games.TryGetValue(gameId, out var game);
        return game;
    }

    public void DeleteById(Guid gameId)
    {
        _games.Remove(gameId);
    }

    public List<Game> FindAll()
    {
        return _games.Values.ToList();
    }
}