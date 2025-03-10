namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGameAddonRepository interface using Entity Framework Core.
/// </summary>
public class GameAddonRepository : IGameAddonRepository
{
    private readonly DbContext _context;
    private readonly DbSet<GameAddon> _gameAddons;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAddonRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GameAddonRepository(DbContext context)
    {
        _context = context;
        _gameAddons = context.Set<GameAddon>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameAddon>> GetAllAsync()
    {
        return await _gameAddons
            .Include(ga => ga.Game) // Include the associated game
            .Include(ga => ga.UserLibraryAddons) // Include user library addons
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameAddon?> GetByIdAsync(Guid id)
    {
        return await _gameAddons
            .Include(ga => ga.Game) // Include the associated game
            .Include(ga => ga.UserLibraryAddons) // Include user library addons
            .FirstOrDefaultAsync(ga => ga.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameAddon>> GetByGameIdAsync(Guid gameId)
    {
        return await _gameAddons
            .Where(ga => ga.GameId == gameId)
            .Include(ga => ga.UserLibraryAddons) // Include user library addons
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameAddon?> AddAsync(GameAddon gameAddon)
    {
        await _gameAddons.AddAsync(gameAddon);
        await _context.SaveChangesAsync();
        return gameAddon;
    }

    /// <inheritdoc />
    public async Task<GameAddon?> UpdateAsync(GameAddon gameAddon)
    {
        _gameAddons.Update(gameAddon);
        await _context.SaveChangesAsync();
        return gameAddon;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var gameAddon = await _gameAddons.FindAsync(id);
        if (gameAddon != null)
        {
            _gameAddons.Remove(gameAddon);
            await _context.SaveChangesAsync();
        }
    }
}