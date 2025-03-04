namespace Forja.Application.Interfaces.Games;

public interface IGameService
{
    Task<Game?> GetGameByIdAsync(Guid id);
    Task<IEnumerable<Game>> GetAllGamesAsync();
    Task CreateGameAsync(Game game);
}