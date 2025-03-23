namespace Forja.Application.Interfaces.Store;

public interface IOrderService
{
    // Order Operations
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task AddOrderAsync(OrderCreateRequest request);
    Task UpdateOrderAsync(OrderUpdateRequest request);
    Task DeleteOrderAsync(Guid orderId);
}