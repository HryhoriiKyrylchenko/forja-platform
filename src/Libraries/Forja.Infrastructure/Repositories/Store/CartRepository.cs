using Forja.Domain.Enums;

namespace Forja.Infrastructure.Repositories.Store;

/// <summary>
/// Implementation of the Cart repository interface for managing Cart data.
/// </summary>
public class CartRepository : ICartRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<Cart> _carts;
    
    public CartRepository(ForjaDbContext context)
    {
        _context = context;
        _carts = context.Set<Cart>();
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<Cart?> GetActiveCartByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Invalid user ID.", nameof(userId));
        }
        
        return await _carts
            .Where(c => c.UserId == userId)
            .Where(c => c.Status == CartStatus.Active)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task AddCartAsync(Cart cart)
    {
        if (!StoreModelValidator.ValidateCartModel(cart, out string errors))
        {
            throw new ArgumentException(errors);
        }
        
        await _carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateCartAsync(Cart cart)
    {
        if (!StoreModelValidator.ValidateCartModel(cart, out string errors))
        {
            throw new ArgumentException(errors);
        }
        
        _carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<IEnumerable<Cart>> GetCartsByStatusAndInactivityAsync(CartStatus active,
        TimeSpan inactivityPeriod)
    {
        if (inactivityPeriod.TotalSeconds < 0)
        {
            throw new ArgumentException("Inactivity period cannot be negative.", nameof(inactivityPeriod));
        }
        
        var cutoffDate = DateTime.UtcNow - inactivityPeriod;

        return await _carts
            .Where(c => c.Status == active && c.LastModifiedAt <= cutoffDate)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Cart?> GetLatestAbandonedCartByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Invalid user ID.", nameof(userId));
        }
        
        return await _carts
            .Where(c => c.UserId == userId)
            .Where(c => c.Status == CartStatus.Abandoned)
            .OrderByDescending(c => c.LastModifiedAt)
            .FirstOrDefaultAsync();
    }
}