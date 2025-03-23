namespace Forja.Application.Interfaces.Store;

public interface IOrderService
{
    // Order Operations
    Task<OrderDto?> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(Guid userId);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task<OrderDto?> AddOrderAsync(OrderCreateRequest request);
    Task<OrderDto?> UpdateOrderStatusAsync(OrderUpdateRequest request);
    Task DeleteOrderAsync(Guid orderId);
}