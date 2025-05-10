namespace Forja.Application.Interfaces.Store;

/// <summary>
/// Provides operations for managing shopping carts and their associated items.
/// This service supports various functionalities for handling user-specific carts,
/// cart items, abandoned carts, and more.
/// </summary>
public interface ICartService
{
    // Cart Operations
    /// <summary>
    /// Retrieves a shopping cart by its unique identifier.
    /// </summary>
    /// <param name="cartId">The unique identifier of the shopping cart to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CartDto"/> if the cart is found, <c>null</c> otherwise.</returns>
    Task<CartDto?> GetCartByIdAsync(Guid cartId);

    /// <summary>
    /// Retrieves all shopping carts associated with a specified user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose carts are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="CartDto"/> objects associated with the given user.</returns>
    Task<IEnumerable<CartDto>> GetCartsByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves the active shopping cart for a user or creates a new one if no active cart exists.
    /// </summary>
    /// <param name="request">An object containing the details needed to identify the user and create a cart, if necessary.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the active <see cref="CartDto"/> for the user.</returns>
    Task<CartDto> GetOrCreateActiveCartAsync(CartCreateRequest request);

    /// <summary>
    /// Removes a shopping cart by its unique identifier.
    /// </summary>
    /// <param name="cartId">The unique identifier of the shopping cart to remove.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveCartAsync(Guid cartId);

    /// <summary>
    /// Archives a shopping cart by marking it as archived in the system.
    /// </summary>
    /// <param name="cartId">The unique identifier of the shopping cart to archive.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ArchiveCartAsync(Guid cartId);

    /// <summary>
    /// Handles the processing of abandoned shopping carts based on a specified inactivity period.
    /// </summary>
    /// <param name="inactivityPeriod">The duration of inactivity after which carts are considered abandoned.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task HandleAbandonedCartsAsync(TimeSpan inactivityPeriod);

    /// <summary>
    /// Recovers the most recently abandoned cart of the specified user and marks it as active.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose abandoned cart is to be recovered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CartDto"/> of the recovered cart if found, <c>null</c> otherwise.</returns>
    Task<CartDto?> RecoverAbandonedCartAsync(Guid userId);

    /// <summary>
    /// Determines whether the specified shopping cart is still relevant based on its status, contents, and total amount validation.
    /// </summary>
    /// <param name="cartId">The unique identifier of the shopping cart to evaluate.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the cart is relevant.</returns>
    Task<bool> IsCartRelevantAsync(Guid cartId);

    /// <summary>
    /// Updates an existing shopping cart and recalculates its total based on the associated cart items.
    /// </summary>
    /// <param name="cartId">The unique identifier of the shopping cart to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see cref="CartDto"/> if the operation is successful, <c>null</c> otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided cart ID is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the cart cannot be updated.</exception>
    Task<CartDto?> UpdateCartAsync(Guid cartId);

    // Cart Item Operations
    /// <summary>
    /// Retrieves a specific cart item by its unique identifier.
    /// </summary>
    /// <param name="cartItemId">The unique identifier of the cart item to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CartItemDto"/> if the cart item is found, <c>null</c> otherwise.</returns>
    Task<CartItemDto?> GetCartItemByIdAsync(Guid cartItemId);

    /// <summary>
    /// Retrieves all items in a shopping cart by the cart's unique identifier.
    /// </summary>
    /// <param name="cartId">The unique identifier of the shopping cart.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="CartItemDto"/> associated with the specified cart.</returns>
    Task<List<CartItemDto>> GetCartItemsByCartIdAsync(Guid cartId);

    /// <summary>
    /// Adds a new item to the specified shopping cart.
    /// </summary>
    /// <param name="request">The request containing details about the cart and product to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CartItemDto"/> for the added item, or <c>null</c> if the operation fails.</returns>
    Task<CartItemDto?> AddCartItemAsync(CartItemCreateRequest request);

    /// <summary>
    /// Removes an item from a shopping cart by its unique identifier.
    /// </summary>
    /// <param name="cartItemId">The unique identifier of the cart item to remove.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<CartDto?> RemoveCartItemAsync(Guid cartItemId);

    /// <summary>
    /// Recalculates the total price of a shopping cart based on its current items and applicable discounts.
    /// </summary>
    /// <param name="cartId">The unique identifier of the shopping cart whose total is being recalculated.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RecalculateCartTotalAsync(Guid cartId);

    /// <summary>
    /// Adds the products of a specified bundle to an active shopping cart.
    /// </summary>
    /// <param name="request">The details of the request, including the cart ID and bundle ID to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="CartItemDto"/> for the added bundle products.</returns>
    Task<List<CartItemDto>> AddBundleToCartAsync(CartAddBundleRequest request);
}