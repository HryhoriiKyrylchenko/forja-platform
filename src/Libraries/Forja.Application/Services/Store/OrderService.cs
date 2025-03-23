namespace Forja.Application.Services.Store;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;

    public OrderService(IOrderRepository orderRepository, 
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
    }


    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
        }

        return await _orderRepository.GetOrderByIdAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        return await _orderRepository.GetOrdersByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllOrdersAsync();
    }

    public async Task AddOrderAsync(OrderCreateRequest request)
    {
        if (!StoreRequestsValidator.ValidateOrderCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }
        
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CartId = request.CartId,
            OrderDate = DateTime.UtcNow,
            PaymentStatus = OrderPaymentStatus.Pending
        };

        await _orderRepository.AddOrderAsync(order);
    }

    public async Task UpdateOrderAsync(OrderUpdateRequest request)
    {
        if (!StoreRequestsValidator.ValidateOrderUpdateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }

        var order = await _orderRepository.GetOrderByIdAsync(request.Id);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {request.Id} not found.");
        }
        
        order.PaymentStatus = request.PaymentStatus;

        await _orderRepository.UpdateOrderAsync(order);
    }

    public async Task DeleteOrderAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
        }

        await _orderRepository.DeleteOrderAsync(orderId);
    }
}