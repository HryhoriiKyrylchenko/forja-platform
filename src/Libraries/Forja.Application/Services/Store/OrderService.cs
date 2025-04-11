namespace Forja.Application.Services.Store;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IUserLibraryService _userLibraryService;

    public OrderService(IOrderRepository orderRepository, 
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository,
        IUserLibraryService userLibraryService)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _userLibraryService = userLibraryService;
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

        if (order.Status == request.Status)
        {
            return StoreEntityToDtoMapper.MapToOrderDto(order);
        }
        
        order.Status = request.Status;
        var updatedOrder = await _orderRepository.UpdateOrderAsync(order);
        if (updatedOrder == null)
        {
            throw new InvalidOperationException("Failed to update order status.");
        }
        var cartItems = order.Cart.CartItems;

        switch (updatedOrder.Status)
        {
            case OrderStatus.Completed:
                List<CartItem> gameCartItems = [];
                List<CartItem> addonCartItems = [];

                foreach (var cartItem in cartItems)
                {
                    if (cartItem.Product is Game)
                    {
                        gameCartItems.Add(cartItem);
                    }
                    else if (cartItem.Product is GameAddon)
                    {
                        addonCartItems.Add(cartItem);
                    }
                }
                
                foreach (var cartItem in gameCartItems)
                {
                    var userGame = await _userLibraryService.AddUserLibraryGameAsync(new UserLibraryGameCreateRequest
                    {
                        GameId = cartItem.ProductId,
                        UserId = order.Cart.UserId
                    });
                    if (userGame == null)
                    {
                        throw new InvalidOperationException("Failed to add user game.");
                    }
                }

                foreach (var cartItem in addonCartItems)
                {
                    var addon = (GameAddon)cartItem.Product;
                    var userLibraryGame = await _userLibraryService.GetUserLibraryGameByGameIdAsync(addon.GameId);
                    if (userLibraryGame == null)
                    {
                        throw new InvalidOperationException("Failed to get user library game.");
                    }
                    var userAddon = await _userLibraryService.AddUserLibraryAddonAsync(new UserLibraryAddonCreateRequest
                    {
                        AddonId = cartItem.ProductId,
                        UserLibraryGameId = userLibraryGame.Id
                    });
                    if (userAddon == null)
                    {
                        throw new InvalidOperationException("Failed to add user addon.");
                    }
                }
                break;
            case OrderStatus.Canceled:
                foreach (var cartItem in cartItems)
                {
                    if (cartItem.Product is Game)
                    {
                        var userGame = await _userLibraryService.GetUserLibraryGameByGameIdAsync(cartItem.ProductId);
                        if (userGame == null)
                        {
                            throw new InvalidOperationException("Failed to get user library game.");
                        }
                        await _userLibraryService.DeleteUserLibraryGameAsync(userGame.Id);
                    }
                    else if (cartItem.Product is GameAddon)
                    {
                        var userAddon = await _userLibraryService.GetUserLibraryAddonByAddonIdAsync(cartItem.ProductId);
                        if (userAddon == null)
                        {
                            throw new InvalidOperationException("Failed to get user library addon.");
                        }
                        await _userLibraryService.DeleteUserLibraryAddonAsync(cartItem.ProductId);
                    }
                }
                break;
        }
        
        return StoreEntityToDtoMapper.MapToOrderDto(updatedOrder);
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