namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IProductMatureContentRepository interface using Entity Framework Core.
/// </summary>
public class ProductMatureContentRepository : IProductMatureContentRepository
{
    private readonly DbContext _context;
    private readonly DbSet<ProductMatureContent> _productMatureContents;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductMatureContentRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public ProductMatureContentRepository(DbContext context)
    {
        _context = context;
        _productMatureContents = context.Set<ProductMatureContent>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductMatureContent>> GetAllAsync()
    {
        return await _productMatureContents
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ProductMatureContent?> GetByIdAsync(Guid id)
    {
        return await _productMatureContents
            .FirstOrDefaultAsync(pmc => pmc.Id == id);
    }

    /// <inheritdoc />
    public async Task<ProductMatureContent?> AddAsync(ProductMatureContent productMatureContent)
    {
        await _productMatureContents.AddAsync(productMatureContent);
        await _context.SaveChangesAsync();
        return productMatureContent;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var productMatureContent = await _productMatureContents.FindAsync(id);
        if (productMatureContent != null)
        {
            _productMatureContents.Remove(productMatureContent);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductMatureContent>> GetAllWithDetailsAsync()
    {
        return await _productMatureContents
            .Include(pmc => pmc.Product)       // Include linked Product details
            .Include(pmc => pmc.MatureContent) // Include linked MatureContent details
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductMatureContent>> GetByProductIdAsync(Guid productId)
    {
        return await _productMatureContents
            .Where(pmc => pmc.ProductId == productId)
            .Include(pmc => pmc.MatureContent) // Optionally include MatureContent details
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductMatureContent>> GetByMatureContentIdAsync(Guid matureContentId)
    {
        return await _productMatureContents
            .Where(pmc => pmc.MatureContentId == matureContentId)
            .Include(pmc => pmc.Product) // Optionally include Product details
            .ToListAsync();
    }
}