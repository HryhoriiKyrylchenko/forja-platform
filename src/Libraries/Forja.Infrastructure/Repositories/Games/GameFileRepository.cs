namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGameFileRepository interface using Entity Framework Core.
/// </summary>
public class GameFileRepository : IGameFileRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<GameFile> _gameFiles;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameFileRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GameFileRepository(ForjaDbContext context)
    {
        _context = context;
        _gameFiles = context.Set<GameFile>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameFile>> GetAllAsync()
    {
        return await _gameFiles
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameFile?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game file ID.", nameof(id));
        }

        return await _gameFiles
            .FirstOrDefaultAsync(gf => gf.Id == id);
    }

    public async Task<GameFile?> GetGameFileByGameVersionIdAndFileName(Guid gameVersionId, string fileName)
    {
        if (gameVersionId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game version ID.", nameof(gameVersionId));
        }
        
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Invalid file name.", nameof(fileName));
        }
        
        return await _gameFiles
            .FirstOrDefaultAsync(gf => gf.GameVersionId == gameVersionId && gf.FileName == fileName);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameFile>> GetByGameVersionIdAsync(Guid gameVersionId)
    {
        if (gameVersionId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game version ID.", nameof(gameVersionId));
        }

        return await _gameFiles
            .Where(gf => gf.GameVersionId == gameVersionId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameFile?> AddAsync(GameFile gameFile)
    {
        if (!GamesModelValidator.ValidateGameFile(gameFile, out var errors))
        {
            throw new ArgumentException("Invalid game file.", nameof(gameFile));
        }

        await _gameFiles.AddAsync(gameFile);
        await _context.SaveChangesAsync();
        return gameFile;
    }

    /// <inheritdoc />
    public async Task<GameFile?> UpdateAsync(GameFile gameFile)
    {
        if (!GamesModelValidator.ValidateGameFile(gameFile, out var errors))
        {
            throw new ArgumentException("Invalid game file.", nameof(gameFile));
        }

        _gameFiles.Update(gameFile);
        await _context.SaveChangesAsync();
        return gameFile;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game file ID.", nameof(id));
        }

        var gameFile = await _gameFiles.FindAsync(id);
        if (gameFile == null)
        {
            throw new ArgumentException("Game file not found.", nameof(id));
        }

        _gameFiles.Remove(gameFile);
        await _context.SaveChangesAsync();
    }
}