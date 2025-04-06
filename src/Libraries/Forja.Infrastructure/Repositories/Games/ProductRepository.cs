namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IProductRepository interface using Entity Framework Core.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<Product> _products;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public ProductRepository(ForjaDbContext context)
    {
        _context = context;
        _products = context.Set<Product>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _products
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid product ID.", nameof(id));
        }
        
        return await _products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> GetByTypeAsync<T>() where T : Product
    {
        return await _products
            .OfType<T>()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsByIdsAsync(List<Guid> productIds)
    {
        if (productIds.Count == 0)
        {
            return new List<Product>();
        }
        
        return await _products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();
    }
}
