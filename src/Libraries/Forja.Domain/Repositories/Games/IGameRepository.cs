namespace Forja.Domain.Repositories.Games;

public interface IGameRepository
{
    Task<Game?> GetByIdAsync(Guid id);
    Task<IEnumerable<Game>> GetAllAsync();
    Task AddAsync(Game game);
}
