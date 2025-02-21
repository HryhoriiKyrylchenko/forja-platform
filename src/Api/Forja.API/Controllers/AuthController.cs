namespace Forja.API.Controllers;

/// <summary>
/// Controller responsible for handling authentication, user registration, login, logout,
/// token refreshing, and role management in the application.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRegistrationService _registrationService;
    
    public AuthController(IUserRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    /// <summary>
    /// Handles the registration of a new user in the application.
    /// </summary>
    /// <param name="request">The details of the user to be registered, including email and password.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the registration process.
    /// Returns an Ok response if successful, or a Bad Request response with an error message if registration fails.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
        try
        {
            await _registrationService.RegisterUserAsync(request);
            return Ok("Registration successful");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Handles user login by validating credentials and generating an authentication token.
    /// </summary>
    /// <param name="request">The user's login details, including email and password.</param>
    /// <returns>An <see cref="IActionResult"/> containing the authentication token if login is successful.
    /// Returns a Bad Request response with an error message if login fails.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
    {
        try
        {
            var tokenResponse = await _registrationService.LoginUserAsync(request);
            return Ok(tokenResponse);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Handles the logout process for a user, invalidating their current session and associated tokens.
    /// </summary>
    /// <param name="request">The logout details, including the refresh token to be invalidated.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the logout process.
    /// Returns an Ok response if the logout is successful, or a Bad Request response with an error message if the process fails.</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand request)
    {
        try
        {
            await _registrationService.LogoutUserAsync(request);
            return Ok("Logout successful");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Refreshes the authentication token for a user using a provided refresh token.
    /// </summary>
    /// <param name="request">The refresh token data required to generate a new authentication token.</param>
    /// <returns>An <see cref="IActionResult"/> containing the new token if the process is successful,
    /// or a Bad Request response with an error message if the refresh token is invalid or the process fails.</returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand request)
    {
        try
        {
            var tokenResponse = await _registrationService.RefreshTokenAsync(request);
            return Ok(tokenResponse);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    // Role management endpoints

    /// <summary>
    /// Creates a new role in the application with the specified details.
    /// </summary>
    /// <param name="command">The details of the role to be created, including role name and description.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the role creation process.
    /// Returns an Ok response if the role is successfully created, or a Bad Request response with an error message if creation fails.</returns>
    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
    {
        try
        {
            await _registrationService.CreateRoleAsync(command);
            return Ok("Role created successfully");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all roles available in the application.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing a list of roles.
    /// Returns an Ok response with the roles if successful, or a Bad Request response with an error message if the operation fails.</returns>
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        try
        {
            var roles = await _registrationService.GetAllRolesAsync();
            return Ok(roles);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a role by its name.
    /// </summary>
    /// <param name="roleName">The name of the role to be retrieved.</param>
    /// <returns>An <see cref="IActionResult"/> containing the role details if the role is found.
    /// Returns a NotFound response if the role does not exist, or a Bad Request response in case of an error.</returns>
    [HttpGet("roles/{roleName}")]
    public async Task<IActionResult> GetRoleByName(string roleName)
    {
        try
        {
            var role = await _registrationService.GetRoleByNameAsync(roleName);
            if (role == null)
                return NotFound();
            return Ok(role);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves the roles assigned to a specified user by their user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose roles are to be retrieved.</param>
    /// <returns>An <see cref="IActionResult"/> containing the list of roles assigned to the user if successful.
    /// Returns a Bad Request response with an error message if the retrieval fails.</returns>
    [HttpGet("{userId}/roles")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        try
        {
            var roles = await _registrationService.GetUserRolesAsync(userId);
            return Ok(roles);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Checks if a specific user has a particular role assigned within the system.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose roles are being checked.</param>
    /// <param name="roleName">The name of the role to check if it is assigned to the user.</param>
    /// <returns>An <see cref="IActionResult"/> containing a boolean value indicating whether the user has the specified role.
    /// Returns an Ok response with the result if successful, or a Bad Request response with an error message if the check fails.</returns>
    [HttpGet("{userId}/role-check")]
    public async Task<IActionResult> CheckUserRole(string userId, [FromQuery] string roleName)
    {
        try
        {
            var result = await _registrationService.CheckUserRoleAsync(userId, roleName);
            return Ok(result);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Assigns a list of roles to a specified user.
    /// </summary>
    /// <param name="userId">The identifier of the user to whom the roles are being assigned.</param>
    /// <param name="roles">A collection of roles to be assigned to the user. Each role is represented as a <see cref="RoleRepresentation"/>.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns an Ok response if the roles are successfully assigned, or a Bad Request response with an error message if the operation fails.</returns>
    [HttpPost("{userId}/assign-roles")]
    public async Task<IActionResult> AssignRoles(string userId, [FromBody] IEnumerable<RoleRepresentation> roles)
    {
        try
        {
            await _registrationService.AssignRolesToUserAsync(userId, roles);
            return Ok("Roles assigned successfully");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Assigns a specific role to a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to whom the role will be assigned.</param>
    /// <param name="role">The details of the role to be assigned, including its name and description.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.
    /// Returns an Ok response if the role is successfully assigned, or a Bad Request response with an error message if the process fails.</returns>
    [HttpPost("{userId}/assign-role")]
    public async Task<IActionResult> AssignRole(string userId, [FromBody] RoleRepresentation role)
    {
        try
        {
            await _registrationService.AssignRoleToUserAsync(userId, role);
            return Ok("Role assigned successfully");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Removes specified roles from a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user from whom roles are to be deleted.</param>
    /// <param name="roles">A collection of roles to be removed from the user.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the deletion process.
    /// Returns an Ok response if successful, or a Bad Request response with an error message if an exception occurs.</returns>
    [HttpDelete("{userId}/delete-roles")]
    public async Task<IActionResult> DeleteRoles(string userId, [FromBody] IEnumerable<RoleRepresentation> roles)
    {
        try
        {
            await _registrationService.DeleteRolesFromUserAsync(userId, roles);
            return Ok("Roles deleted successfully");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a specified role from a user within the system.
    /// </summary>
    /// <param name="userId">The unique identifier of the user from whom the role will be removed.</param>
    /// <param name="role">The role to be deleted from the user's assigned roles.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the role deletion process.
    /// Returns an Ok response with a success message if successful, or a Bad Request response with an error message if the deletion fails.</returns>
    [HttpDelete("{userId}/delete-role")]
    public async Task<IActionResult> DeleteRole(string userId, [FromBody] RoleRepresentation role)
    {
        try
        {
            await _registrationService.DeleteRoleFromUserAsync(userId, role);
            return Ok("Role deleted successfully");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}