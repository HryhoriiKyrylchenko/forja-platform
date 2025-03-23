namespace Forja.Infrastructure.Repositories.Store;

/// <summary>
/// Implementation of the Discount repository interface for managing Discount data.
/// </summary>
public class DiscountRepository : IDiscountRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<Discount> _discounts;

    /// <summary>
    /// Initializes a new instance of the DiscountRepository class.
    /// </summary>
    /// <param name="context">The database context to use for operations.</param>
    public DiscountRepository(ForjaDbContext context)
    {
        _context = context;
        _discounts = context.Set<Discount>();
    }

    /// <summary>
    /// Retrieves a Discount by its unique identifier.
    /// </summary>
    /// <param name="discountId">The discount's unique identifier.</param>
    /// <returns>The matching Discount object or null if not found.</returns>
    public async Task<Discount?> GetDiscountByIdAsync(Guid discountId)
    {
        if (discountId == Guid.Empty)
        {
            throw new ArgumentException("Invalid discount ID.", nameof(discountId));
        }

        return await _discounts
            .Where(d => !d.IsDeleted)
            .Include(d => d.ProductDiscounts)
                .ThenInclude(pd => pd.Product)
            .FirstOrDefaultAsync(d => d.Id == discountId);
    }

    /// <summary>
    /// Retrieves all Discounts in the database.
    /// </summary>
    /// <returns>A list of all Discounts.</returns>
    public async Task<IEnumerable<Discount>> GetAllDiscountsAsync()
    {
        return await _discounts
            .Where(d => !d.IsDeleted)
            .Include(d => d.ProductDiscounts)
                .ThenInclude(pd => pd.Product)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves active Discounts in the current time period.
    /// </summary>
    /// <param name="currentDate">The current date as a reference point.</param>
    /// <returns>A list of Discounts that are currently active.</returns>
    public async Task<IEnumerable<Discount>> GetActiveDiscountsAsync(DateTime currentDate)
    {
        return await _discounts
            .Where(d => !d.IsDeleted)
            .Where(d => (!d.StartDate.HasValue || d.StartDate <= currentDate) &&
                        (!d.EndDate.HasValue || d.EndDate >= currentDate))
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new Discount to the database.
    /// </summary>
    /// <param name="discount">The Discount to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<Discount?> AddDiscountAsync(Discount discount)
    {
        if (!StoreModelValidator.ValidateDiscountModel(discount, out string errors))
        {
            throw new ArgumentException(errors);
        }

        await _discounts.AddAsync(discount);
        await _context.SaveChangesAsync();
        
        return discount;
    }

    /// <summary>
    /// Updates an existing Discount in the database.
    /// </summary>
    /// <param name="discount">The Discount to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<Discount?> UpdateDiscountAsync(Discount discount)
    {
        if (!StoreModelValidator.ValidateDiscountModel(discount, out string errors))
        {
            throw new ArgumentException(errors);
        }

        _discounts.Update(discount);
        await _context.SaveChangesAsync();
        
        return discount;
    }

    /// <summary>
    /// Deletes a Discount from the database by its ID.
    /// </summary>
    /// <param name="discountId">The discount's unique identifier.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteDiscountAsync(Guid discountId)
    {
        if (discountId == Guid.Empty)
        {
            throw new ArgumentException("Invalid discount ID.", nameof(discountId));
        }

        var discount = await _discounts.FindAsync(discountId);
        if (discount == null)
        {
            throw new KeyNotFoundException($"Discount with ID {discountId} not found.");
        }

        discount.IsDeleted = true;
        _discounts.Update(discount);
        await _context.SaveChangesAsync();
    }
}