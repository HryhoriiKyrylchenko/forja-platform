namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGameVersionRepository interface using Entity Framework Core.
/// </summary>
public class GameVersionRepository : IGameVersionRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<GameVersion> _gameVersions;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameVersionRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GameVersionRepository(ForjaDbContext context)
    {
        _context = context;
        _gameVersions = context.Set<GameVersion>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameVersion>> GetAllAsync()
    {
        return await _gameVersions
            .Include(gv => gv.Files)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameVersion?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game version ID.", nameof(id));
        }

        return await _gameVersions
            .Include(gv => gv.Files)
            .FirstOrDefaultAsync(gv => gv.Id == id);
    }

    public async Task<GameVersion?> GetByGameIdAndVersionAsync(Guid gameId, string version)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game ID.", nameof(gameId));
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new ArgumentException("Invalid version.", nameof(version));
        }
        
        return await _gameVersions
            .Include(gv => gv.Files)
            .FirstOrDefaultAsync(gv => gv.GameId == gameId && gv.Version == version);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameVersion>> GetByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game ID.", nameof(gameId));
        }

        return await _gameVersions
            .Where(gv => gv.GameId == gameId)
            .Include(gv => gv.Files)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameVersion?> AddAsync(GameVersion gameVersion)
    {
        if (!GamesModelValidator.ValidateGameVersion(gameVersion, out var errors))
        {
            throw new ArgumentException("Invalid game version.", nameof(gameVersion));
        }

        await _gameVersions.AddAsync(gameVersion);
        await _context.SaveChangesAsync();
        return await _gameVersions
            .Include(gv => gv.Files)
            .FirstOrDefaultAsync(gv => gv.GameId == gameVersion.GameId);
    }

    /// <inheritdoc />
    public async Task<GameVersion?> UpdateAsync(GameVersion gameVersion)
    {
        if (!GamesModelValidator.ValidateGameVersion(gameVersion, out var errors))
        {
            throw new ArgumentException("Invalid game version.", nameof(gameVersion));
        }

        _gameVersions.Update(gameVersion);
        await _context.SaveChangesAsync();
        return await _gameVersions
            .Include(gv => gv.Files)
            .FirstOrDefaultAsync(gv => gv.GameId == gameVersion.GameId);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game version ID.", nameof(id));
        }

        var gameVersion = await _gameVersions.FindAsync(id);
        if (gameVersion == null)
        {
            throw new ArgumentException("Game version not found.", nameof(id));
        }

        _gameVersions.Remove(gameVersion);
        await _context.SaveChangesAsync();
    }
}