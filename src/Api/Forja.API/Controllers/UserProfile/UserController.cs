
using Forja.Application.Interfaces.UserProfile;
using Forja.Application.Services.UserProfile;
using System.Security.Claims;

namespace Forja.API.Controllers.UserProfile;

/// <summary>
/// Provides API endpoints for managing user profiles and operations related to users.
/// </summary>
/// <remarks>
/// This controller handles user-related operations including retrieving user profiles, updating user profiles,
/// deleting users, retrieving all users, and retrieving deleted users. It communicates with the
/// <see cref="IUserService"/> for business logic and <see cref="IKeycloakClient"/> for operations
/// related to Keycloak.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IKeycloakClient _keycloakClient;
    private readonly IUserFollowerService _userFollowerService;
    private readonly IUserWishListService _userWishListService;
    private readonly IUserLibraryService _userLibraryService;
    private readonly IAuditLogService _auditLogService;
    private readonly IDistributedCache _cache;

    public UserController(IUserService userService, 
        IKeycloakClient keycloakClient,
        IAuditLogService auditLogService,
        IDistributedCache cache)
    {
        _userService = userService;
        _keycloakClient = keycloakClient;
        _userFollowerService = userFollowerService;
        _userWishListService = userWishListService;
        _userLibraryService = userLibraryService;
        _auditLogService = auditLogService;
        _cache = cache;
    }


    /// <summary>
    /// Retrieves a user profile by the specified user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing the user's profile data as a <see cref="UserProfileDto"/> if found,
    /// or an appropriate error response if the user ID is invalid or the user does not exist.</returns>
    [Authorize]
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserProfileDto>> GetUserById([FromRoute] Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { error = $"User with ID {userId} not found." });
            }

            return Ok(_userService.HidePersonalData(user));
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get user by id: {userId}." }
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
    /// Retrieves a deleted user's profile by their unique user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the deleted user to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an <see cref="ActionResult{T}"/>
    /// of type <see cref="UserProfileDto"/>.
    /// Returns a 200 OK response with the deleted user's profile data if found.
    /// Returns a 404 Not Found response if the deleted user cannot be located.
    /// Returns a 400 Bad Request response if the specified ID is invalid or an error occurs during the operation.
    /// </returns>
    [Authorize(Policy = "UserViewPolicy")]
    [HttpGet("deleted/{userId}")]
    public async Task<ActionResult<UserProfileDto>> GetDeletedUserById([FromRoute] Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var user = await _userService.GetDeletedUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { error = $"Deleted user with ID {userId} not found." });
            }

            return Ok(user);
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get deleted user by id: {userId}." }
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

    /// Retrieves a user profile by the specified Keycloak ID.
    /// <param name="keycloakId">The Keycloak ID of the user to be retrieved.</param>
    /// <returns>
    /// An `ActionResult` containing the `UserProfileDto` if the user is found;
    /// otherwise, an error response indicating the issue (e.g., user not found or bad request).
    /// </returns>
    [Authorize]
    [HttpGet("profile/{keycloakId}")]
    public async Task<ActionResult<UserProfileDto>> GetUserByKeycloakId([FromRoute] string keycloakId)
    {
        if (string.IsNullOrWhiteSpace(keycloakId))
        {
            return BadRequest(new { error = "Keycloak ID is required." });
        }

        try
        {
            var user = await _userService.GetUserByKeycloakIdAsync(keycloakId);
            if (user == null)
            {
                return NotFound(new { error = $"User with Keycloak ID {keycloakId} not found." });
            }

            return Ok(_userService.HidePersonalData(user));
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get user by keycloak id: {keycloakId}." }
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
    /// Retrieves a deleted user profile based on a specified Keycloak ID.
    /// </summary>
    /// <param name="keycloakId">The Keycloak ID of the user to retrieve.</param>
    /// <returns>Returns an <see cref="UserProfileDto"/> representing the user's profile
    /// if the user is found and has been deleted, or a suitable error response
    /// in case of bad request or if the user is not found.</returns>
    [Authorize(Policy = "UserViewPolicy")]
    [HttpGet("deleted/profile/{keycloakId}")]
    public async Task<ActionResult<UserProfileDto>> GetDeletedUserByKeycloakId([FromRoute] string keycloakId)
    {
        if (string.IsNullOrWhiteSpace(keycloakId))
        {
            return BadRequest(new { error = "Keycloak ID is required." });
        }

        try
        {
            var user = await _userService.GetDeletedUserByKeycloakIdAsync(keycloakId);
            if (user == null)
            {
                return NotFound(new { error = $"User with Keycloak ID {keycloakId} not found." });
            }

            return Ok(user);
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get deleted user by keycloak id: {keycloakId}." }
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
    /// Retrieves a user profile by their email address.
    /// </summary>
    /// <param name="email">Email address of the user whose profile is to be retrieved.</param>
    /// <returns>A user profile represented by <see cref="UserProfileDto"/> if the user is found.
    /// Returns a 400 Bad Request if the provided email is invalid or an error occurs.
    /// Returns a 404 Not Found if the user is not found.</returns>
    [Authorize]
    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserProfileDto>> GetUserByEmail([FromRoute] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new { error = "Email is required." });
        }

        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { error = $"User with email {email} not found." });
            }

            return Ok(_userService.HidePersonalData(user));
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get user by email: {email}." }
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
    /// Retrieves a deleted user's profile based on their email address.
    /// </summary>
    /// <param name="email">The email address of the deleted user to retrieve.</param>
    /// <returns>Returns an <see cref="ActionResult"/> containing the <see cref="UserProfileDto"/> of the deleted user
    /// if found. Returns a 404 status if the user is not found, or a 400 status for invalid input or errors.</returns>
    [Authorize(Policy = "UserViewPolicy")]
    [HttpGet("deleted/email/{email}")]
    public async Task<ActionResult<UserProfileDto>> GetDeletedUserByEmail([FromRoute] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new { error = "Email is required." });
        }

        try
        {
            var user = await _userService.GetDeletedUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { error = $"Deleted user with email {email} not found." });
            }

            return Ok(user);
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get deleted user by email: {email}." }
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
    /// Retrieves the profile of the currently authenticated user based on the user's Keycloak token.
    /// </summary>
    /// <remarks>
    /// This method requires the user to be authenticated. If the access token is invalid, missing,
    /// or the corresponding user cannot be found, an appropriate error response is returned.
    /// </remarks>
    /// <returns>
    /// An HTTP response containing the <see cref="UserProfileDto"/> of the authenticated user if found.
    /// Returns Unauthorized if the token is invalid or missing, NotFound if the user does not exist,
    /// and BadRequest for any other errors.
    /// </returns>
    [Authorize]
    [HttpGet("self-profile")]
    public async Task<ActionResult<UserProfileDto>> GetSelfUserProfile()
    {
        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var cachedProfile = await _cache.GetStringAsync($"user_profile_{keycloakUserId}");
            if (!string.IsNullOrWhiteSpace(cachedProfile))
            {
                var userProfile = JsonSerializer.Deserialize<UserProfileDto>(cachedProfile);
                return Ok(userProfile);
            }

            var result = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            if (result == null)
            {
                return NotFound(new { error = $"User with Keycloak ID {keycloakUserId} not found." });
            }
                
            var serializedData = JsonSerializer.Serialize(result);

            await _cache.SetStringAsync($"user_profile_{keycloakUserId}", serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get self user profile." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, new { error = "An internal error occurred.", details = ex.Message });
        }
    }

    [HttpGet("debug-token")]
    [Obsolete("This method is deprecated and will be removed in a future release.", false)]
    public IActionResult DebugToken()
    {
        if (Request.Cookies.TryGetValue("access_token", out var token))
        {
            return Ok(new { message = "Token is present in cookies", accessToken = token });
        }

        return Unauthorized(new { error = "No access token found in cookies" });
    }


    /// <summary>
    /// Retrieves the user profile for a specific user ID for manager-level access.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to retrieve the profile for.</param>
    /// <returns>
    /// An <see cref="ActionResult"/> containing the <see cref="UserProfileDto"/> object of the user if found,
    /// or a NotFound result if the user does not exist, or an error if the input is invalid or another issue occurs.
    /// </returns>
    [Authorize(Policy = "UserViewPolicy")]
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<UserProfileDto>> GetUserByIdForManager([FromRoute] Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _userService.GetUserByIdAsync(userId);
            if (result == null)
            {
                return NotFound(new { error = $"User with ID {userId} not found." });
            }
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get user by id (for manager): {userId}." }
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
    /// Adds a new user to the system based on the provided user creation request data.
    /// </summary>
    /// <param name="request">
    /// The request containing the details for creating a new user. This includes all necessary user
    /// information required for registration.
    /// </param>
    /// <returns>
    /// Returns the created user's profile details as a <c>UserProfileDto</c> if the operation is successful.
    /// If the creation fails, an error response is returned.
    /// </returns>
    [Authorize(Policy = "UserManagePolicy")]
    [HttpPost]
    public async Task<ActionResult<UserProfileDto>> AddUser([FromBody] UserCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var user = await _userService.AddUserAsync(request);
            if (user == null)
            {
                return BadRequest(new { error = "Unable to create user." });
            }

            try
            {
                var logEntry = new LogEntry<UserProfileDto>
                {
                    State = user,
                    UserId = user.Id,
                    Exception = null,
                    ActionType = AuditActionType.Create,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"User with email: {request.Email} added successfully." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }

            return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, user);
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to add user with email: {request.Email}." }
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
    /// Updates the profile information of the authenticated user.
    /// </summary>
    /// <param name="request">The user profile update request containing the updated user information.</param>
    /// <returns>An <see cref="ActionResult"/> representing the result of the update operation.
    /// If successful, returns a status code indicating no content. Otherwise, returns an error response.</returns>
    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult<UserProfileDto>> UpdateUserProfile([FromBody] UserUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var accessToken = GetAccessTokenFromRequest();

            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized(new { error = "Authorization header is missing or empty." });
            }

            var keycloakUserId = _keycloakClient.GetKeycloakUserId(accessToken);

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "Unable to retrieve user information from token." });
            }

            var user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            if (user == null)
            {
                return NotFound(new { error = $"User with Keycloak ID {keycloakUserId} not found." });
            }

            if (user.Id != request.Id)
            {
                return Unauthorized(new { error = "You are not authorized to update this user's profile." });
            }

            var updatedUser = await _userService.UpdateUserAsync(request);
            if (updatedUser == null)
            {
                return BadRequest(new { error = "Unable to update user." });
            }

            try
            {
                var logEntry = new LogEntry<UserProfileDto>
                {
                    State = updatedUser,
                    UserId = updatedUser.Id,
                    Exception = null,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"User with id: {request.Id} updated successfully." }
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
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update user profile for user with id: {request.Id}." }
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

    /// Updates a user profile with the specified details for a manager.
    /// <param name="request">An instance of UserUpdateRequest containing the updated user data.</param>
    /// <returns>Returns an IActionResult indicating the outcome of the update operation.</returns>
    [Authorize(Policy = "UserManagePolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateUserProfileForManager([FromBody] UserUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedUser = await _userService.UpdateUserAsync(request);
            if (updatedUser == null)
            {
                return BadRequest(new { error = "Unable to update user." });
            }

            try
            {
                var logEntry = new LogEntry<UserProfileDto>
                {
                    State = updatedUser,
                    UserId = updatedUser.Id,
                    Exception = null,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"User with id: {request.Id} updated successfully." }
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
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update user profile for user with id (for manager): {request.Id}." }
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
    /// Updates the user's profile hat variant file based on the provided request.
    /// </summary>
    /// <param name="request">The request containing details of the user and the new profile hat variant to be updated.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation, which may include success, validation errors,
    /// or authorization errors.</returns>
    [Authorize]
    [HttpPost("profile-hat-variant")]
    public async Task<IActionResult> UpdateUserProfileHatVariantFile([FromBody] UserUpdateProfileHatVariantRequest request)
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

            var user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            if (user == null)
            {
                return NotFound(new { error = $"User not found." });
            }

            if (user.Id != request.UserId)
            {
                return Unauthorized(new { error = "You are not authorized to update this user's profile." });
            }

            UserProfileDto? userProfile = null;
            if (user.ProfileHatVariant != request.Variant)
            {
                await _userService.UpdateProfileHatVariant(request);
            }

            return userProfile == null ? BadRequest(new { error = "Failed to update user profile hat variant." }) : Ok(userProfile);
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update user profile hat variant for user with id: {request.UserId}." }
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

    /// Deletes the user profile associated with the currently authenticated user.
    /// This method retrieves the access token from the request, validates it,
    /// and fetches the associated Keycloak user ID. If the Keycloak user ID
    /// is retrieved successfully, the corresponding user is deleted by making
    /// a service call. If the operation is successful, a 204 No Content response
    /// is returned. Otherwise, appropriate error responses are returned.
    /// <return>Returns a 204 No Content response if the user is successfully deleted.
    /// Returns 401 Unauthorized if the access token is missing or invalid.
    /// Returns 400 Bad Request if an error occurs during the operation.</return>
    [Authorize]
    [HttpDelete("profile")]
    public async Task<IActionResult> DeleteUser()
    {
        try
        {
            var accessToken = GetAccessTokenFromRequest();

            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized(new { error = "Authorization header is missing or empty." });
            }

            var keycloakUserId = _keycloakClient.GetKeycloakUserId(accessToken);

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "Unable to retrieve user information from token." });
            }

            await _userService.DeleteUserAsync(keycloakUserId);

            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "deleted",
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Delete,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"User with keycloak id: {keycloakUserId} deleted successfully." }
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete self user profile." }
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
    /// Deletes a user identified by the provided Keycloak ID.
    /// </summary>
    /// <param name="keycloakId">The Keycloak ID of the user to be deleted.</param>
    /// <returns>
    /// Returns a <see cref="IActionResult"/> indicating the result of the operation:
    /// - <c>NoContent</c> if the user was successfully deleted.
    /// - <c>BadRequest</c> if the Keycloak ID is invalid or if an error occurs during the operation.
    /// </returns>
    [Authorize(Policy = "UserManagePolicy")]
    [HttpDelete("{keycloakId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] string keycloakId)
    {
        if (string.IsNullOrWhiteSpace(keycloakId))
        {
            return BadRequest(new { error = "Keycloak ID is required." });
        }

        try
        {
            await _userService.DeleteUserAsync(keycloakId);

            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "deleted",
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Delete,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"User with keycloak id: {keycloakId} deleted successfully." }
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete user with keycloak id: {keycloakId}." }
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
    /// Restores a previously deleted user by their unique identifier.
    /// </summary>
    /// <param name="userId">
    /// The unique identifier of the user to be restored.
    /// </param>
    /// <returns>
    /// A <see cref="IActionResult"/> indicating the result of the restore operation.
    /// Returns 204 No Content if the restore is successful,
    /// 400 Bad Request if the userId is invalid or there is an error in processing,
    /// or 404 Not Found if the user does not exist.
    /// </returns>
    [Authorize(Policy = "UserManagePolicy")]
    [HttpPost("{userId}/restore")]
    public async Task<IActionResult> RestoreUser([FromRoute] Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("Invalid ID.");
        }

        try
        {
            await _userService.RestoreUserAsync(userId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to restore user with id: {userId}." }
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
    /// Retrieves a list of all active users from the system.
    /// </summary>
    /// <returns>A task representing the asynchronous operation that returns an IActionResult
    /// containing a list of all active users or a BadRequest with an error message in case of failure.</returns>
    [Authorize(Policy = "UserViewPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var result = await _userService.GetAllUsersAsync();
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all users." }
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

    /// Retrieves all deleted user profiles.
    /// <returns>A list of deleted user profiles encapsulated in a UserProfileDto if found; otherwise returns
    /// a NotFound response if no deleted users exist or BadRequest in case of an error.</returns>
    [Authorize(Policy = "UserViewPolicy")]
    [HttpGet("deleted")]
    public async Task<ActionResult<List<UserProfileDto>>> GetAllDeletedUsers()
    {
        try
        {
            var result = await _userService.GetAllDeletedUsersAsync();
            if (!result.Any())
            {
                return NotFound(new { error = "No deleted users found." });
            }
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
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all deleted users." }
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

    /// Retrieves the access token from the current HTTP request's Authorization header.
    /// The access token is expected to be provided in the Authorization header
    /// in the format "Bearer [token]". If the header is not present or does not follow
    /// the expected format, null is returned.
    /// <returns>
    /// A string representing the access token if found and valid; otherwise, null.
    /// </returns>
    private string? GetAccessTokenFromRequest()
    {
        if (HttpContext.Request.Cookies.TryGetValue("access_token", out var cookieToken))
        {
            return cookieToken;
        }

        return null;
    }

    [Authorize]
    [HttpGet("statistics/{userId}")]
    public async Task<IActionResult> GetUserStatistics([FromRoute] Guid userId)
    {
        var gamesOwnedCount = await _userLibraryService.GetUsersGamesCountAsync(userId);
        var dlcOwnedCount = await _userLibraryService.GetUsersAddonsCountAsync(userId);
        var followersCount = await _userFollowerService.GetFollowersCountAsync(userId);
        var wishlistCount = await _userWishListService.GetWishListCountAsync(userId);

        var statsDTO = new UserStatisticsDto
        {
            GamesOwned = gamesOwnedCount,
            DlcOwned = dlcOwnedCount,
            Follows = followersCount,
            Whishlisted = wishlistCount
        };

        return Ok(statsDTO);
    }
}