namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IItemImageRepository interface using Entity Framework Core.
/// </summary>
public class ItemImageRepository : IItemImageRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<ItemImage> _itemImages;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemImageRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public ItemImageRepository(ForjaDbContext context)
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
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid item image ID.", nameof(id));
        }
        
        return await _itemImages
            .FirstOrDefaultAsync(ii => ii.Id == id);
    }

    /// <inheritdoc />
    public async Task<ItemImage?> AddAsync(ItemImage itemImage)
    {
        if (!GamesModelValidator.ValidateItemImage(itemImage, out _))
        {
            throw new ArgumentException("Invalid item image.", nameof(itemImage));
        }
        
        await _itemImages.AddAsync(itemImage);
        await _context.SaveChangesAsync();
        return itemImage;
    }

    /// <inheritdoc />
    public async Task<ItemImage?> UpdateAsync(ItemImage itemImage)
    {
        if (!GamesModelValidator.ValidateItemImage(itemImage, out _))
        {
            throw new ArgumentException("Invalid item image.", nameof(itemImage));
        }

        _itemImages.Update(itemImage);
        await _context.SaveChangesAsync();
        return itemImage;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid item image ID.", nameof(id));
        }
        
        var itemImage = await _itemImages.FindAsync(id);
        if (itemImage == null)
        {
            throw new ArgumentException("Item image not found.", nameof(id));
        }
        
        _itemImages.Remove(itemImage);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ItemImage>> GetAllWithProductImagesAsync()
    {
        return await _itemImages
            .Include(ii => ii.ProductImages) 
            .ThenInclude(pi => pi.Product)  
            .ToListAsync();
    }
}