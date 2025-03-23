namespace Forja.API.Controllers.Store;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IUserService _userService;

    public CartController(ICartService cartService,
        IUserService userService)
    {
        _cartService = cartService;
        _userService = userService;
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
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("/recover")]
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
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpGet("/item/{cartItemId:guid}")]
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
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("/items")]
    public async Task<IActionResult> AddCartItem([FromBody] CartItemCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var item = await _cartService.AddCartItemAsync(request);
            return Ok(item);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPut("/items")]
    public async Task<IActionResult> UpdateCartItem([FromBody] CartItemUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var item = await _cartService.UpdateCartItemAsync(request);
            return Ok(item);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpDelete("/items/{itemId:guid}")]
    public async Task<IActionResult> RemoveCartItem([FromRoute] Guid itemId)
    {
        if (itemId == Guid.Empty) return BadRequest(new { error = "Cart item ID is required." });
        try
        {
            await _cartService.RemoveCartItemAsync(itemId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}