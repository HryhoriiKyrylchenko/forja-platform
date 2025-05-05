namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IGameFileRepository interface using Entity Framework Core.
/// </summary>
public class ProductFileRepository : IProductFileRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<ProductFile> _gameFiles;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductFileRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public ProductFileRepository(ForjaDbContext context)
    {
        _context = context;
        _gameFiles = context.Set<ProductFile>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductFile>> GetAllAsync()
    {
        return await _gameFiles
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ProductFile?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid game file ID.", nameof(id));
        }

        return await _gameFiles
            .FirstOrDefaultAsync(gf => gf.Id == id);
    }

    public async Task<ProductFile?> GetGameFileByGameVersionIdAndFileName(Guid productVersionId, string fileName)
    {
        if (productVersionId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game version ID.", nameof(productVersionId));
        }
        
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Invalid file name.", nameof(fileName));
        }
        
        return await _gameFiles
            .FirstOrDefaultAsync(gf => gf.ProductVersionId == productVersionId && gf.FileName == fileName);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductFile>> GetByGameVersionIdAsync(Guid gameVersionId)
    {
        if (gameVersionId == Guid.Empty)
        {
            throw new ArgumentException("Invalid game version ID.", nameof(gameVersionId));
        }

        return await _gameFiles
            .Where(gf => gf.ProductVersionId == gameVersionId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ProductFile?> AddAsync(ProductFile productFile)
    {
        if (!GamesModelValidator.ValidateProductFile(productFile, out var errors))
        {
            throw new ArgumentException($"Invalid game file. Error: {errors}", nameof(productFile));
        }

        await _gameFiles.AddAsync(productFile);
        await _context.SaveChangesAsync();
        return productFile;
    }

    /// <inheritdoc />
    public async Task<ProductFile?> UpdateAsync(ProductFile productFile)
    {
        if (!GamesModelValidator.ValidateProductFile(productFile, out var errors))
        {
            throw new ArgumentException($"Invalid game file. Error: {errors}", nameof(productFile));
        }

        _gameFiles.Update(productFile);
        await _context.SaveChangesAsync();
        return productFile;
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

    public async Task<ProductFile?> FindByPlatformVersionProductIdAndNameAsync(PlatformType platform, string version, Guid productId, string fileName)
    {
        if (string.IsNullOrWhiteSpace(version))
        {
            throw new ArgumentException("Invalid version.", nameof(version));
        }
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Invalid file name.", nameof(fileName));
        }
        
        return await _gameFiles
            .FirstOrDefaultAsync(gf => gf.ProductVersion.Platform == platform 
                                                && gf.ProductVersion.ProductId == productId
                                                && gf.ProductVersion.Version == version 
                                                && gf.FileName == fileName);
    }
}