namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IBundleProductRepository interface using Entity Framework Core.
/// </summary>
public class BundleProductRepository : IBundleProductRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<BundleProduct> _bundleProducts;
    private readonly IProductRepository _productRepository;

    public BundleProductRepository(ForjaDbContext context,
        IProductRepository productRepository)
    {
        _context = context;
        _bundleProducts = context.Set<BundleProduct>();
        _productRepository = productRepository;
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
        
        var createdBundleProduct = await _bundleProducts
                                    .Include(bp => bp.Bundle) 
                                    .Include(bp => bp.Product) 
                                    .FirstOrDefaultAsync(bp => bp.Id == bundleProduct.Id);
        return createdBundleProduct;
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
        
        var updatedBundleProduct = await _bundleProducts
                                        .Include(bp => bp.Bundle) 
                                        .Include(bp => bp.Product) 
                                        .FirstOrDefaultAsync(bp => bp.Id == bundleProduct.Id);
        return updatedBundleProduct;
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
    
    /// <inheritdoc />
    public async Task<List<BundleProduct>> DistributeBundlePrice(List<BundleProduct> bundleProducts, decimal bundleTotalPrice)
    {
        if (bundleProducts == null || bundleProducts.Count == 0)
        {
            throw new ArgumentException("Bundle products cannot be null or empty", nameof(bundleProducts));
        }
        
        var productIds = bundleProducts.Select(bp => bp.ProductId).ToList();

        var products = await _productRepository.GetProductsByIdsAsync(productIds);

        var productMap = products.ToDictionary(p => p.Id, p => p.Price);

        var totalOriginalPrice = bundleProducts.Sum(bp =>
            productMap.GetValueOrDefault(bp.ProductId, 0m));

        if (totalOriginalPrice == 0)
        {
            var equalPrice = Math.Round(bundleTotalPrice / bundleProducts.Count, 2);
            foreach (var bp in bundleProducts)
            {
                bp.DistributedPrice = equalPrice;
            }
        }
        else
        {
            decimal distributedTotal = 0;

            for (int i = 0; i < bundleProducts.Count; i++)
            {
                var productId = bundleProducts[i].ProductId;
                var productPrice = productMap.GetValueOrDefault(productId, 0m);

                if (i == bundleProducts.Count - 1)
                {
                    bundleProducts[i].DistributedPrice = Math.Round(bundleTotalPrice - distributedTotal, 2);
                }
                else
                {
                    var proportionalPrice = productPrice / totalOriginalPrice * bundleTotalPrice;
                    var roundedPrice = Math.Round(proportionalPrice, 2);
                    bundleProducts[i].DistributedPrice = roundedPrice;
                    distributedTotal += roundedPrice;
                }
            }
        }
        
        return bundleProducts;
    }

    public async Task<bool> HasBundleProducts(Guid bundleId)
    {
        if (bundleId == Guid.Empty)
        {
            throw new ArgumentException("Bundle id cannot be empty", nameof(bundleId));
        }
        
        return await _bundleProducts
            .AnyAsync(bp => bp.BundleId == bundleId);
    }
}