namespace Forja.API.Controllers.Store;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;
    private readonly ICartService _cartService;

    public OrderController(IOrderService orderService, 
        IUserService userService,
        ICartService cartService)
    {
        _orderService = orderService;
        _userService = userService;
        _cartService = cartService;
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
            return CreatedAtAction(nameof(GetOrderById), new { orderId = order?.Id }, order);
        }
        catch (Exception ex)
        {
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
            return Ok(updatedOrder);
        }
        catch (Exception ex)
        {
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
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}