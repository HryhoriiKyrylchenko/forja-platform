namespace Forja.Infrastructure.Repositories.UserProfile;

/// <summary>
/// Repository class for managing GameSave entities in the data store.
/// </summary>
public class GameSaveRepository : IGameSaveRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<GameSave> _gameSaves;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameSaveRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used to interact with the data store.</param>
    public GameSaveRepository(ForjaDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _gameSaves = _context.Set<GameSave>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameSave>> GetAllAsync()
    {
        return await _gameSaves.Include(gs => gs.User)
                           .Include(gs => gs.UserLibraryGame)
                           .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameSave?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("GameSave id cannot be empty.", nameof(id));
        }
        return await _gameSaves.Include(gs => gs.User)
                           .Include(gs => gs.UserLibraryGame)
                           .FirstOrDefaultAsync(gs => gs.Id == id);
    }

    /// <inheritdoc />
    public async Task<List<GameSave>> GetAllByFilterAsync(Guid? libraryGameId, Guid? userId)
    {
        return await _gameSaves.Include(gs => gs.User)
                           .Include(gs => gs.UserLibraryGame)
                           .Where(gs => (!libraryGameId.HasValue || gs.UserLibraryGameId == libraryGameId) &&
                                        (!userId.HasValue || gs.UserId == userId))
                           .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameSave?> AddAsync(GameSave gameSave)
    {
        if (!UserProfileModelValidator.ValidateGameSave(gameSave))
        {
            throw new ArgumentException("GameSave is invalid.", nameof(gameSave));
        }

        await _gameSaves.AddAsync(gameSave);
        await _context.SaveChangesAsync();
        return gameSave;
    }

    /// <inheritdoc />
    public async Task<GameSave?> UpdateAsync(GameSave gameSave)
    {
        if (!UserProfileModelValidator.ValidateGameSave(gameSave))
        {
            throw new ArgumentException("GameSave is invalid.", nameof(gameSave));
        }

        _gameSaves.Update(gameSave);
        await _context.SaveChangesAsync();
        return gameSave;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("GameSave id cannot be empty.", nameof(id));
        }
        var gameSave = await GetByIdAsync(id);
        if (gameSave == null)
        {
            throw new InvalidOperationException($"GameSave with id {id} not found.");
        }

        _gameSaves.Remove(gameSave);
        await _context.SaveChangesAsync();
    }
}