namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IBundleProductRepository interface using Entity Framework Core.
/// </summary>
public class BundleProductRepository : IBundleProductRepository
{
    private readonly DbContext _context;
    private readonly DbSet<BundleProduct> _bundleProducts;

    /// <summary>
    /// Initializes a new instance of the <see cref="BundleProductRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public BundleProductRepository(DbContext context)
    {
        _context = context;
        _bundleProducts = context.Set<BundleProduct>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BundleProduct>> GetAllAsync()
    {
        return await _bundleProducts
            .Include(bp => bp.Bundle) // Include the related bundle
            .Include(bp => bp.Product) // Include the related product
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<BundleProduct?> GetByIdAsync(Guid id)
    {
        return await _bundleProducts
            .Include(bp => bp.Bundle) // Include the related bundle
            .Include(bp => bp.Product) // Include the related product
            .FirstOrDefaultAsync(bp => bp.Id == id);
    }

    /// <inheritdoc />
    public async Task<BundleProduct?> AddAsync(BundleProduct bundleProduct)
    {
        await _bundleProducts.AddAsync(bundleProduct);
        await _context.SaveChangesAsync();
        return bundleProduct;
    }

    /// <inheritdoc />
    public async Task<BundleProduct?> UpdateAsync(BundleProduct bundleProduct)
    {
        _bundleProducts.Update(bundleProduct);
        await _context.SaveChangesAsync();
        return bundleProduct;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var bundleProduct = await _bundleProducts.FindAsync(id);
        if (bundleProduct != null)
        {
            _bundleProducts.Remove(bundleProduct);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BundleProduct>> GetByBundleIdAsync(Guid bundleId)
    {
        return await _bundleProducts
            .Where(bp => bp.BundleId == bundleId)
            .Include(bp => bp.Product) // Include related product
            .ToListAsync();
    }
}