namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGameAddonRepository interface using Entity Framework Core.
/// </summary>
public class GameAddonRepository : IGameAddonRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<GameAddon> _gameAddons;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAddonRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GameAddonRepository(ForjaDbContext context)
    {
        _context = context;
        _gameAddons = context.Set<GameAddon>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameAddon>> GetAllAsync()
    {
        return await _gameAddons 
            .Where(ga => !ga.IsDeleted)
            .ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<GameAddon>> GetAllDeletedAsync()
    {
        return await _gameAddons 
            .Where(ga => ga.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameAddon?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game addon ID.", nameof(id));
        }
        
        return await _gameAddons
            .Where(ga => !ga.IsDeleted)
            .FirstOrDefaultAsync(ga => ga.Id == id);
    }

    /// <inheritdoc />
    public async Task<GameAddon?> GetByStorageUrlAsync(string storageUrl)
    {
        if (string.IsNullOrWhiteSpace(storageUrl))
        {
            throw new ArgumentException("Invalid storage URL.", nameof(storageUrl));
        }
        
        return await _gameAddons
            .Where(ga => !ga.IsDeleted)
            .FirstOrDefaultAsync(ga => ga.StorageUrl == storageUrl);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameAddon>> GetByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game ID.", nameof(gameId));
        }
        
        return await _gameAddons
            .Where(ga => ga.GameId == gameId)
            .Where(ga => !ga.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameAddon?> AddAsync(GameAddon gameAddon)
    {
        if (!GamesModelValidator.ValidateGameAddon(gameAddon, out _))
        {
            throw new ArgumentException("Invalid game addon.", nameof(gameAddon));
        }
        
        await _gameAddons.AddAsync(gameAddon);
        await _context.SaveChangesAsync();
        return gameAddon;
    }

    /// <inheritdoc />
    public async Task<GameAddon?> UpdateAsync(GameAddon gameAddon)
    {
        if (!GamesModelValidator.ValidateGameAddon(gameAddon, out _))
        {
            throw new ArgumentException("Invalid game addon.", nameof(gameAddon));
        }
        
        _gameAddons.Update(gameAddon);
        await _context.SaveChangesAsync();
        return gameAddon;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game addon ID.", nameof(id));
        }
        
        var gameAddon = await _gameAddons.FindAsync(id);
        if (gameAddon == null)
        {
            throw new ArgumentException("Game addon not found.", nameof(id));
        }
        
        gameAddon.IsDeleted = true;
        gameAddon.IsActive = false;
        gameAddon.ModifiedAt = DateTime.UtcNow;
        _gameAddons.Update(gameAddon);
        await _context.SaveChangesAsync();
    }
}