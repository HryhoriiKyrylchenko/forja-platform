namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IItemImageRepository interface using Entity Framework Core.
/// </summary>
public class ItemImageRepository : IItemImageRepository
{
    private readonly DbContext _context;
    private readonly DbSet<ItemImage> _itemImages;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemImageRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public ItemImageRepository(DbContext context)
    {
        _context = context;
        _itemImages = context.Set<ItemImage>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ItemImage>> GetAllAsync()
    {
        return await _itemImages
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ItemImage?> GetByIdAsync(Guid id)
    {
        return await _itemImages
            .FirstOrDefaultAsync(ii => ii.Id == id);
    }

    /// <inheritdoc />
    public async Task<ItemImage?> AddAsync(ItemImage itemImage)
    {
        await _itemImages.AddAsync(itemImage);
        await _context.SaveChangesAsync();
        return itemImage;
    }

    /// <inheritdoc />
    public async Task<ItemImage?> UpdateAsync(ItemImage itemImage)
    {
        _itemImages.Update(itemImage);
        await _context.SaveChangesAsync();
        return itemImage;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var itemImage = await _itemImages.FindAsync(id);
        if (itemImage != null)
        {
            _itemImages.Remove(itemImage);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ItemImage>> GetAllWithProductImagesAsync()
    {
        return await _itemImages
            .Include(ii => ii.ProductImages) // Include associated ProductImages
            .ThenInclude(pi => pi.Product)  // Include the related Products (optional)
            .ToListAsync();
    }
}