namespace Forja.Application.Interfaces.Store;

public interface ICartService
{
    // Cart Operations
    Task<CartDto?> GetCartByIdAsync(Guid cartId);
    Task<IEnumerable<CartDto>> GetCartsByUserIdAsync(Guid userId);
    Task<CartDto> GetOrCreateActiveCartAsync(CartCreateRequest request);
    Task RemoveCartAsync(Guid cartId);
    Task ArchiveCartAsync(Guid cartId);
    Task HandleAbandonedCartsAsync(TimeSpan inactivityPeriod);
    Task<CartDto?> RecoverAbandonedCartAsync(Guid userId);
    Task<bool> IsCartRelevantAsync(Guid cartId);
    Task<CartDto?> UpdateCartAsync(Guid cartId);

    // Cart Item Operations
    Task<CartItemDto?> GetCartItemByIdAsync(Guid cartItemId);
    Task<List<CartItemDto>> GetCartItemsByCartIdAsync(Guid cartId);
    Task<CartItemDto?> AddCartItemAsync(CartItemCreateRequest request);
    Task RemoveCartItemAsync(Guid cartItemId);
    Task RecalculateCartTotalAsync(Guid cartId);

    Task<List<CartItemDto>> AddBundleToCartAsync(CartAddBundleRequest request);
}