namespace Forja.Infrastructure.Repositories.Games;

public class GamePatchRepository : IGamePatchRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<GamePatch> _gamePatches;

    /// <summary>
    /// Initializes a new instance of the <see cref="GamePatchRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GamePatchRepository(ForjaDbContext context)
    {
        _context = context;
        _gamePatches = context.Set<GamePatch>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GamePatch>> GetAllAsync()
    {
        return await _gamePatches.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GamePatch?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game patch ID.", nameof(id));
        }

        return await _gamePatches.FirstOrDefaultAsync(gp => gp.Id == id);
    }

    /// <inheritdoc />
    public async Task<GamePatch?> GetByGameIdPlatformAndPatchNameAsync(Guid gameId, PlatformType platform, string patchName)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game ID.", nameof(gameId));
        }
        
        if (string.IsNullOrWhiteSpace(patchName))
        {
            throw new ArgumentException("Invalid patch name.", nameof(patchName));
        }
        
        return await _gamePatches.FirstOrDefaultAsync(gp => gp.GameId == gameId 
                                                            && gp.Name == patchName
                                                            && gp.Platform == platform);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GamePatch>> GetByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game ID.", nameof(gameId));
        }

        return await _gamePatches
            .Where(gp => gp.GameId == gameId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GamePatch?> AddAsync(GamePatch gamePatch)
    {
        // Validation logic can be added here (similar to GameFile validation)
        await _gamePatches.AddAsync(gamePatch);
        await _context.SaveChangesAsync();
        return gamePatch;
    }

    /// <inheritdoc />
    public async Task<GamePatch?> UpdateAsync(GamePatch gamePatch)
    {
        // Validation logic can be added here (similar to GameFile validation)
        _gamePatches.Update(gamePatch);
        await _context.SaveChangesAsync();
        return gamePatch;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game patch ID.", nameof(id));
        }

        var gamePatch = await _gamePatches.FindAsync(id);
        if (gamePatch == null)
        {
            throw new ArgumentException("Game patch not found.", nameof(id));
        }

        _gamePatches.Remove(gamePatch);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<GamePatch?> GetByGameIdPlatformAndVersionsAsync(Guid gameId, PlatformType platform, string fromVersion, string toVersion)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game ID.", nameof(gameId));
        }

        if (string.IsNullOrWhiteSpace(fromVersion))
        {
            throw new ArgumentException("Invalid from version.", nameof(fromVersion));
        }

        if (string.IsNullOrWhiteSpace(toVersion))
        {
            throw new ArgumentException("Invalid to version.", nameof(toVersion));
        }

        return await _gamePatches.FirstOrDefaultAsync(gp => gp.GameId == gameId 
                                                            && gp.FromVersion == fromVersion 
                                                            && gp.ToVersion == toVersion
                                                            && gp.Platform == platform);
    }
}