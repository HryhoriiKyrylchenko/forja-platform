namespace Forja.Domain.Repositories.Store;

/// <summary>
/// Interface for the Cart repository, defining methods for Cart CRUD operations.
/// </summary>
public interface ICartRepository
{
    /// <summary>
    /// Retrieves a cart by its unique identifier.
    /// </summary>
    /// <param name="cartId">The unique cart identifier.</param>
    /// <returns>The matching Cart object or null if not found.</returns>
    Task<Cart?> GetCartByIdAsync(Guid cartId);

    /// <summary>
    /// Retrieves all carts for a specific user by user ID.
    /// </summary>
    /// <param name="userId">The unique user ID.</param>
    /// <returns>A list of all carts for the provided user.</returns>
    Task<IEnumerable<Cart>> GetCartsByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves the active cart for a specific user by their user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The active Cart object for the specified user, or null if none exists.</returns>
    Task<Cart?> GetActiveCartByUserIdAsync(Guid userId);
    
    /// <summary>
    /// Adds a new Cart to the database.
    /// </summary>
    /// <param name="cart">The Cart to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Cart?> AddCartAsync(Cart cart);

    /// <summary>
    /// Updates an existing Cart in the database.
    /// </summary>
    /// <param name="cart">The Cart to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Cart?> UpdateCartAsync(Cart cart);

    /// <summary>
    /// Deletes a Cart from the database by its ID.
    /// </summary>
    /// <param name="cartId">The ID of the cart to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteCartAsync(Guid cartId);

    /// <summary>
    /// Retrieves carts matching the specified status and inactivity period.
    /// </summary>
    /// <param name="active">The status of the carts to retrieve (e.g., Active, Abandoned, Archived).</param>
    /// <param name="inactivityPeriod">The time span representing the period of inactivity to filter carts.</param>
    /// <returns>A collection of carts that match the specified status and inactivity criteria.</returns>
    Task<IEnumerable<Cart>> GetCartsByStatusAndInactivityAsync(CartStatus active, TimeSpan inactivityPeriod);

    /// <summary>
    /// Retrieves the latest abandoned cart for a specific user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The latest abandoned Cart object for the specified user or null if none exist.</returns>
    Task<Cart?> GetLatestAbandonedCartByUserIdAsync(Guid userId);
}