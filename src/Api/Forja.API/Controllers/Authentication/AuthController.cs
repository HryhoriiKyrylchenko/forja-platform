namespace Forja.API.Controllers.Authentication;

/// <summary>
/// Controller responsible for handling authentication, user registration, login, logout,
/// token refreshing, and role management in the application.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserAuthService _authService;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAnalyticsSessionService _analyticsSessionService;
    private readonly IAuditLogService _auditLogService;

    public AuthController(IUserAuthService authService, 
        IUserService userService, 
        IHttpContextAccessor httpContextAccessor,
        IAnalyticsSessionService analyticsSessionService,
        IAuditLogService auditLogService)
    {
        _authService = authService;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _analyticsSessionService = analyticsSessionService;
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Handles the registration of a new user in the application.
    /// </summary>
    /// <param name="request">The details of the user to be registered, including email and password.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the registration process.
    /// Returns an Ok response if successful, or a Bad Request response with an error message if registration fails.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        try
        {
            var result = await _authService.RegisterUserAsync(request);
            if (result == null)
            {
                return BadRequest(new { error = "Registration failed" });
            }
            
            try
            {
                var logEntry = new LogEntry<UserProfileDto>
                {
                    State = result,
                    UserId = result.Id,
                    Exception = null,
                    ActionType = AuditActionType.Create,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "User created successfully" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return Ok(new { Message = "Registration successful" });
        }
        catch(Exception ex)
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
                        { "Message", "Failed to create user" }
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
    /// Handles user login by validating credentials and generating an authentication token.
    /// </summary>
    /// <param name="request">The user's login details, including email and password.</param>
    /// <returns>An <see cref="IActionResult"/> containing the authentication token if login is successful.
    /// Returns a Bad Request response with an error message if login fails.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        try
        {
            var tokenResponse = await _authService.LoginUserAsync(request);

            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HTTP Context is unavailable.");

            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddSeconds(tokenResponse.RefreshExpiresIn)
            };

            httpContext.Response.Cookies.Append("access_token", tokenResponse.AccessToken, accessTokenOptions);
            httpContext.Response.Cookies.Append("refresh_token", tokenResponse.RefreshToken, refreshTokenOptions);

            var keycloakUserId = await _authService.GetKeycloakUserIdAsync(tokenResponse.AccessToken);
            var user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString(); 
                var userAgent = Request.Headers["User-Agent"].ToString(); 
                var authorizationHeader = Request.Headers["Authorization"].ToString();
                var customHeader = Request.Headers["X-Custom-Header"].ToString();

                var metadata = new Dictionary<string, string>
                {
                    { "User-Agent", userAgent },
                    { "IpAddress", ipAddress ?? "Unknown" },
                    { "Authorization", authorizationHeader },
                    { "CustomHeader", customHeader }
                };
                
                var session = await _analyticsSessionService.AddSessionAsync(user.Id, metadata);
                if (session == null)
                {
                    throw new Exception("Session not found");
                }

                var backendSessionOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };
                
                httpContext.Response.Cookies.Append("backend_session", session.SessionId.ToString(), backendSessionOptions);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed create a session: {e.Message}");
            }
            
            try
            {
                var logEntry = new LogEntry<TokenResponse>
                {
                    State = tokenResponse,
                    UserId = user.Id,
                    Exception = null,
                    ActionType = AuditActionType.Login,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "User logged in successfully" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }

            return Ok(tokenResponse);
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
                    ActionType = AuditActionType.Login,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to log in user" }
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
    /// Handles the logout process for a user, invalidating their current session and associated tokens.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the logout process.
    /// Returns an Ok response if the logout is successful, or a Bad Request response with an error message if the process fails.</returns>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (!Request.Cookies.TryGetValue("refresh_token", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest(new { error = "Refresh token is missing or invalid." });
        }

        var logoutRequest = new LogoutRequest { RefreshToken = refreshToken };

        try
        {
            await _authService.LogoutUserAsync(logoutRequest);

            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            try
            {
                if (Request.Cookies.TryGetValue("backend_session", out var sessionIdString)
                    && Guid.TryParse(sessionIdString, out var sessionId))
                {
                    await _analyticsSessionService.EndSessionAsync(sessionId);

                    Response.Cookies.Delete("backend_session");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to delete session: {e.Message}");
            }
            
            try
            {
                UserProfileDto? user = null;
                Request.Cookies.TryGetValue("access_token", out var accessToken);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    var keycloakUserId = await _authService.GetKeycloakUserIdAsync(accessToken);
                     user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
                }
                
                var logEntry = new LogEntry<string>
                {
                    State = "Success",
                    UserId = user?.Id ?? null,
                    Exception = null,
                    ActionType = AuditActionType.Login,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "User logged out successfully" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }

            return Ok(new { message = "Logout successful" });
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
                    ActionType = AuditActionType.Logout,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to log out user" }
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
    /// Refreshes the authentication token for a user using a provided refresh token.
    /// </summary>
    /// <param name="request">The refresh token data required to generate a new authentication token.</param>
    /// <returns>An <see cref="IActionResult"/> containing the new token if the process is successful,
    /// or a Bad Request response with an error message if the refresh token is invalid or the process fails.</returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var tokenResponse = await _authService.RefreshTokenAsync(request);
            
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HTTP Context is unavailable.");

            // set `access_token` & `refresh_token` in the HttpOnly Cookies**
            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddSeconds(tokenResponse.RefreshExpiresIn)
            };

            httpContext.Response.Cookies.Append("access_token", tokenResponse.AccessToken, accessTokenOptions);
            httpContext.Response.Cookies.Append("refresh_token", tokenResponse.RefreshToken, refreshTokenOptions);
            
            return Ok(tokenResponse);
        }
        catch(Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.AuthenticationError,
                    EntityType = AuditEntityType.User,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to refresh tokens" }
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
    
    // Role management endpoints

    /// <summary>
    /// Creates a new role in the application with the specified details.
    /// </summary>
    /// <param name="request">The details of the role to be created, including role name and description.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the role creation process.
    /// Returns an Ok response if the role is successfully created, or a Bad Request response with an error message if creation fails.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        try
        {
            await _authService.CreateRoleAsync(request);
            return Ok(new { Message = "Role created successfully" });
        }
        catch(Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to create keycloak role" }
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
    /// Creates all the default project roles listed in the UserRole enumeration.
    /// </summary>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpPost("all-default-roles")]
    public async Task<IActionResult> CreateAllDefaultProjectRoles()
    {
        try
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                await _authService.CreateRoleAsync(new CreateRoleRequest
                {
                    RoleName = role.ToString(),
                    Description = GetRoleDescription(role)
                });
            }
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
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to create keycloak roles" }
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
        

        return Ok(new { Message = "All default roles have been created successfully." });
        
        
        string GetRoleDescription(UserRole role)
        {
            var memberInfo = typeof(UserRole).GetMember(role.ToString());
            var descriptionAttribute = memberInfo[0]
                .GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
                .FirstOrDefault() as System.ComponentModel.DescriptionAttribute;

            return descriptionAttribute?.Description ?? role.ToString();
        }
    }

    /// <summary>
    /// Retrieves all roles available in the application.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing a list of roles.
    /// Returns an Ok response with the roles if successful, or a Bad Request response with an error message if the operation fails.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        try
        {
            var roles = await _authService.GetAllRolesAsync();
            return Ok(roles);
        }
        catch(Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to get all keycloak roles" }
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
    /// Retrieves a role by its name.
    /// </summary>
    /// <param name="roleName">The name of the role to be retrieved.</param>
    /// <returns>An <see cref="IActionResult"/> containing the role details if the role is found.
    /// Returns a NotFound response if the role does not exist, or a Bad Request response in case of an error.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("roles/{roleName}")]
    public async Task<IActionResult> GetRoleByName([FromRoute] string roleName)
    {
        try
        {
            var role = await _authService.GetRoleByNameAsync(roleName);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }
        catch(Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to get keycloak role by name" }
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
    /// Retrieves the roles assigned to a specified user by their user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose roles are to be retrieved.</param>
    /// <returns>An <see cref="IActionResult"/> containing the list of roles assigned to the user if successful.
    /// Returns a Bad Request response with an error message if the retrieval fails.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{userId}/roles")]
    public async Task<IActionResult> GetUserRoles([FromRoute] string userId)
    {
        try
        {
            var roles = await _authService.GetUserRolesAsync(userId);
            return Ok(roles);
        }
        catch(Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get keycloak user {userId} roles" }
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
    /// Checks if a specific user has a particular role assigned within the system.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose roles are being checked.</param>
    /// <param name="roleName">The name of the role to check if it is assigned to the user.</param>
    /// <returns>An <see cref="IActionResult"/> containing a boolean value indicating whether the user has the specified role.
    /// Returns an Ok response with the result if successful, or a Bad Request response with an error message if the check fails.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{userId}/role-check")]
    public async Task<IActionResult> CheckUserRole([FromRoute] string userId, [FromQuery] string roleName)
    {
        try
        {
            var result = await _authService.CheckUserRoleAsync(userId, roleName);
            return Ok(result);
        }
        catch(Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to check keycloak user {userId} role {roleName}" }
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
    /// Assigns a list of roles to a specified user.
    /// </summary>
    /// <param name="userId">The identifier of the user to whom the roles are being assigned.</param>
    /// <param name="roles">A collection of roles to be assigned to the user. Each role is represented as a <see cref="RoleRepresentation"/>.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns an Ok response if the roles are successfully assigned, or a Bad Request response with an error message if the operation fails.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("{userId}/assign-roles")]
    public async Task<IActionResult> AssignRoles([FromRoute] string userId, [FromBody] IEnumerable<RoleRepresentation> roles)
    {
        try
        {
            await _authService.AssignRolesToUserAsync(userId, roles);
            return Ok(new { Message = "Roles assigned successfully" });
        }
        catch(Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to assign roles to keycloak user {userId}" }
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
    /// Assigns a specific role to a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to whom the role will be assigned.</param>
    /// <param name="role">The details of the role to be assigned, including its name and description.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.
    /// Returns an Ok response if the role is successfully assigned, or a Bad Request response with an error message if the process fails.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("{userId}/assign-role")]
    public async Task<IActionResult> AssignRole([FromRoute] string userId, [FromBody] RoleRepresentation role)
    {
        try
        {
            await _authService.AssignRoleToUserAsync(userId, role);
            return Ok(new { Message = "Role assigned successfully" });
        }
        catch(Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to assign role {role.Name} to keycloak user {userId}" }
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
    /// Removes specified roles from a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user from whom roles are to be deleted.</param>
    /// <param name="roles">A collection of roles to be removed from the user.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the deletion process.
    /// Returns an Ok response if successful, or a Bad Request response with an error message if an exception occurs.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{userId}/delete-roles")]
    public async Task<IActionResult> DeleteRoles([FromRoute] string userId, [FromBody] IEnumerable<RoleRepresentation> roles)
    {
        try
        {
            await _authService.DeleteRolesFromUserAsync(userId, roles);
            return Ok(new { Message = "Roles deleted successfully" });
        }
        catch(Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete roles from keycloak user {userId}" }
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
    /// Deletes a specified role from a user within the system.
    /// </summary>
    /// <param name="userId">The unique identifier of the user from whom the role will be removed.</param>
    /// <param name="role">The role to be deleted from the user's assigned roles.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the role deletion process.
    /// Returns an Ok response with a success message if successful, or a Bad Request response with an error message if the deletion fails.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{userId}/delete-role")]
    public async Task<IActionResult> DeleteRole([FromRoute] string userId, [FromBody] RoleRepresentation role)
    {
        try
        {
            await _authService.DeleteRoleFromUserAsync(userId, role);
            return Ok(new { Message = "Role deleted successfully" });
        }
        catch(Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete role {role.Name} from keycloak user {userId}" }
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
    /// Handles the password change request for an authenticated user.
    /// </summary>
    /// <param name="accessToken">The access token of the authenticated user, provided in the request header.</param>
    /// <param name="request">The ChangePasswordRequest.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the password change operation.
    /// Returns an Ok response if successful, a Bad Request response if the new password is invalid, or an Unauthorized response if the access token is invalid.</returns>
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromHeader] string accessToken, [FromBody] ChangePasswordRequest request)
    {
        try
        {
            if (!AuthenticationRequestsValidator.ValidateChangePasswordRequest(request, out string _))
            {
                return BadRequest(new { error = "Invalid request." });
            }
            
            var isValid = await _authService.ValidateTokenAsync(accessToken);
            if (!isValid)
            {
                return Unauthorized(new { error = "Invalid access token." });
            }

            var userId = await _authService.GetKeycloakUserIdAsync(accessToken);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new { error = "Invalid access token." });
            }

            await _authService.ChangePasswordAsync(userId, request.Password);
            return Ok(new { Message = "Password changed successfully." });
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = accessToken,
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to delete roles from keycloak user" }
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
    /// Enables or disables a user account based on the provided request.
    /// </summary>
    /// <param name="request">The request containing the user ID and a boolean value indicating whether to enable or disable the account.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the operation.
    /// Returns an Ok response if the user is successfully enabled or disabled.
    /// Returns an error response if the operation fails or if the input is invalid.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("enable-disable-user")]
    public async Task<IActionResult> EnableDisableUser([FromBody] EnableDisableUserRequest request)
    {
        try
        {
            await _authService.EnableDisableUserAsync(request);
            var action = request.Enable ? "enabled" : "disabled";
            return Ok(new { Message = $"User has been {action} successfully." });
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
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to enable/disable keycloak user {request.KeycloakUserId}" }
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
    /// Confirms a Keycloak user's email.
    /// </summary>
    /// <param name="keycloakUserId">The Keycloak user ID whose email is to be confirmed.</param>
    /// <param name="token">The email confirmation token required to confirm email.</param>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPut("users/{keycloakUserId}/confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromRoute]string keycloakUserId, [FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            return BadRequest(new { error = "Keycloak User ID must be provided." });
        }

        try
        {
            await _authService.ConfirmUserEmailAsync(token);
            await _userService.ConfirmEmailAsync(keycloakUserId, true);
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
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to confirm email to keycloak user {keycloakUserId}" }
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
    /// Resets the password of a user in the system.
    /// </summary>
    /// <param name="request">The details of the password reset request, including the Keycloak User ID, new password, and whether the password is temporary.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.
    /// Returns a NoContent response if the password is successfully reset, or a BadRequest response with an error message if the input is invalid or the reset fails.</returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetUserPassword([FromBody] ResetUserPasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.KeycloakUserId) 
            || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "Keycloak User ID and new password must be provided." });
        }

        try
        {
            await _authService.ResetUserPasswordAsync(request);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to reset keycloak user {request.KeycloakUserId} password" }
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
    /// Initiates the forgot password process by sending a password reset email to the user.
    /// </summary>
    /// <param name="request">The request containing the email address of the user who wants to reset their password.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the forgot password request.
    /// Returns an Ok response if the email is successfully sent, or a Bad Request response if the email is invalid.</returns>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> TriggerForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { error = "Email cannot be null or empty." });
        }

        try
        {
            await _authService.TriggerForgotPasswordAsync(request.Email);
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
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to trigger forgot password for email {request.Email}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return Ok(new { Message = "Password reset email sent successfully." });
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
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to trigger forgot password for email {request.Email}" }
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
        
        return Ok(new { Message = "Password reset email sent successfully." });
    }

    /// <summary>
    /// Validates a password reset token to ensure it is valid and not expired.
    /// </summary>
    /// <param name="request">The password reset token to be validated.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the validation.
    /// Returns an Ok response with a success message if the token is valid,
    /// an Unauthorized response if the token is invalid or expired,
    /// or a Server Error response if an exception occurs during validation.</returns>
    [HttpPost("validate-reset-token")]
    public async Task<IActionResult> ValidateResetToken([FromBody] ValidateResetTokenRequest request)
    {
        try
        {
            var isValid = await _authService.ValidateResetTokenAsync(request);
            if (!isValid)
            {
                return Unauthorized(new { Message = "Invalid or expired token." });
                
            }
            return Ok(new { Message = "The token is valid." });
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
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to validate reset token" }
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
    /// Sends an email confirmation to the user associated with the provided access token.
    /// </summary>
    /// <param name="token">The access token of the user for whom the email confirmation will be sent.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the outcome of the operation.
    /// Returns an Ok response with a success message if the email is sent successfully, or a Bad Request response with an error message if the operation fails.</returns>
    [Authorize]
    [HttpPost("send-email-confirmation")]
    public async Task<IActionResult> SendEmailConfirmation([FromHeader] string token)
    {
        try
        {
            var keycloakUserId = await _authService.GetKeycloakUserIdAsync(token); 
            if (string.IsNullOrWhiteSpace(keycloakUserId))
            {
                return BadRequest(new { error = "User ID is required." });
            }
            
            await _authService.SendEmailConfirmationAsync(keycloakUserId);

            return Ok("Email confirmation sent successfully.");
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
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to send email confirmation on email" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return BadRequest( new { error = ex.Message });
        }
    }
}