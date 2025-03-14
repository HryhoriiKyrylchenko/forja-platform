namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IBundleProductRepository interface using Entity Framework Core.
/// </summary>
public class BundleProductRepository : IBundleProductRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<BundleProduct> _bundleProducts;

    /// <summary>
    /// Initializes a new instance of the <see cref="BundleProductRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public BundleProductRepository(ForjaDbContext context)
    {
        _context = context;
        _bundleProducts = context.Set<BundleProduct>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BundleProduct>> GetAllAsync()
    {
        return await _bundleProducts
            .Include(bp => bp.Bundle) 
            .Include(bp => bp.Product) 
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<BundleProduct?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(id));
        }
        
        return await _bundleProducts
            .Include(bp => bp.Bundle) 
            .Include(bp => bp.Product) 
            .FirstOrDefaultAsync(bp => bp.Id == id);
    }

    /// <inheritdoc />
    public async Task<BundleProduct?> AddAsync(BundleProduct bundleProduct)
    {
        if (!GamesModelValidator.ValidateBundleProduct(bundleProduct, out _))
        {
            throw new ArgumentException("Invalid bundle product", nameof(bundleProduct));
        }
        
        await _bundleProducts.AddAsync(bundleProduct);
        await _context.SaveChangesAsync();
        return bundleProduct;
    }

    /// <inheritdoc />
    public async Task<BundleProduct?> UpdateAsync(BundleProduct bundleProduct)
    {
        if (!GamesModelValidator.ValidateBundleProduct(bundleProduct, out _))
        {
            throw new ArgumentException("Invalid bundle product", nameof(bundleProduct));
        }
        
        _bundleProducts.Update(bundleProduct);
        await _context.SaveChangesAsync();
        return bundleProduct;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(id));
        }
        
        var bundleProduct = await _bundleProducts.FindAsync(id);
        if (bundleProduct == null)
        {
            throw new ArgumentException("Bundle product not found", nameof(id));
        }
        
        _bundleProducts.Remove(bundleProduct);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BundleProduct>> GetByBundleIdAsync(Guid bundleId)
    {
        if (bundleId == Guid.Empty)
        {
            throw new ArgumentException("Bundle id cannot be empty", nameof(bundleId));
        }
        
        return await _bundleProducts
            .Where(bp => bp.BundleId == bundleId)
            .Include(bp => bp.Product)
            .ToListAsync();
    }
}