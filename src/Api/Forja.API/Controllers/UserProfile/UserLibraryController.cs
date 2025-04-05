namespace Forja.API.Controllers.UserProfile;

/// <summary>
/// Provides endpoints to manage user library content such as games and addons.
/// Includes operations for adding, updating, retrieving, deleting, and restoring user library items.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserLibraryController : ControllerBase
{
    private readonly IUserLibraryService _userLibraryService;
    private readonly IAuditLogService _auditLogService;

    public UserLibraryController(IUserLibraryService userLibraryService,
        IAuditLogService auditLogService)
    {
        _userLibraryService = userLibraryService;
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Adds a new game to the user's library.
    /// </summary>
    /// <param name="request">The request object containing the user ID and game ID for the new library game.</param>
    /// <returns>
    /// An IActionResult indicating the result of the operation.
    /// Returns a 200 OK response with the created resource if successful, or a 400 BadRequest response if the operation fails or the model-state is invalid.
    /// </returns>
    [HttpPost("game")]
    public async Task<ActionResult<UserLibraryGameExtendedDto>> AddUserLibraryGame([FromBody] UserLibraryGameCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _userLibraryService.AddUserLibraryGameAsync(request);
            return result != null ? Ok(result) : BadRequest(new { error = "Failed to add user library game." });
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to add user library game for dame with id: {request.GameId} for user with id: {request.UserId}." }
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
    /// Updates a game entry in the user's library with the specified details.
    /// </summary>
    /// <param name="request">The details required to update a user library game, including the game ID, user ID, and purchase date.</param>
    /// <returns>
    /// Returns an IActionResult indicating the result of the update operation.
    /// If the operation is successful, returns an "Ok" response with the updated game details.
    /// If the game is not found, returns a "NotFound" response.
    /// If the operation fails due to an error, returns a "BadRequest" response with the error details.
    /// </returns>
    [HttpPut("game")]
    public async Task<ActionResult<UserLibraryGameExtendedDto>> UpdateUserLibraryGame([FromBody] UserLibraryGameUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _userLibraryService.UpdateUserLibraryGameAsync(request);
            return result != null ? Ok(result) : NotFound(new { error = "User library game not found." });
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update user library game with id: {request.Id}" }
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
    /// Deletes a game from the user's library based on the provided ID.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the user's library game to be deleted.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation, such as success or error messages.</returns>
    [HttpDelete("game/{userLibraryGameId}")]
    public async Task<IActionResult> DeleteUserLibraryGame([FromRoute] Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            await _userLibraryService.DeleteUserLibraryGameAsync(userLibraryGameId);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete user library game with id: {userLibraryGameId}" }
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
    /// Restores a previously deleted game in the user's library.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the game to be restored.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the restored game details if successful,
    /// or an appropriate error message if the restoration fails or the game is not found.
    /// </returns>
    [HttpPut("game/restore/{userLibraryGameId}")]
    public async Task<ActionResult<UserLibraryGameExtendedDto>> RestoreUserLibraryGame([FromRoute] Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _userLibraryService.RestoreUserLibraryGameAsync(userLibraryGameId);
            return result != null ? Ok(result) : NotFound(new { error = "User library game not found." });
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to restore user library game with id: {userLibraryGameId}" }
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
    /// Retrieves a specific user library game by its unique identifier.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the user library game to retrieve.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the details of the user library game if found
    /// </returns>
    [HttpGet("game/{userLibraryGameId}")]
    public async Task<ActionResult<UserLibraryGameExtendedDto>> GetUserLibraryGameById([FromRoute] Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _userLibraryService.GetUserLibraryGameByIdAsync(userLibraryGameId);
            return result != null ? Ok(result) : NotFound(new { error = "User library game not found." });
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get user library game by id: {userLibraryGameId}" }
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
    /// Retrieves a deleted game from the user's library by its unique identifier.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the deleted user library game.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the deleted game details if found, or an error response if the game is not found or if the request is invalid.
    /// </returns>
    [HttpGet("game/deleted/{userLibraryGameId}")]
    public async Task<ActionResult<UserLibraryGameExtendedDto>> GetDeletedUserLibraryGameById([FromRoute] Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _userLibraryService.GetDeletedUserLibraryGameByIdAsync(userLibraryGameId);
            return result != null ? Ok(result) : NotFound(new { error = "Deleted user library game not found." });
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get deleted user library game by id: {userLibraryGameId}" }
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
    /// Retrieves the list of all user library games.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains a collection of user library games.</returns>
    [HttpGet("games")]
    public async Task<ActionResult<List<UserLibraryGameExtendedDto>>> GetAllUserLibraryGames()
    {
        try
        {
            var result = await _userLibraryService.GetAllUserLibraryGamesAsync();
            return Ok(result);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all user library games" }
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
    /// Retrieves a list of all deleted games in the user library.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing a list of deleted user library games.</returns>
    [HttpGet("games/deleted")]
    public async Task<ActionResult<List<UserLibraryGameExtendedDto>>> GetAllDeletedUserLibraryGames()
    {
        try
        {
            var result = await _userLibraryService.GetAllDeletedUserLibraryGamesAsync();
            return Ok(result);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all deleted user library games" }
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
    /// Retrieves all library games associated with a specific user by their user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose library games are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, containing the HTTP response with the list of user library games.</returns>
    [HttpGet("games/user/{userId}")]
    public async Task<ActionResult<List<UserLibraryGameExtendedDto>>> GetAllUserLibraryGamesByUserId(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid user ID." });
        }

        try
        {
            var result = await _userLibraryService.GetAllUserLibraryGamesByUserIdAsync(userId);
            return Ok(result);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all user library games by user id: {userId}" }
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
    /// Retrieves all deleted games from a user's library based on the specified user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose deleted games are to be retrieved.</param>
    /// <returns>An IActionResult containing a list of deleted games associated with the specified user ID or an appropriate error message.</returns>
    [HttpGet("games/deleted/user/{userId}")]
    public async Task<ActionResult<List<UserLibraryGameExtendedDto>>> GetAllDeletedUserLibraryGamesByUserId(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid user ID." });
        }
        try
        {
            var result = await _userLibraryService.GetAllDeletedUserLibraryGamesByUserIdAsync(userId);
            return Ok(result);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all deleted user library games by user id: {userId}" }
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
    /// Adds a new addon to the user library associated with a specific game.
    /// </summary>
    /// <param name="request">The details of the addon to be added, including the associated game ID and addon ID.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the result of the operation.
    /// Returns an HTTP 200 OK response with the added addon details if successful, or an HTTP 400 Bad Request with an error message if the operation fails.
    /// </returns>
    [HttpPost("addon")]
    public async Task<ActionResult<UserLibraryAddonDto>> AddUserLibraryAddon([FromBody] UserLibraryAddonCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _userLibraryService.AddUserLibraryAddonAsync(request);
            return result != null ? Ok(result) : BadRequest(new { error = "Failed to add user library addon." });
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to add user library addon for game with is: {request.UserLibraryGameId} and addon with id: {request.AddonId}" }
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
    /// Updates a specific user library addon with the provided details.
    /// </summary>
    /// <param name="request">The update request containing the addon information such as Id, UserLibraryGameId, AddonId, and PurchaseDate.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> that represents the result of the operation.
    /// The response is an updated user library addon object if successful, otherwise a NotFound or BadRequest result is returned.
    /// </returns>
    [HttpPut("addon")]
    public async Task<ActionResult<UserLibraryAddonDto>> UpdateUserLibraryAddon([FromBody] UserLibraryAddonUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _userLibraryService.UpdateUserLibraryAddonAsync(request);
            return result != null ? Ok(result) : NotFound(new { error = "User library addon not found." });
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update user library addon with is: {request.Id}" }
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
    /// Deletes a specified user library addon by its unique identifier.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the user library addon to delete.</param>
    /// <returns>An IActionResult indicating the result of the operation. Returns NoContent if successful,
    /// or BadRequest if an error occurs or the provided ID is invalid.</returns>
    [HttpDelete("addon/{userLibraryAddonId}")]
    public async Task<IActionResult> DeleteUserLibraryAddon([FromRoute] Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            await _userLibraryService.DeleteUserLibraryAddonAsync(userLibraryAddonId);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete user library addon with is: {userLibraryAddonId}" }
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
    /// Restores a previously deleted user library addon by its unique identifier.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the user library addon to restore.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the restored user library addon if found and restored successfully,
    /// a <c>NotFound</c> result if the addon does not exist, or a <c>BadRequest</c> result if an error occurs.
    /// </returns>
    [HttpPut("addon/restore/{userLibraryAddonId}")]
    public async Task<ActionResult<UserLibraryAddonDto>> RestoreUserLibraryAddon([FromRoute] Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _userLibraryService.RestoreUserLibraryAddonAsync(userLibraryAddonId);
            return result != null ? Ok(result) : NotFound(new { error = "User library addon not found." });
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to restore user library addon with is: {userLibraryAddonId}" }
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
    /// Retrieves a specific user library addon by its unique identifier.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the user library addon to retrieve.</param>
    /// <returns>An IActionResult containing the user library addon data if found, or an error response if the addon is not found or the identifier is invalid.</returns>
    [HttpGet("addon/{userLibraryAddonId}")]
    public async Task<ActionResult<UserLibraryAddonDto>> GetUserLibraryAddonById([FromRoute] Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _userLibraryService.GetUserLibraryAddonByIdAsync(userLibraryAddonId);
            return result != null ? Ok(result) : NotFound(new { error = "User library addon not found." });
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get user library addon by is: {userLibraryAddonId}" }
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
    /// Retrieves a deleted user library add-on by its unique identifier.
    /// </summary>
    /// <param name="userLibraryAddonId">The unique identifier of the deleted user library add-on to retrieve.</param>
    /// <returns>
    /// Returns an <see cref="IActionResult"/> containing the details of the deleted user library add-on
    /// if found; otherwise, returns a NotFound or BadRequest result.
    /// </returns>
    [HttpGet("addon/deleted/{userLibraryAddonId}")]
    public async Task<ActionResult<UserLibraryAddonDto>> GetDeletedUserLibraryAddonById([FromRoute] Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _userLibraryService.GetDeletedUserLibraryAddonByIdAsync(userLibraryAddonId);
            return result != null ? Ok(result) : NotFound(new { error = "Deleted user library addon not found." });
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get deleted user library addon by is: {userLibraryAddonId}" }
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
    /// Retrieves all user library addons from the system.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of user library addons.</returns>
    [HttpGet("addons")]
    public async Task<ActionResult<List<UserLibraryAddonDto>>> GetAllUserLibraryAddons()
    {
        try
        {
            var result = await _userLibraryService.GetAllUserLibraryAddonsAsync();
            return Ok(result);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all user library addons" }
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
    /// Retrieves all deleted user library addons from the system.
    /// This method interacts with the UserLibraryService to return a list of addons
    /// that have been soft-deleted and are no longer active in the user's library.
    /// </summary>
    /// <returns>
    /// An IActionResult containing either a list of deleted user library addon DTOs
    /// or a BadRequest with an error message in case of an exception.
    /// </returns>
    [HttpGet("addons/deleted")]
   public async Task<ActionResult<List<UserLibraryAddonDto>>> GetAllDeletedUserLibraryAddons()
    {
        try
        {
            var result = await _userLibraryService.GetAllDeletedUserLibraryAddonsAsync();
            return Ok(result);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all deleted user library addons" }
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
    /// Retrieves all user library addons associated with a specific game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game whose addons are to be retrieved.</param>
    /// <returns>An <see cref="IActionResult"/> containing a list of addons or an error message if the retrieval fails.</returns>
    [HttpGet("addons/game/{gameId}")]
    public async Task<ActionResult<List<UserLibraryAddonDto>>> GetAllUserLibraryAddonsByGameId([FromRoute] Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid Game ID." });
        }

        try
        {
            var result = await _userLibraryService.GetAllUserLibraryAddonsByGameIdAsync(gameId);
            return Ok(result);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all user library addons by game id: {gameId}" }
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
    /// Retrieves all deleted addons associated with a specific game from the user's library.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game to retrieve deleted addons for.</param>
    /// <returns>A list of deleted user library addons associated with the specified game ID.</returns>
    [HttpGet("addons/deleted/game/{gameId}")]
    public async Task<ActionResult<List<UserLibraryAddonDto>>> GetAllDeletedUserLibraryAddonsByGameId([FromRoute] Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid Game ID." });
        }

        try
        {
            var result = await _userLibraryService.GetAllDeletedUserLibraryAddonsByGameIdAsync(gameId);
            return Ok(result);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all deleted user library addons by game id: {gameId}" }
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