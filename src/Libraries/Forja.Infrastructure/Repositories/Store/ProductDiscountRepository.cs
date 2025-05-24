namespace Forja.Infrastructure.Repositories.Store;

/// <summary>
/// Implementation of the ProductDiscount repository interface for managing ProductDiscount data.
/// </summary>
public class ProductDiscountRepository : IProductDiscountRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<ProductDiscount> _productDiscounts;

    /// <summary>
    /// Initializes a new instance of the ProductDiscountRepository class.
    /// </summary>
    /// <param name="context">The database context to use for operations.</param>
    public ProductDiscountRepository(ForjaDbContext context)
    {
        _context = context;
        _productDiscounts = context.Set<ProductDiscount>();
    }

    /// <summary>
    /// Retrieves a ProductDiscount by its unique identifier.
    /// </summary>
    /// <param name="productDiscountId">The unique ID of the ProductDiscount.</param>
    /// <returns>The matching ProductDiscount object or null if not found.</returns>
    public async Task<ProductDiscount?> GetProductDiscountByIdAsync(Guid productDiscountId)
    {
        if (productDiscountId == Guid.Empty)
        {
            throw new ArgumentException("Invalid ProductDiscount ID.", nameof(productDiscountId));
        }

        return await _productDiscounts
            .Include(pd => pd.Product)
            .Include(pd => pd.Discount)
            .FirstOrDefaultAsync(pd => pd.Id == productDiscountId);
    }

    /// <summary>
    /// Retrieves all ProductDiscounts associated with a specific product.
    /// </summary>
    /// <param name="productId">The unique ID of the product.</param>
    /// <returns>A list of ProductDiscounts for the specified product.</returns>
    public async Task<IEnumerable<ProductDiscount>> GetProductDiscountsByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Invalid product ID.", nameof(productId));
        }

        return await _productDiscounts
            .Include(pd => pd.Discount)
            .Where(pd => pd.ProductId == productId)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all ProductDiscounts associated with a specific discount.
    /// </summary>
    /// <param name="discountId">The unique ID of the discount.</param>
    /// <returns>A list of ProductDiscounts for the specified discount.</returns>
    public async Task<IEnumerable<ProductDiscount>> GetProductDiscountsByDiscountIdAsync(Guid discountId)
    {
        if (discountId == Guid.Empty)
        {
            throw new ArgumentException("Invalid discount ID.", nameof(discountId));
        }

        return await _productDiscounts
            .Include(pd => pd.Product)
            .Include(pd => pd.Discount)
            .Where(pd => pd.DiscountId == discountId)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new ProductDiscount to the database.
    /// </summary>
    /// <param name="productDiscount">The ProductDiscount object to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<ProductDiscount?> AddProductDiscountAsync(ProductDiscount productDiscount)
    {
        if (!StoreModelValidator.ValidateProductDiscountModel(productDiscount, out string errors))
        {
            throw new ArgumentException(errors);
        }

        await _productDiscounts.AddAsync(productDiscount);
        await _context.SaveChangesAsync();
        
        return productDiscount;
    }

    /// <summary>
    /// Updates an existing ProductDiscount in the database.
    /// </summary>
    /// <param name="productDiscount">The ProductDiscount object to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<ProductDiscount?> UpdateProductDiscountAsync(ProductDiscount productDiscount)
    {
        if (!StoreModelValidator.ValidateProductDiscountModel(productDiscount, out string errors))
        {
            throw new ArgumentException(errors);
        }

        _productDiscounts.Update(productDiscount);
        await _context.SaveChangesAsync();
        
        return productDiscount;
    }

    /// <summary>
    /// Deletes a ProductDiscount from the database by its unique identifier.
    /// </summary>
    /// <param name="productDiscountId">The unique ID of the ProductDiscount to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteProductDiscountAsync(Guid productDiscountId)
    {
        if (productDiscountId == Guid.Empty)
        {
            throw new ArgumentException("Invalid ProductDiscount ID.", nameof(productDiscountId));
        }

        var productDiscount = await _productDiscounts.FindAsync(productDiscountId);
        if (productDiscount == null)
        {
            throw new KeyNotFoundException($"ProductDiscount with ID {productDiscountId} not found.");
        }

        _productDiscounts.Remove(productDiscount);
        await _context.SaveChangesAsync();
    }
}