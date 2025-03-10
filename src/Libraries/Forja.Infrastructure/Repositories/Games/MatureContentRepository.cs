namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IMatureContentRepository interface using Entity Framework Core.
/// </summary>
public class MatureContentRepository : IMatureContentRepository
{
    private readonly DbContext _context;
    private readonly DbSet<MatureContent> _matureContents;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatureContentRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public MatureContentRepository(DbContext context)
    {
        _context = context;
        _matureContents = context.Set<MatureContent>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MatureContent>> GetAllAsync()
    {
        return await _matureContents
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<MatureContent?> GetByIdAsync(Guid id)
    {
        return await _matureContents
            .FirstOrDefaultAsync(mc => mc.Id == id);
    }

    /// <inheritdoc />
    public async Task<MatureContent?> AddAsync(MatureContent matureContent)
    {
        await _matureContents.AddAsync(matureContent);
        await _context.SaveChangesAsync();
        return matureContent;
    }

    /// <inheritdoc />
    public async Task<MatureContent?> UpdateAsync(MatureContent matureContent)
    {
        _matureContents.Update(matureContent);
        await _context.SaveChangesAsync();
        return matureContent;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var matureContent = await _matureContents.FindAsync(id);
        if (matureContent != null)
        {
            _matureContents.Remove(matureContent);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MatureContent>> GetAllWithProductMatureContentsAsync()
    {
        return await _matureContents
            .Include(mc => mc.ProductMatureContents) // Include ProductMatureContents relationship
            .ThenInclude(pm => pm.Product) // Optionally include the related Product entities
            .ToListAsync();
    }
}