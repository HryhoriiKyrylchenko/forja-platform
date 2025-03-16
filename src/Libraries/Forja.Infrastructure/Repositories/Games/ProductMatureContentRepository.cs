namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IProductMatureContentRepository interface using Entity Framework Core.
/// </summary>
public class ProductMatureContentRepository : IProductMatureContentRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<ProductMatureContent> _productMatureContents;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductMatureContentRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public ProductMatureContentRepository(ForjaDbContext context)
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
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid product mature content ID.", nameof(id));
        }
        
        return await _productMatureContents
            .FirstOrDefaultAsync(pmc => pmc.Id == id);
    }

    /// <inheritdoc />
    public async Task<ProductMatureContent?> AddAsync(ProductMatureContent productMatureContent)
    {
        if (!GamesModelValidator.ValidateProductMatureContent(productMatureContent, out _))
        {
            throw new ArgumentException("Invalid product mature content.", nameof(productMatureContent));
        }
        
        await _productMatureContents.AddAsync(productMatureContent);
        await _context.SaveChangesAsync();
        return productMatureContent;
    }

    /// <inheritdoc />
    public async Task<ProductMatureContent?> UpdateAsync(ProductMatureContent productMatureContent)
    {
        if (!GamesModelValidator.ValidateProductMatureContent(productMatureContent, out _))
        {
            throw new ArgumentException("Invalid product mature content.", nameof(productMatureContent));
        }

        _productMatureContents.Update(productMatureContent);
        await _context.SaveChangesAsync();
        return productMatureContent;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid product mature content ID.", nameof(id));
        }
        
        var productMatureContent = await _productMatureContents.FindAsync(id);
        if (productMatureContent == null)
        {
            throw new ArgumentException("Product mature content not found.", nameof(id));
        }
        
        _productMatureContents.Remove(productMatureContent);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductMatureContent>> GetAllWithDetailsAsync()
    {
        return await _productMatureContents
            .Include(pmc => pmc.Product)       
            .Include(pmc => pmc.MatureContent) 
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductMatureContent>> GetByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Invalid product ID.", nameof(productId));
        }
        
        return await _productMatureContents
            .Where(pmc => pmc.ProductId == productId)
            .Include(pmc => pmc.MatureContent) 
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductMatureContent>> GetByMatureContentIdAsync(Guid matureContentId)
    {
        if (matureContentId == Guid.Empty)
        {
            throw new ArgumentException("Invalid mature content ID.", nameof(matureContentId));
        }
        
        return await _productMatureContents
            .Where(pmc => pmc.MatureContentId == matureContentId)
            .Include(pmc => pmc.Product)
            .ToListAsync();
    }
}