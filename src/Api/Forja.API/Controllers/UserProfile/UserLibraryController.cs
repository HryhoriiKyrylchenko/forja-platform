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
    private readonly IUserService _userService;

    public UserLibraryController(IUserLibraryService userLibraryService,
        IAuditLogService auditLogService,
        IUserService userService)
    {
        _userLibraryService = userLibraryService;
        _auditLogService = auditLogService;
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserLibraryGameExtendedDto>> GetSelfUserLibrary(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] List<string>? genres = null,
        [FromQuery] List<string>? mechanics = null,
        [FromQuery] List<string>? tags = null,
        [FromQuery] List<string>? matureContents = null,
        [FromQuery] string? search = null)
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
            
            var userLibrary = await _userLibraryService.GetAllUserLibraryGamesByUserIdAsync(user.Id);
            
            var filtered = userLibrary.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(libraryGame =>
                    (!string.IsNullOrEmpty(libraryGame.Game.Title) && libraryGame.Game.Title.Contains(search, StringComparison.OrdinalIgnoreCase)));
            }

            if (genres is { Count: > 0 })
            {
                filtered = filtered.Where(libraryGame => libraryGame.Game.Genres.Any(g => genres.Contains(g.Name)));
            }

            if (mechanics is { Count: > 0 })
            {
                filtered = filtered.Where(libraryGame => libraryGame.Game.Mechanics.Any(m => mechanics.Contains(m.Name)));
            }

            if (tags is { Count: > 0 })
            {
                filtered = filtered.Where(libraryGame => libraryGame.Game.Tags.Any(t => tags.Contains(t.Title)));
            }

            if (matureContents is { Count: > 0 })
            {
                filtered = filtered.Where(libraryGame => libraryGame.Game.MatureContent.Any(mc => matureContents.Contains(mc.Name)));
            }

            var pagedResult = filtered
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            return Ok(pagedResult);
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
                        { "Message", $"Failed to get all user library games by user id" }
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
    [HttpGet("launcher")]
    public async Task<ActionResult<UserLibraryGameExtendedDto>> GetSelfUserGamesForLauncher()
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
            
            var userLibraryForLauncher = await _userLibraryService.GetAllUserLibraryGamesForLauncherByUserIdAsync(user.Id);
            
            return Ok(userLibraryForLauncher);
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
                        { "Message", $"Failed to get all user library games by user id" }
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
    /// Adds a new game to the user's library.
    /// </summary>
    /// <param name="request">The request object containing the details for the game to be added to the user's library.</param>
    /// <returns>
    /// An ActionResult containing the created UserLibraryGameExtendedDto if the operation is successful.
    /// Returns a 200 OK response with the created resource if successful, a 400 BadRequest response if the operation fails or the model-state is invalid,
    /// or in case of an exception, an error message with a 400 BadRequest response.
    /// </returns>
    [Authorize(Policy = "UserManagePolicy")]
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
    /// Updates an existing game in the user's library.
    /// </summary>
    /// <param name="request">The request object containing the updated details of the user library game.</param>
    /// <returns>
    /// An ActionResult containing the updated UserLibraryGameExtendedDto if the operation is successful.
    /// Returns a 200 OK response with the updated resource if successful, a 404 NotFound response if the game is not found,
    /// or a 400 BadRequest response if the operation fails or the model-state is invalid.
    /// </returns>
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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
    [Authorize(Policy = "UserManagePolicy")]
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