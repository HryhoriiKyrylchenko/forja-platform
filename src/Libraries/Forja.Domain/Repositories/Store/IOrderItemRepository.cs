namespace Forja.Domain.Repositories.Store;

/// <summary>
/// Interface for the OrderItem repository, defining methods for OrderItem CRUD operations.
/// </summary>
public interface IOrderItemRepository
{
    /// <summary>
    /// Retrieves an OrderItem by its unique identifier.
    /// </summary>
    /// <param name="orderItemId">The unique identifier of the OrderItem.</param>
    /// <returns>The matching OrderItem object or null if not found.</returns>
    Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId);

    /// <summary>
    /// Retrieves all OrderItems associated with a specific Order.
    /// </summary>
    /// <param name="orderId">The unique identifier of the Order.</param>
    /// <returns>A collection of OrderItems for the specified Order.</returns>
    Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(Guid orderId);

    /// <summary>
    /// Adds a new OrderItem to the database.
    /// </summary>
    /// <param name="orderItem">The OrderItem object to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddOrderItemAsync(OrderItem orderItem);

    /// <summary>
    /// Updates an existing OrderItem in the database.
    /// </summary>
    /// <param name="orderItem">The OrderItem object to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateOrderItemAsync(OrderItem orderItem);

    /// <summary>
    /// Deletes an OrderItem from the database by its unique identifier.
    /// </summary>
    /// <param name="orderItemId">The unique identifier of the OrderItem to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteOrderItemAsync(Guid orderItemId);
}