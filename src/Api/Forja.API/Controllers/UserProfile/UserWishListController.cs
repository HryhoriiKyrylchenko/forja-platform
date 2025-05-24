namespace Forja.API.Controllers.UserProfile;

/// <summary>
/// Controller for managing UserWishList-related functionality.
/// Exposes UserWishList operations through API endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserWishListController : ControllerBase
{
    private readonly IUserWishListService _userWishListService;
    private readonly IAuditLogService _auditLogService;
    private readonly IUserService _userService;
    private readonly IDistributedCache _cache;

    public UserWishListController(IUserWishListService userWishListService,
        IAuditLogService auditLogService,
        IUserService userService,
        IDistributedCache cache)
    {
        _userWishListService = userWishListService;
        _auditLogService = auditLogService;
        _userService = userService;
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all UserWishList entries.
    /// </summary>
    /// <returns>A list of UserWishListDTO.</returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserWishListDto>>> GetAll()
    {
        try
        {
            var wishLists = await _userWishListService.GetAllAsync();
            return Ok(wishLists);
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
                        { "Message", $"Failed to get all user wish lisls" }
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

    /// <summary>
    /// Retrieves a UserWishList entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the UserWishList entry.</param>
    /// <returns>A UserWishListDTO if found.</returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserWishListDto>> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid ID.");
        }

        try
        {
            var wishList = await _userWishListService.GetByIdAsync(id);
            if (wishList == null)
            {
                return NotFound($"UserWishList entry with ID {id} not found.");
            }

            return Ok(wishList);
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
                        { "Message", $"Failed to get user wish lisl by id: {id}" }
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

    /// <summary>
    /// Adds a new item to the user's wish list.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to be added to the wish list.</param>
    /// <returns>The newly created UserWishListDto containing the details of the added wish list item.</returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UserWishListDto>> Add([FromQuery] Guid productId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var cachedProfile = await _cache.GetStringAsync($"user_profile_{keycloakUserId}");
            UserProfileDto? userProfile;
            if (!string.IsNullOrWhiteSpace(cachedProfile))
            {
                userProfile = JsonSerializer.Deserialize<UserProfileDto>(cachedProfile);
            }
            else
            {
                userProfile = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            }
            
            if (userProfile == null)
            {
                return NotFound(new { error = $"User not found." });
            }
            
            var createdWishList = await _userWishListService.AddAsync(new UserWishListCreateRequest
            {
                UserId = userProfile.Id,
                ProductId = productId
            });
            if (createdWishList == null)
            {
                return BadRequest(new { error = "Failed to create wishlist." });
            }
            return CreatedAtAction(nameof(GetById), new { id = createdWishList.Id }, createdWishList);
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
                        { "Message", $"Failed to add to a wish list product with id: {productId}" }
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

    /// <summary>
    /// Updates an existing UserWishList entry.
    /// </summary>
    /// <param name="request">The UserWishListUpdateRequest containing updated user and product IDs.</param>
    /// <returns>No content if update is successful.</returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut]
    public async Task<ActionResult<UserWishListDto>> Update([FromBody] UserWishListUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _userWishListService.UpdateAsync(request);
            if (result == null)
            {
                return BadRequest(new { error = "Failed to update wishlist." });
            }
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
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
                        { "Message", $"Failed to update user wish lisl with id: {request.Id}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a UserWishList entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entry to delete.</param>
    /// <returns>No content if deletion is successful.</returns>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid ID.");
        }

        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var cachedProfile = await _cache.GetStringAsync($"user_profile_{keycloakUserId}");
            UserProfileDto? userProfile;
            if (!string.IsNullOrWhiteSpace(cachedProfile))
            {
                userProfile = JsonSerializer.Deserialize<UserProfileDto>(cachedProfile);
            }
            else
            {
                userProfile = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            }
            
            if (userProfile == null)
            {
                return NotFound(new { error = $"User not found." });
            }
            
            var wishList = await _userWishListService.GetByIdAsync(id);
            if (wishList == null)
            {
                return NotFound(new { error = $"UserWishList entry with ID {id} not found." });
            }

            if (wishList.UserId != userProfile.Id)
            {
                return Unauthorized(new { error = $"User does not have permission to delete this wish list." });
            }
            
            await _userWishListService.DeleteAsync(id);
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
                        { "Message", $"Failed to delete user wish lisl with id: {id}" }
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

    /// <summary>
    /// Retrieves all UserWishList entries for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of UserWishListDTO entries associated with the user.</returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<UserWishListDto>>> GetByUserId(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("Invalid ID.");
        }

        try
        {
            var wishLists = await _userWishListService.GetByUserIdAsync(userId);
            return Ok(wishLists);
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
                        { "Message", $"Failed to get user wish lisl by user id: {userId}" }
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
    [HttpGet("self")]
    public async Task<ActionResult<IEnumerable<UserWishListDto>>> GetAllSelfWishLists()
    {
        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var cachedProfile = await _cache.GetStringAsync($"user_profile_{keycloakUserId}");
            UserProfileDto? userProfile;
            if (!string.IsNullOrWhiteSpace(cachedProfile))
            {
                userProfile = JsonSerializer.Deserialize<UserProfileDto>(cachedProfile);
            }
            else
            {
                userProfile = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            }
            
            if (userProfile == null)
            {
                return NotFound(new { error = $"User not found." });
            }
            
            var wishLists = await _userWishListService.GetByUserIdWithExtendedGameAsync(userProfile.Id);
            return Ok(wishLists);
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
                        { "Message", $"Failed to get user self wish lisl for user" }
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