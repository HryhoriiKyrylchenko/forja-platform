namespace Forja.API.Controllers.Store;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IUserService _userService;
    private readonly IAuditLogService _auditLogService;

    public CartController(ICartService cartService,
        IUserService userService,
        IAuditLogService auditLogService)
    {
        _cartService = cartService;
        _userService = userService;
        _auditLogService = auditLogService;
    }

    [Authorize(Policy = "StoreManagePolicy")]
    [HttpGet("{cartId:guid}")]
    public async Task<IActionResult> GetCartById([FromRoute] Guid cartId)
    {
        if (cartId == Guid.Empty) return BadRequest(new { error = "Cart ID is required." });
        try
        {
            var cart = await _cartService.GetCartByIdAsync(cartId);
            if (cart == null) return NotFound(new { error = $"No cart found with ID {cartId}."});
            return Ok(cart);
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get cart by id: {cartId}" }
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
    public async Task<IActionResult> GetCartsByUserId([FromRoute] Guid userId)
    {
        if (userId == Guid.Empty) return BadRequest(new { error = "User ID is required." });
        try
        {
            var carts = await _cartService.GetCartsByUserIdAsync(userId);
            return Ok(carts);
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get cart by user id: {userId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest( new { error = ex.Message } );
        }
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetSelfActiveCart()
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
            
            var cart = await _cartService.GetOrCreateActiveCartAsync(new CartCreateRequest
            {
                UserId = user.Id
            });
            return Ok(cart);
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get self cart" }
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
    [HttpDelete("{cartId:guid}")]
    public async Task<IActionResult> RemoveCart([FromRoute] Guid cartId)
    {
        try
        {
            await _cartService.RemoveCartAsync(cartId);
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to remove cart with id: {cartId}" }
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
    
    // [Authorize]
    [HttpGet("indicator")]
    public async Task<IActionResult> GetCartIndicator()
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
    
            var allCarts = await _cartService.GetCartsByUserIdAsync(user.Id);

            var activeCart = allCarts
                .FirstOrDefault(c => string.Equals(c.Status, "Active", StringComparison.OrdinalIgnoreCase));
            
            int totalItems = 0; 
            decimal totalPrice = 0;
            
            if (activeCart != null)
            {
                totalItems = activeCart.CartItems.Count();
                totalPrice = activeCart.TotalAmount;
                
                return Ok(new
                {
                    hasItems = totalItems > 0,
                    totalItems,
                    totalPrice
                });
            }

            return Ok(new
            {
                hasItems = false,
                totalItems = 0,
                totalPrice = 0
            });
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get cart indicator" }
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
    [HttpPost("handle-abandoned")]
    public async Task<IActionResult> HandleAbandonedCarts([FromQuery] TimeSpan inactivityPeriod)
    {
        try
        {
            await _cartService.HandleAbandonedCartsAsync(inactivityPeriod);
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to handle abandoned carts" }
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
    [HttpPost("recover")]
    public async Task<IActionResult> RecoverAbandonedCart()
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
            
            var cart = await _cartService.RecoverAbandonedCartAsync(user.Id);
            if (cart == null) return NotFound(new { error = $"No abandoned cart for user {user.Id}." });
            return Ok(cart);
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to recover abandoned carts" }
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


    [HttpGet("item/{cartItemId:guid}")]
    public async Task<IActionResult> GetCartItemById([FromRoute] Guid cartItemId)
    {
        if (cartItemId == Guid.Empty) return BadRequest(new { error = "Cart item ID is required." });
        try
        {
            var item = await _cartService.GetCartItemByIdAsync(cartItemId);
            if (item == null) return NotFound(new { error = $"No cart item found with ID {cartItemId}." });
            return Ok(item);
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed get cart item by id: {cartItemId}" }
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
    
    [HttpGet("{cartId:guid}/items")]
    public async Task<IActionResult> GetCartItems([FromRoute] Guid cartId)
    {
        if (cartId == Guid.Empty) return BadRequest(new { error = "Cart ID is required." });
        try
        {
            var items = await _cartService.GetCartItemsByCartIdAsync(cartId);
            return Ok(items);
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed get cart items" }
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

    [HttpPost("items")]
    public async Task<IActionResult> AddCartItem([FromBody] CartItemCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var item = await _cartService.AddCartItemAsync(request);
            if (item == null) return NotFound(new { error = $"Cart item was not added for product with ID {request.ProductId}." });
            return Ok(item);
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed add cart item with product id: {request.ProductId}" }
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
    
    [HttpPost("bundle")]
    public async Task<IActionResult> AddBundleToCartItems([FromBody] CartAddBundleRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var items = await _cartService.AddBundleToCartAsync(request);
            if (items.Count == 0) return NotFound(new { error = $"No items found for bundle with ID {request.BundleId}." });
            return Ok(items);
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed add bundle with id {request.BundleId} - to cart items" }
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
    
    [HttpDelete("items/{itemId:guid}")]
    public async Task<IActionResult> RemoveCartItem([FromRoute] Guid itemId)
    {
        if (itemId == Guid.Empty) return BadRequest(new { error = "Cart item ID is required." });
        try
        {
            var cart = await _cartService.RemoveCartItemAsync(itemId);
            return Ok(cart);
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
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed remove cart item with id: {itemId}" }
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