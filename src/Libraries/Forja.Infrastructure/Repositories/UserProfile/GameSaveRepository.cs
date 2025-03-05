namespace Forja.Infrastructure.Repositories.UserProfile;

/// <summary>
/// Repository class for managing GameSave entities in the data store.
/// </summary>
public class GameSaveRepository : IGameSaveRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<GameSave> _dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameSaveRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used to interact with the data store.</param>
    public GameSaveRepository(ForjaDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<GameSave>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameSave>> GetAllAsync()
    {
        return await _dbSet.Include(gs => gs.User)
                           .Include(gs => gs.UserLibraryGame)
                           .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameSave?> GetByIdAsync(Guid id)
    {
        return await _dbSet.Include(gs => gs.User)
                           .Include(gs => gs.UserLibraryGame)
                           .FirstOrDefaultAsync(gs => gs.Id == id);
    }

    /// <inheritdoc />
    public async Task<GameSave> AddAsync(GameSave gameSave)
    {
        if (gameSave == null)
        {
            throw new ArgumentNullException(nameof(gameSave));
        }

        var result = await _dbSet.AddAsync(gameSave);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    /// <inheritdoc />
    public async Task<GameSave> UpdateAsync(GameSave gameSave)
    {
        if (gameSave == null)
        {
            throw new ArgumentNullException(nameof(gameSave));
        }

        _dbSet.Update(gameSave);
        await _context.SaveChangesAsync();
        return gameSave;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var gameSave = await GetByIdAsync(id);
        if (gameSave == null)
        {
            throw new InvalidOperationException($"GameSave with id {id} not found.");
        }

        _dbSet.Remove(gameSave);
        await _context.SaveChangesAsync();
    }
}