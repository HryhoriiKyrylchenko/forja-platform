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

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Retrieves the user profile associated with the specified Keycloak ID.
    /// </summary>
    /// <param name="keycloakId">The unique Keycloak ID of the user whose profile is to be retrieved.</param>
    /// <returns>An asynchronous operation that returns the user profile as a <see cref="UserProfileDto"/>.</returns>
    [HttpGet("{keycloakId}")]
    public async Task<IActionResult> GetUserProfile(string keycloakId)
    {
        var result = await _userService.GetUserProfileAsync(keycloakId);
        return Ok(result);
    }

    /// <summary>
    /// Updates the user profile with the provided details.
    /// </summary>
    /// <param name="userProfileDto">An object containing the updated details of the user profile.</param>
    /// <returns>An asynchronous operation.</returns>
    [HttpPut]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileDto userProfileDto)
    {
        await _userService.UpdateUserProfileAsync(userProfileDto);
        return NoContent();
    }

    /// <summary>
    /// Deletes the user associated with the specified Keycloak ID and disables the user in Keycloak.
    /// </summary>
    /// <param name="keycloakId">The unique Keycloak ID of the user to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation for deleting the user.</returns>
    [HttpDelete("{keycloakId}")]
    public async Task<IActionResult> DeleteUser(string keycloakId)
    {
        await _userService.DeleteUserAsync(keycloakId);
        return NoContent();
    }
    
    /// <summary>
    /// Restores a soft-deleted user identified by their Keycloak ID.
    /// </summary>
    /// <param name="keycloakId">The unique Keycloak ID of the user to restore.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [HttpPost("{keycloakId}/restore")]
    public async Task<IActionResult> RestoreUser(string keycloakId)
    {
        // Validate input
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
    [HttpGet("deleted")]
    public async Task<IActionResult> GetAllDeletedUsers()
    {
        var result = await _userService.GetAllDeletedUsersAsync();
        return Ok(result);
    }
}