namespace Forja.Domain.Repositories.Store;

/// <summary>
/// Interface for the Order repository, defining methods for Order CRUD operations.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Retrieves an Order by its unique identifier.
    /// </summary>
    /// <param name="orderId">The unique identifier of the Order.</param>
    /// <returns>The matching Order object or null if not found.</returns>
    Task<Order?> GetOrderByIdAsync(Guid orderId);

    /// <summary>
    /// Retrieves all Orders placed by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of all Orders placed by the specified user.</returns>
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves all Orders in the database.
    /// </summary>
    /// <returns>A list of all Orders.</returns>
    Task<IEnumerable<Order>> GetAllOrdersAsync();

    /// <summary>
    /// Adds a new Order to the database.
    /// </summary>
    /// <param name="order">The Order object to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddOrderAsync(Order order);

    /// <summary>
    /// Updates an existing Order in the database.
    /// </summary>
    /// <param name="order">The Order object to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateOrderAsync(Order order);

    /// <summary>
    /// Deletes an Order from the database by its unique identifier.
    /// </summary>
    /// <param name="orderId">The unique identifier of the Order to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteOrderAsync(Guid orderId);
}