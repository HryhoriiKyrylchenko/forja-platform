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

    public UserController(IUserService userService, IKeycloakClient keycloakClient)
    {
        _userService = userService;
        _keycloakClient = keycloakClient;
    }

    /// <summary>
    /// Retrieves the user profile for the specified Keycloak ID.
    /// </summary>
    /// <param name="keycloakId">The Keycloak ID of the user whose profile is to be retrieved.</param>
    /// <returns>An asynchronous operation that returns the user profile. If personal information is restricted, returns a profile without personal details.</returns>
    [HttpGet("profile/{keycloakId}")]
    public async Task<IActionResult> GetUserProfile([FromRoute] string keycloakId)
    {
        var result = await _userService.GetUserProfileAsync(keycloakId);

        result.Id = Guid.Empty;
        
        if (result.ShowPersonalInfo == false)
        {
            result.Firstname = null;
            result.Lastname = null;
            result.Email = string.Empty;
            result.PhoneNumber = null;
            result.BirthDate = null;
            result.Gender = null;
            result.Country = null;
            result.City = null;
        }

        return Ok(result);
    }

    /// <summary>
    /// Retrieves the authenticated user's profile based on the information from the access token.
    /// </summary>
    /// <returns>An asynchronous operation that returns the authenticated user's profile as a <see cref="UserProfileDto"/>.</returns>
    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        var accessToken = GetAccessTokenFromRequest();

        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized("Authorization header is missing or empty.");
        }

        var keycloakUserId = _keycloakClient.GetKeycloakUserId(accessToken);

        if (string.IsNullOrEmpty(keycloakUserId))
        {
            return Unauthorized("Unable to retrieve user information from token.");
        }
        
        var result = await _userService.GetUserProfileAsync(keycloakUserId);
        return Ok(result);
    }
    
    /// <summary>
    /// Retrieves the user profile associated with the specified Keycloak ID.
    /// </summary>
    /// <returns>An asynchronous operation that returns the user profile as a <see cref="UserProfileDto"/>.</returns>
    [Authorize(Policy = "UserViewPolicy")]
    [HttpGet("{keycloakId}")]
    public async Task<IActionResult> GetUserProfileForManager([FromRoute] string keycloakId)
    {
        var result = await _userService.GetUserProfileAsync(keycloakId);
        return Ok(result);
    }

    /// <summary>
    /// Updates the user's profile with the provided profile data.
    /// </summary>
    /// <param name="userProfileDto">The updated profile data to apply to the user's profile.</param>
    /// <returns>An asynchronous operation that, upon completion, returns a no-content response if the update is successful. Returns an unauthorized response if the user is not authorized to perform the operation.</returns>
    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileDto userProfileDto)
    {
        var accessToken = GetAccessTokenFromRequest();

        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized("Authorization header is missing or empty.");
        }

        var keycloakUserId = _keycloakClient.GetKeycloakUserId(accessToken);
        
        if (string.IsNullOrEmpty(keycloakUserId))
        {
            return Unauthorized("Unable to retrieve user information from token.");
        }

        var user = await _userService.GetUserProfileAsync(keycloakUserId);

        if (user.Id != userProfileDto.Id)
        {
            return Unauthorized("You are not authorized to update this user's profile.");
        }
        
        await _userService.UpdateUserProfileAsync(userProfileDto);
        return NoContent();
    }

    /// <summary>
    /// Updates the user profile with the provided details.
    /// </summary>
    /// <param name="userProfileDto">An object containing the updated details of the user profile.</param>
    /// <returns>An asynchronous operation.</returns>
    [Authorize(Policy = "UserManagePolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateUserProfileForManager([FromBody] UserProfileDto userProfileDto)
    {
        await _userService.UpdateUserProfileAsync(userProfileDto);
        return NoContent();
    }

    /// <summary>
    /// Deletes the authenticated user based on the Keycloak ID extracted from the token.
    /// </summary>
    /// <returns>An asynchronous operation that returns no content if the deletion is successful. If the user information cannot be retrieved from the token, returns an unauthorized result.</returns>
    [Authorize]
    [HttpDelete("profile")]
    public async Task<IActionResult> DeleteUser()
    {
        var accessToken = GetAccessTokenFromRequest();

        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized("Authorization header is missing or empty.");
        }

        var keycloakUserId = _keycloakClient.GetKeycloakUserId(accessToken);
        
        if (string.IsNullOrEmpty(keycloakUserId))
        {
            return Unauthorized("Unable to retrieve user information from token.");
        }
        
        await _userService.DeleteUserAsync(keycloakUserId);
        return NoContent();
    }

    /// <summary>
    /// Deletes the user associated with the specified Keycloak ID and disables the user in Keycloak.
    /// </summary>
    /// <param name="keycloakId">The unique Keycloak ID of the user to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation for deleting the user.</returns>
    [Authorize(Policy = "UserManagePolicy")]
    [HttpDelete("{keycloakId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] string keycloakId)
    {
        await _userService.DeleteUserAsync(keycloakId);
        return NoContent();
    }
    
    /// <summary>
    /// Restores a soft-deleted user identified by their Keycloak ID.
    /// </summary>
    /// <param name="keycloakId">The unique Keycloak ID of the user to restore.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Authorize(Policy = "UserManagePolicy")]
    [HttpPost("{keycloakId}/restore")]
    public async Task<IActionResult> RestoreUser([FromRoute]string keycloakId)
    {
        if (string.IsNullOrWhiteSpace(keycloakId))
        {
            return BadRequest("Keycloak ID is required.");
        }

        try
        {
            await _userService.RestoreUserAsync(keycloakId);
            return NoContent();  
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a list of all user profiles from the system.
    /// </summary>
    /// <returns>An asynchronous operation that returns a list of all user profiles as <see cref="UserProfileDto"/> objects.</returns>
    [Authorize(Policy = "UserViewPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a list of all deleted user profiles.
    /// </summary>
    /// <returns>An asynchronous operation that returns a list of deleted user profiles as a collection of <see cref="UserProfileDto"/>.</returns>
    [Authorize(Policy = "UserViewPolicy")]
    [HttpGet("deleted")]
    public async Task<IActionResult> GetAllDeletedUsers()
    {
        var result = await _userService.GetAllDeletedUsersAsync();
        return Ok(result);
    }
    
    private string? GetAccessTokenFromRequest()
    {
        var accessToken = HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return null; // Indicate that the token is missing or invalid
        }

        // Optionally, remove "Bearer " prefix if present
        if (accessToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            accessToken = accessToken.Substring("Bearer ".Length).Trim();
        }

        return accessToken;
    }
}