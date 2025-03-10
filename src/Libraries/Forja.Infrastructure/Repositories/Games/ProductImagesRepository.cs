namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IProductImagesRepository interface using Entity Framework Core.
/// </summary>
public class ProductImagesRepository : IProductImagesRepository
{
    private readonly DbContext _context;
    private readonly DbSet<ProductImages> _productImages;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductImagesRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public ProductImagesRepository(DbContext context)
    {
        _context = context;
        _productImages = context.Set<ProductImages>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImages>> GetAllAsync()
    {
        return await _productImages
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ProductImages?> GetByIdAsync(Guid id)
    {
        return await _productImages
            .FirstOrDefaultAsync(pi => pi.Id == id);
    }

    /// <inheritdoc />
    public async Task<ProductImages?> AddAsync(ProductImages productImage)
    {
        await _productImages.AddAsync(productImage);
        await _context.SaveChangesAsync();
        return productImage;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var productImage = await _productImages.FindAsync(id);
        if (productImage != null)
        {
            _productImages.Remove(productImage);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImages>> GetAllWithDetailsAsync()
    {
        return await _productImages
            .Include(pi => pi.Product) // Include linked Product details
            .Include(pi => pi.ItemImage) // Include linked ItemImage details
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImages>> GetByProductIdAsync(Guid productId)
    {
        return await _productImages
            .Where(pi => pi.ProductId == productId)
            .Include(pi => pi.ItemImage) // Optionally include linked ItemImage details
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImages>> GetByItemImageIdAsync(Guid itemImageId)
    {
        return await _productImages
            .Where(pi => pi.ItemImageId == itemImageId)
            .Include(pi => pi.Product) // Optionally include linked Product details
            .ToListAsync();
    }
}