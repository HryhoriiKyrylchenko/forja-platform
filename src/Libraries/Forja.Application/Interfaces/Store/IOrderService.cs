namespace Forja.Application.Interfaces.Store;

/// <summary>
/// Interface to define operations related to order management.
/// </summary>
public interface IOrderService
{
    // Order Operations
    /// <summary>
    /// Retrieves order details for a given order ID asynchronously.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order to retrieve.</param>
    /// <returns>
    /// An <see cref="OrderDto"/> object containing order details if found; otherwise, null if no order exists with the given ID.
    /// </returns>
    Task<OrderDto?> GetOrderByIdAsync(Guid orderId);

    /// <summary>
    /// Retrieves all orders associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose orders are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="OrderDto"/> objects representing the user's orders.</returns>
    Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves all orders available in the system.
    /// </summary>
    /// <returns>An enumerable collection of order data transfer objects representing all orders.</returns>
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();

    /// <summary>
    /// Adds a new order based on the provided create request.
    /// </summary>
    /// <param name="request">The request object containing the necessary data to create an order.</param>
    /// <returns>
    /// Returns an <see cref="OrderDto"/> representing the newly created order if the operation is successful.
    /// Returns null if the order creation fails.
    /// </returns>
    Task<OrderDto?> AddOrderAsync(OrderCreateRequest request);

    /// <summary>
    /// Updates the status of an order based on the provided update request.
    /// </summary>
    /// <param name="request">The order update request containing the order ID and the new status.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated order data if successful; otherwise, null.
    /// </returns>
    Task<OrderDto?> UpdateOrderStatusAsync(OrderUpdateRequest request);

    /// <summary>
    /// Deletes an order asynchronously based on the provided order ID.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteOrderAsync(Guid orderId);
}