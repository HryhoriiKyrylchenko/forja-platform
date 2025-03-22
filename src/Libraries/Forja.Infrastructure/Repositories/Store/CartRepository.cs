namespace Forja.Infrastructure.Repositories.Store;

/// <summary>
/// Implementation of the Cart repository interface for managing Cart data.
/// </summary>
public class CartRepository : ICartRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<Cart> _carts;

    /// <summary>
    /// Initializes a new instance of the CartRepository class.
    /// </summary>
    /// <param name="context">The database context to use for operations.</param>
    public CartRepository(ForjaDbContext context)
    {
        _context = context;
        _carts = context.Set<Cart>();
    }

    /// <summary>
    /// Retrieves a cart by its unique identifier.
    /// </summary>
    /// <param name="cartId">The cart's unique identifier.</param>
    /// <returns>The matching Cart object or null if not found.</returns>
    public async Task<Cart?> GetCartByIdAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Invalid cart ID.", nameof(cartId));
        }
        
        return await _carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.Id == cartId);
    }

    /// <summary>
    /// Retrieves all carts for a specific user by user ID.
    /// </summary>
    /// <param name="userId">The user's unique identifier.</param>
    /// <returns>A list of all carts matching the user ID.</returns>
    public async Task<IEnumerable<Cart>> GetCartsByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Invalid user ID.", nameof(userId));
        }
        
        return await _carts
            .Include(c => c.CartItems)
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new Cart to the database.
    /// </summary>
    /// <param name="cart">The Cart to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddCartAsync(Cart cart)
    {
        if (!StoreModelValidator.ValidateCartModel(cart, out string errors))
        {
            throw new ArgumentException(errors);
        }
        
        await _carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing Cart in the database.
    /// </summary>
    /// <param name="cart">The Cart to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateCartAsync(Cart cart)
    {
        if (!StoreModelValidator.ValidateCartModel(cart, out string errors))
        {
            throw new ArgumentException(errors);
        }
        
        _carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a Cart from the database by its ID.
    /// </summary>
    /// <param name="cartId">The cart's unique identifier.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteCartAsync(Guid cartId)
    {
        if (cartId == Guid.Empty)
        {
            throw new ArgumentException("Invalid cart ID.", nameof(cartId));
        }
        
        var cart = await _carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.Id == cartId);
        
        if (cart == null)
        {
            throw new KeyNotFoundException($"Cart with ID {cartId} not found.");
        }

        if (cart.CartItems.Any())
        {
            foreach (var cartItem in cart.CartItems)
            {
                _context.CartItems.Remove(cartItem);
            }
        }

        _carts.Remove(cart);
        await _context.SaveChangesAsync();
    }
}