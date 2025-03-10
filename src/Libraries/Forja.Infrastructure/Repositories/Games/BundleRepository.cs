namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IBundleRepository interface using Entity Framework Core.
/// </summary>
public class BundleRepository : IBundleRepository
{
    private readonly DbContext _context;
    private readonly DbSet<Bundle> _bundles;

    /// <summary>
    /// Initializes a new instance of the <see cref="BundleRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public BundleRepository(DbContext context)
    {
        _context = context;
        _bundles = context.Set<Bundle>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Bundle>> GetAllAsync()
    {
        return await _bundles
            .Include(b => b.BundleProducts) // Include related data if needed
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Bundle?> GetByIdAsync(Guid id)
    {
        return await _bundles
            .Include(b => b.BundleProducts) // Include related data if needed
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    /// <inheritdoc />
    public async Task<Bundle?> AddAsync(Bundle bundle)
    {
        await _bundles.AddAsync(bundle);
        await _context.SaveChangesAsync();
        return bundle;
    }

    /// <inheritdoc />
    public async Task<Bundle?> UpdateAsync(Bundle bundle)
    {
        _bundles.Update(bundle);
        await _context.SaveChangesAsync();
        return bundle;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var bundle = await _bundles.FindAsync(id);
        if (bundle != null)
        {
            _bundles.Remove(bundle);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Bundle>> GetActiveBundlesAsync()
    {
        return await _bundles
            .Where(b => b.IsActive)
            .Include(b => b.BundleProducts)
            .ToListAsync();
    }
}