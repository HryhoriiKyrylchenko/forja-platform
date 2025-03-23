namespace Forja.Infrastructure.Repositories.Store;

/// <summary>
/// Implementation of the CartItem repository interface for managing CartItem data.
/// </summary>
public class CartItemRepository : ICartItemRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<CartItem> _cartItems;

    /// <summary>
    /// Initializes a new instance of the CartItemRepository class.
    /// </summary>
    /// <param name="context">The database context to use for operations.</param>
    public CartItemRepository(ForjaDbContext context)
    {
        _context = context;
        _cartItems = context.Set<CartItem>();
    }

    /// <summary>
    /// Retrieves a CartItem by its unique identifier.
    /// </summary>
    /// <param name="cartItemId">The unique identifier of the CartItem.</param>
    /// <returns>The matching CartItem object or null if not found.</returns>
    public async Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId)
    {
        if (cartItemId == Guid.Empty)
        {
            throw new ArgumentException("Invalid cart item ID.", nameof(cartItemId));
        }

        return await _cartItems
            .Include(ci => ci.Cart)
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
    }

    /// <summary>
    /// Retrieves all CartItems in a specific Cart by Cart ID.
    /// </summary>
    /// <param name="cartId">The unique identifier of the Cart.</param>
    /// <returns>A list of all CartItems in the specified Cart.</returns>
    public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Invalid cart ID.", nameof(cartId));
        }

        return await _cartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.CartId == cartId)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new CartItem to the database.
    /// </summary>
    /// <param name="cartItem">The CartItem to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<CartItem?> AddCartItemAsync(CartItem cartItem)
    {
        if (StoreModelValidator.ValidateCartItemModel(cartItem, out string errors))
        {
            throw new ArgumentException(errors);
        }

        await _cartItems.AddAsync(cartItem);
        await _context.SaveChangesAsync();
        
        return cartItem;
    }

    /// <summary>
    /// Updates an existing CartItem in the database.
    /// </summary>
    /// <param name="cartItem">The CartItem to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<CartItem?> UpdateCartItemAsync(CartItem cartItem)
    {
        if (StoreModelValidator.ValidateCartItemModel(cartItem, out string errors))
        {
            throw new ArgumentException(errors);
        }

        _cartItems.Update(cartItem);
        await _context.SaveChangesAsync();
        
        return cartItem;
    }

    /// <summary>
    /// Deletes a CartItem from the database by its ID.
    /// </summary>
    /// <param name="cartItemId">The CartItem's unique identifier.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteCartItemAsync(Guid cartItemId)
    {
        if (cartItemId == Guid.Empty)
        {
            throw new ArgumentException("Invalid cart item ID.", nameof(cartItemId));
        }

        var cartItem = await _cartItems.FindAsync(cartItemId);
        if (cartItem == null)
        {
            throw new KeyNotFoundException($"Cart item with ID {cartItemId} not found.");
        }

        _cartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
    }
}