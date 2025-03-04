namespace Forja.Application.Services.Games;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
        
    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }
        
    public async Task<Game?> GetGameByIdAsync(Guid id)
    {
        return await _gameRepository.GetByIdAsync(id);
    }
        
    public async Task<IEnumerable<Game>> GetAllGamesAsync()
    {
        return await _gameRepository.GetAllAsync();
    }
        
    public async Task CreateGameAsync(Game game)
    {
        await _gameRepository.AddAsync(game);
    }
}