namespace Forja.Domain.Repositories.Store;

/// <summary>
/// Interface for the CartItem repository, defining methods for CartItem CRUD operations.
/// </summary>
public interface ICartItemRepository
{
    /// <summary>
    /// Retrieves a CartItem by its unique identifier.
    /// </summary>
    /// <param name="cartItemId">The unique identifier of the CartItem.</param>
    /// <returns>The matching CartItem object or null if not found.</returns>
    Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId);

    /// <summary>
    /// Retrieves all CartItems in a specific Cart by Cart ID.
    /// </summary>
    /// <param name="cartId">The unique identifier of the Cart.</param>
    /// <returns>A list of all CartItems in the specified Cart.</returns>
    Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(Guid cartId);

    /// <summary>
    /// Adds a new CartItem to the database.
    /// </summary>
    /// <param name="cartItem">The CartItem to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddCartItemAsync(CartItem cartItem);

    /// <summary>
    /// Updates an existing CartItem in the database.
    /// </summary>
    /// <param name="cartItem">The CartItem to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateCartItemAsync(CartItem cartItem);

    /// <summary>
    /// Deletes a CartItem from the database by its ID.
    /// </summary>
    /// <param name="cartItemId">The CartItem's unique identifier.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteCartItemAsync(Guid cartItemId);
}