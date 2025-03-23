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

    // Cart Item Operations
    Task<CartItemDto?> GetCartItemByIdAsync(Guid cartItemId);
    Task<IEnumerable<CartItemDto>> GetCartItemsByCartIdAsync(Guid cartId);
    Task<CartItemDto?> AddCartItemAsync(CartItemCreateRequest request);
    Task<CartItemDto?> UpdateCartItemAsync(CartItemUpdateRequest request);
    Task RemoveCartItemAsync(Guid cartItemId);

    Task RecalculateCartTotalAsync(Guid cartId);
}