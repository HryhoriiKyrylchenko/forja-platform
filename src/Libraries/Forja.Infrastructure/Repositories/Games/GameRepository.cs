namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGameRepository interface using Entity Framework Core.
/// </summary>
public class GameRepository : IGameRepository
{
    private readonly DbContext _context;
    private readonly DbSet<Game> _games;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GameRepository(DbContext context)
    {
        _context = context;
        _games = context.Set<Game>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await _games
            .Include(g => g.GameAddons)
            .Include(g => g.GameTags)
            .Include(g => g.UserLibraryGames)
            .Include(g => g.GameMechanics)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Game?> GetByIdAsync(Guid id)
    {
        return await _games
            .Include(g => g.GameAddons)
            .Include(g => g.GameTags)
            .Include(g => g.UserLibraryGames)
            .Include(g => g.GameMechanics)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <inheritdoc />
    public async Task<Game?> AddAsync(Game game)
    {
        await _games.AddAsync(game);
        await _context.SaveChangesAsync();
        return game;
    }

    /// <inheritdoc />
    public async Task<Game?> UpdateAsync(Game game)
    {
        _games.Update(game);
        await _context.SaveChangesAsync();
        return game;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var game = await _games.FindAsync(id);
        if (game != null)
        {
            _games.Remove(game);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Game>> GetByTagAsync(Guid tagId)
    {
        return await _games
            .Where(g => g.GameTags.Any(tag => tag.Id == tagId))
            .Include(g => g.GameAddons)
            .Include(g => g.GameTags)
            .Include(g => g.UserLibraryGames)
            .Include(g => g.GameMechanics)
            .ToListAsync();
    }
}