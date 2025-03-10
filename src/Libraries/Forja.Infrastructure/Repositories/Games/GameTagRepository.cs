namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGameTagRepository interface using Entity Framework Core.
/// </summary>
public class GameTagRepository : IGameTagRepository
{
    private readonly DbContext _context;
    private readonly DbSet<GameTag> _gameTags;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameTagRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GameTagRepository(DbContext context)
    {
        _context = context;
        _gameTags = context.Set<GameTag>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameTag>> GetAllAsync()
    {
        return await _gameTags
            .Include(gt => gt.Game) // Include associated Game
            .Include(gt => gt.Tag)  // Include associated Tag
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameTag?> GetByIdAsync(Guid id)
    {
        return await _gameTags
            .Include(gt => gt.Game) // Include associated Game
            .Include(gt => gt.Tag)  // Include associated Tag
            .FirstOrDefaultAsync(gt => gt.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameTag>> GetByGameIdAsync(Guid gameId)
    {
        return await _gameTags
            .Where(gt => gt.GameId == gameId)
            .Include(gt => gt.Tag) // Include associated Tag
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GameTag>> GetByTagIdAsync(Guid tagId)
    {
        return await _gameTags
            .Where(gt => gt.TagId == tagId)
            .Include(gt => gt.Game) // Include associated Game
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<GameTag?> AddAsync(GameTag gameTag)
    {
        await _gameTags.AddAsync(gameTag);
        await _context.SaveChangesAsync();
        return gameTag;
    }

    /// <inheritdoc />
    public async Task<GameTag?> UpdateAsync(GameTag gameTag)
    {
        _gameTags.Update(gameTag);
        await _context.SaveChangesAsync();
        return gameTag;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var gameTag = await _gameTags.FindAsync(id);
        if (gameTag != null)
        {
            _gameTags.Remove(gameTag);
            await _context.SaveChangesAsync();
        }
    }
}