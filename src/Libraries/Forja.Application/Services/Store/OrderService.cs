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
    
    public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
        }

        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        
        return order == null ? null : StoreEntityToDtoMapper.MapToOrderDto(order);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        var orders =  await _orderRepository.GetOrdersByUserIdAsync(userId);
        
        return orders.Select(StoreEntityToDtoMapper.MapToOrderDto);
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();
        
        return orders.Select(StoreEntityToDtoMapper.MapToOrderDto);
    }

    public async Task<OrderDto?> AddOrderAsync(OrderCreateRequest request)
    {
        if (!StoreRequestsValidator.ValidateOrderCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }
        
        var cart = await _cartRepository.GetCartByIdAsync(request.CartId);
        if (cart == null)
        {
            throw new InvalidOperationException("Cart not found.");
        }
        
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CartId = request.CartId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending
        };

        var result = await _orderRepository.AddOrderAsync(order);
        
        cart.Status = CartStatus.Archived;
        await _cartRepository.UpdateCartAsync(cart);
        
        return result == null ? null : StoreEntityToDtoMapper.MapToOrderDto(result);
    }

    public async Task<OrderDto?> UpdateOrderStatusAsync(OrderUpdateRequest request)
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
        
        order.Status = request.Status;
        var result = await _orderRepository.UpdateOrderAsync(order);
        
        return result == null ? null : StoreEntityToDtoMapper.MapToOrderDto(result);
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