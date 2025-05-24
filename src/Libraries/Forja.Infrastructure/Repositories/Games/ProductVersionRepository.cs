namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGameVersionRepository interface using Entity Framework Core.
/// </summary>
public class ProductVersionRepository : IProductVersionRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<ProductVersion> _gameVersions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductVersionRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public ProductVersionRepository(ForjaDbContext context)
    {
        _context = context;
        _gameVersions = context.Set<ProductVersion>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductVersion>> GetAllAsync()
    {
        return await _gameVersions
            .Include(gv => gv.Files)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ProductVersion?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game version ID.", nameof(id));
        }

        return await _gameVersions
            .Include(gv => gv.Files)
            .FirstOrDefaultAsync(gv => gv.Id == id);
    }

    public async Task<ProductVersion?> GetByProductIdPlatformAndVersionAsync(Guid productId, PlatformType platform, string version)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game ID.", nameof(productId));
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new ArgumentException("Invalid version.", nameof(version));
        }
        
        return await _gameVersions
            .Include(gv => gv.Files)
            .FirstOrDefaultAsync(gv => gv.ProductId == productId 
                                       && gv.Version == version
                                       && gv.Platform == platform);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductVersion>> GetByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game ID.", nameof(productId));
        }

        return await _gameVersions
            .Where(gv => gv.ProductId == productId)
            .Include(gv => gv.Files)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ProductVersion?> AddAsync(ProductVersion productVersion)
    {
        if (!GamesModelValidator.ValidateProductVersion(productVersion, out var errors))
        {
            throw new ArgumentException($"Invalid game version. Error: {errors}", nameof(productVersion));
        }

        await _gameVersions.AddAsync(productVersion);
        await _context.SaveChangesAsync();
        return await _gameVersions
            .Include(gv => gv.Files)
            .FirstOrDefaultAsync(gv => gv.ProductId == productVersion.ProductId);
    }

    /// <inheritdoc />
    public async Task<ProductVersion?> UpdateAsync(ProductVersion productVersion)
    {
        if (!GamesModelValidator.ValidateProductVersion(productVersion, out var errors))
        {
            throw new ArgumentException($"Invalid game version. Error: {errors}", nameof(productVersion));
        }

        _gameVersions.Update(productVersion);
        await _context.SaveChangesAsync();
        return await _gameVersions
            .Include(gv => gv.Files)
            .FirstOrDefaultAsync(gv => gv.ProductId == productVersion.ProductId);
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