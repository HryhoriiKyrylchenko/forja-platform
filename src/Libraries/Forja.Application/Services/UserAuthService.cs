namespace Forja.Application.Services;

/// <summary>
/// Provides services for user registration, authentication, and role management operations.
/// </summary>
public class UserAuthService : IUserAuthService
{
    private readonly IKeycloakClient _keycloakClient;
    private readonly IUserProfileUnitOfWork _userProfileUnitOfWork;
    
    public UserAuthService(IKeycloakClient keycloakClient, IUserProfileUnitOfWork userProfileUnitOfWork)
    {
        _keycloakClient = keycloakClient;
        _userProfileUnitOfWork = userProfileUnitOfWork;
    }
    
    /// <inheritdoc />
    public async Task RegisterUserAsync(RegisterUserCommand request)
    {
        string keycloakId = await _keycloakClient.CreateUserAsync(new KeycloakUser
        {
            Email = request.Email,
            Password = request.Password
        });

        var baseUsername = request.Email.Split('@')[0];
        var username = await GenerateUniqueUsernameAsync(baseUsername);
        
        var appUser = new User
        {
            Id = Guid.NewGuid(),
            KeycloakUserId = keycloakId,
            Username = username,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        await _userProfileUnitOfWork.Users.AddAsync(appUser);
        await _userProfileUnitOfWork.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task<TokenResponse> LoginUserAsync(LoginUserCommand request)
    {
        return await _keycloakClient.LoginAsync(request.Email, request.Password);
    }
    
    /// <inheritdoc />
    public async Task LogoutUserAsync(LogoutCommand request)
    {
        await _keycloakClient.LogoutAsync(request.RefreshToken);
    }
    
    /// <inheritdoc />
    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenCommand request)
    {
        return await _keycloakClient.RequestNewTokensAsync(request.RefreshToken);
    }
    
    /// <inheritdoc />
    public async Task CreateRoleAsync(CreateRoleCommand command)
    {
        await _keycloakClient.CreateClientRoleAsync(command.RoleName, command.Description);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<RoleRepresentation>> GetAllRolesAsync()
    {
        return await _keycloakClient.GetClientRolesAsync();
    }
        
    /// <inheritdoc />
    public async Task<RoleRepresentation?> GetRoleByNameAsync(string roleName)
    {
        return await _keycloakClient.GetClientRoleByNameAsync(roleName);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<RoleRepresentation>> GetUserRolesAsync(string userId)
    {
        return await _keycloakClient.GetUserRolesAsync(userId);
    }
        
    /// <inheritdoc />
    public async Task<bool> CheckUserRoleAsync(string userId, string roleName)
    {
        return await _keycloakClient.CheckUserRoleAsync(userId, roleName);
    }
    /// <inheritdoc />
    public async Task AssignRolesToUserAsync(string userId, IEnumerable<RoleRepresentation> roles)
    {
        await _keycloakClient.AssignRolesAsync(userId, roles);
    }
    
    /// <inheritdoc />
    public async Task AssignRoleToUserAsync(string userId, RoleRepresentation role)
    {
        await _keycloakClient.AssignRoleAsync(userId, role);
    }
    
    /// <inheritdoc />
    public async Task DeleteRolesFromUserAsync(string userId, IEnumerable<RoleRepresentation> roles)
    {
        await _keycloakClient.DeleteClientRolesAsync(userId, roles);
    }
        
    /// <inheritdoc />
    public async Task DeleteRoleFromUserAsync(string userId, RoleRepresentation role)
    {
        await _keycloakClient.DeleteClientRoleAsync(userId, role);
    }
    
    /// <inheritdoc />
    public async Task<UserRepresentation?> GetKeycloakUserByEmailAsync(string email)
    {
        return await _keycloakClient.GetUserByEmailAsync(email);
    }

    /// <inheritdoc />
    public async Task ChangePasswordAsync(string keycloakUserId, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(keycloakUserId) || string.IsNullOrWhiteSpace(newPassword))
        {
            throw new ArgumentException("User ID and password must not be null or empty.");
        }

        await _keycloakClient.ChangePasswordAsync(keycloakUserId, newPassword);
    }

    /// <inheritdoc />
    public async Task EnableTwoFactorAuthenticationAsync(string keycloakUserId)
    {
        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentException("User ID must not be null or empty.");
        }

        await _keycloakClient.EnableTwoFactorAuthenticationAsync(keycloakUserId);
    }

    /// <inheritdoc />
    public async Task DisableTwoFactorAuthenticationAsync(string keycloakUserId)
    {
        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentException("User ID must not be null or empty.");
        }

        await _keycloakClient.DisableTwoFactorAuthenticationAsync(keycloakUserId);
    }

    /// <inheritdoc />
    public async Task<bool> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token must not be null or empty.");
        }

        return await _keycloakClient.ValidateTokenAsync(token);
    }

    /// <inheritdoc />
    public async Task TriggerForgotPasswordAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email must not be null or empty.");
        }

        await _keycloakClient.TriggerForgotPasswordAsync(email);
    }

    /// <inheritdoc />
    public async Task EnableDisableUserAsync(string keycloakUserId, bool enable)
    {
        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentException("User ID must not be null or empty.");
        }

        await _keycloakClient.EnableDisableUserAsync(keycloakUserId, enable);
    }

    /// <inheritdoc />
    public async Task<string> GetKeycloakUserIdAsync(string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new ArgumentException("Access token must not be null or empty.");
        }

        // Directly call the GetKeycloakUserId method from the IKeycloakClient
        return await Task.FromResult(_keycloakClient.GetKeycloakUserId(accessToken));
    }

    /// <summary>
    /// Generates a unique username by appending a numeric suffix to the provided base username
    /// if it conflicts with an existing username in the system.
    /// </summary>
    /// <param name="baseUsername">The base username that needs to be made unique.</param>
    /// <returns>A unique username string derived from the base username.</returns>
    private async Task<string> GenerateUniqueUsernameAsync(string baseUsername)
    {
        baseUsername = baseUsername.Trim().ToLowerInvariant();
        string username = baseUsername;
        int suffix = 0;
        
        while (await _userProfileUnitOfWork.Users.ExistsByUsernameAsync(username))
        {
            suffix++;
            username = $"{baseUsername}{suffix}";
        }
    
        return username;
    }
    
    /// <inheritdoc />
    public async Task ConfirmUserEmailAsync(string keycloakUserId)
    {
        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentException("Keycloak User ID cannot be null or empty.", nameof(keycloakUserId));
        }

        await _keycloakClient.ConfirmUserEmailAsync(keycloakUserId);
    }

}