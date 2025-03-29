namespace Forja.API.Controllers.Store;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;
    private readonly ICartService _cartService;
    private readonly IAnalyticsEventService _analyticsEventService;
    private readonly IAuditLogService _auditLogService;

    public OrderController(IOrderService orderService, 
        IUserService userService,
        ICartService cartService,
        IAnalyticsEventService analyticsEventService,
        IAuditLogService auditLogService)
    {
        _orderService = orderService;
        _userService = userService;
        _cartService = cartService;
        _analyticsEventService = analyticsEventService;
        _auditLogService = auditLogService;
    }

    // ------------------- Order Endpoints -------------------

    [Authorize(Policy = "StoreManagePolicy")]
    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
    {
        if (orderId == Guid.Empty) return BadRequest(new { error = "Order ID is required." });
        try
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound(new { error = $"No order found with ID {orderId}." });
            return Ok(order);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Order,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get order by id: {orderId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize(Policy = "StoreManagePolicy")]
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetOrdersByUserId([FromRoute] Guid userId)
    {
        if (userId == Guid.Empty) return BadRequest(new { error = "User ID is required." });
        try
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Order,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get order by user id: {userId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize(Policy = "StoreManagePolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Order,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all orders" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] OrderCreateRequest request)
    {
        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            if (user == null)
            {
                return NotFound(new { error = $"User with Keycloak ID {keycloakUserId} not found." });
            }
            
            var cart = await _cartService.GetCartByIdAsync(request.CartId);
            if (cart == null)
            {
                return NotFound(new { error = $"Cart with ID {request.CartId} not found." });
            }

            if (cart.UserId != user.Id)
            {
                return Unauthorized(new { error = "You are not authorized to create an order for this user." });
            }
                
            var order = await _orderService.AddOrderAsync(request);
            if (order == null) return BadRequest(new { error = "Order creation failed." });
            
            try
            {
                await _analyticsEventService.AddEventAsync(AnalyticEventType.Purchase,
                    user.Id,
                    new Dictionary<string, string>
                    {
                        { "OrderId", order.Id.ToString() },
                        { "CartId", request.CartId.ToString() },
                        { "OrderDate", order.OrderDate.ToString(CultureInfo.InvariantCulture) },
                        { "OrderStatus", order.Status },
                        { "OrderSum", cart.TotalAmount.ToString(CultureInfo.InvariantCulture) }
                    });
            }
            catch (Exception)
            {
                Console.WriteLine("Analytics event creation failed.");
            }
            
            try
            {
                var logEntry = new LogEntry<OrderDto>
                {
                    State = order,
                    UserId = user.Id,
                    Exception = null,
                    ActionType = AuditActionType.Create,
                    EntityType = AuditEntityType.Order,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Order created successfully. Order ID: {order.Id}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Create,
                    EntityType = AuditEntityType.Order,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to add order with cart id: {request.CartId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize(Policy = "StoreManagePolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateOrderStatus([FromBody] OrderUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(request);
            if (updatedOrder == null)
            {
                return NotFound(new { error = $"Order with ID {request.Id} not found." });
            }
            
            try
            {
                var logEntry = new LogEntry<OrderDto>
                {
                    State = updatedOrder,
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Order,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Order updated successfully. Order ID: {updatedOrder.Id}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return Ok(updatedOrder);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Order,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update order with id: {request.Id}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize(Policy = "StoreManagePolicy")]
    [HttpDelete("{orderId:guid}")]
    public async Task<IActionResult> DeleteOrder([FromRoute] Guid orderId)
    {
        try
        {
            await _orderService.DeleteOrderAsync(orderId);
            
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "deleted",
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Delete,
                    EntityType = AuditEntityType.Order,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Order with id: {orderId} - deleted successfully" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Delete,
                    EntityType = AuditEntityType.Order,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete order with id: {orderId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }
}