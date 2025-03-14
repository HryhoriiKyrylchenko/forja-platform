namespace Forja.Infrastructure.Repositories.Games;

/// <summary>
/// Implementation of the IProductImagesRepository interface using Entity Framework Core.
/// </summary>
public class ProductImagesRepository : IProductImagesRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<ProductImages> _productImages;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductImagesRepository"/> class with the provided DbContext.
    /// </summary>
    /// <param name="context">The database context to be used.</param>
    public ProductImagesRepository(ForjaDbContext context)
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
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid product image ID.", nameof(id));
        }
        
        return await _productImages
            .FirstOrDefaultAsync(pi => pi.Id == id);
    }

    /// <inheritdoc />
    public async Task<ProductImages?> AddAsync(ProductImages productImage)
    {
        if (!GamesModelValidator.ValidateProductImages(productImage, out _))
        {
            throw new ArgumentException("Invalid product image.", nameof(productImage));
        }
        
        await _productImages.AddAsync(productImage);
        await _context.SaveChangesAsync();
        return productImage;
    }

    /// <inheritdoc />
    public async Task<ProductImages?> UpdateAsync(ProductImages productImage)
    {
        if (!GamesModelValidator.ValidateProductImages(productImage, out _))
        {
            throw new ArgumentException("Invalid product image.", nameof(productImage));
        }

        _productImages.Update(productImage);
        await _context.SaveChangesAsync();
        return productImage;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid product image ID.", nameof(id));
        }
        
        var productImage = await _productImages.FindAsync(id);
        if (productImage == null)
        {
            throw new ArgumentException("Product image not found.", nameof(id));
        }
        
        _productImages.Remove(productImage);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImages>> GetAllWithDetailsAsync()
    {
        return await _productImages
            .Include(pi => pi.Product) 
            .Include(pi => pi.ItemImage) 
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImages>> GetByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Invalid product ID.", nameof(productId));
        }
        
        return await _productImages
            .Where(pi => pi.ProductId == productId)
            .Include(pi => pi.ItemImage) 
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImages>> GetByItemImageIdAsync(Guid itemImageId)
    {
        if (itemImageId == Guid.Empty)
        {
            throw new ArgumentException("Invalid item image ID.", nameof(itemImageId));
        }
        
        return await _productImages
            .Where(pi => pi.ItemImageId == itemImageId)
            .Include(pi => pi.Product) 
            .ToListAsync();
    }
}