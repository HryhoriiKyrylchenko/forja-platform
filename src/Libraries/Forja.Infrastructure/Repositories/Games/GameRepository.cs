namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGameRepository interface using Entity Framework Core.
/// </summary>
public class GameRepository : IGameRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<Game> _games;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public GameRepository(ForjaDbContext context)
    {
        _context = context;
        _games = context.Set<Game>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await _games
            .Where(g => !g.IsDeleted)
            .ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<Game>> GetAllDeletedAsync()
    {
        return await _games
            .Where(g => g.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Game>> GetAllForCatalogAsync()
    {
        return await _games
            .Where(g => !g.IsDeleted)
            .Include(g => g.ProductGenres)
                .ThenInclude(pg => pg.Genre)
            .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
            .Include(g => g.ProductDiscounts)
                .ThenInclude(dc => dc.Discount)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Game?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game ID.", nameof(id));
        }
        
        return await _games
            .Where(g => !g.IsDeleted)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <inheritdoc />
    public async Task<Game?> GetExtendedByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game ID.", nameof(id));
        }
        
        return await _games
            .Where(g => !g.IsDeleted)
            .Include(g => g.ProductImages)
                .ThenInclude(pi => pi.ItemImage)
            .Include(g => g.GameAddons)
                .ThenInclude(ga => ga.ProductDiscounts)
                    .ThenInclude(dc => dc.Discount)
            .Include(g => g.ProductDiscounts)
                .ThenInclude(dc => dc.Discount)
            .Include(g => g.ProductGenres)
                .ThenInclude(pg => pg.Genre)
            .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
            .Include(g => g.GameMechanics)
                .ThenInclude(gm => gm.Mechanic)
            .Include(g => g.ProductMatureContents)
                .ThenInclude(mc => mc.MatureContent)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <inheritdoc />
    public async Task<Game?> AddAsync(Game game)
    {
        if (!GamesModelValidator.ValidateGame(game, out _))
        {
            throw new ArgumentException("Invalid game.", nameof(game));
        }
        
        await _games.AddAsync(game);
        await _context.SaveChangesAsync();
        return game;
    }

    /// <inheritdoc />
    public async Task<Game?> UpdateAsync(Game game)
    {
        if (!GamesModelValidator.ValidateGame(game, out _))
        {
            throw new ArgumentException("Invalid game.", nameof(game));
        }

        _games.Update(game);
        await _context.SaveChangesAsync();
        return game;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game ID.", nameof(id));
        }
        
        var game = await _games.FindAsync(id);
        if (game == null)
        {
            throw new ArgumentException("Game not found.", nameof(id));
        }
        
        game.IsDeleted = true;
        game.IsActive = false;
        game.ModifiedAt = DateTime.UtcNow;
        _games.Update(game);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Game>> GetByTagAsync(Guid tagId)
    {
        if (tagId == Guid.Empty)
        {
            throw new ArgumentException("Invalid tag ID.", nameof(tagId));
        }
        
        return await _games
            .Where(g => g.GameTags.Any(tag => tag.Id == tagId))
            .Where(g => !g.IsDeleted)
            .ToListAsync();
    }
}