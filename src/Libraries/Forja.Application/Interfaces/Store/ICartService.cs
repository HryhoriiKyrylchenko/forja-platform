namespace Forja.Application.Interfaces.Store;

public interface ICartService
{
    // Cart Operations
    Task<Cart?> GetCartByIdAsync(Guid cartId);
    Task<IEnumerable<Cart>> GetCartsByUserIdAsync(Guid userId);
    Task<Cart> GetOrCreateActiveCartAsync(CartCreateRequest request);
    Task RemoveCartAsync(Guid cartId);
    Task ArchiveCartAsync(Guid cartId);
    Task HandleAbandonedCartsAsync(TimeSpan inactivityPeriod);
    Task<Cart?> RecoverAbandonedCartAsync(Guid userId);

    // Cart Item Operations
    Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId);
    Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(Guid cartId);
    Task AddCartItemAsync(CartItemCreateRequest request);
    Task UpdateCartItemAsync(CartItemUpdateRequest request);
    Task RemoveCartItemAsync(Guid cartItemId);

    Task RecalculateCartTotalAsync(Guid cartId);
}