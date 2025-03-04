namespace Forja.Application.Services.Authentication;

/// <summary>
/// Provides services for user registration, authentication, and role management operations.
/// </summary>
public class UserAuthService : IUserAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IKeycloakClient _keycloakClient;
    private readonly IUserRepository _userRepository;
    
    public UserAuthService(ITokenService tokenService, 
        IEmailService emailService,
        IKeycloakClient keycloakClient, 
        IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _emailService = emailService;
        _keycloakClient = keycloakClient;
        _userRepository = userRepository;
    }
    
    /// <inheritdoc />
    public async Task RegisterUserAsync(RegisterUserCommand request)
    {
        string keycloakId = await _keycloakClient.CreateUserAsync(new KeycloakUser
        {
            Email = request.Email,
            Password = request.Password
        });

        if (string.IsNullOrWhiteSpace(keycloakId))
        {
            throw new Exception("Failed to create user in Keycloak.");
        }
        
        await AssignRoleToUserAsync(keycloakId, UserRole.User);

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

        await _userRepository.AddAsync(appUser);
        
        var user = await _userRepository.GetByKeycloakIdAsync(keycloakId);

        if (user != null)
        {
            await SendEmailConfirmationAsync(keycloakId);
        }
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
    public async Task AssignRoleToUserAsync(string userId, UserRole role)
    {
        var roleRepresentation = await _keycloakClient.GetClientRoleByNameAsync(role.ToString());
        if (roleRepresentation == null)
        {
            await CreateRoleAsync(new CreateRoleCommand
            {
                RoleName = role.ToString(),
                Description = role.ToString()
            });
            
            roleRepresentation = await _keycloakClient.GetClientRoleByNameAsync(role.ToString());
            
            if (roleRepresentation == null)
            {
                throw new Exception("Failed to create role in Keycloak.");
            }
        }
        
        await _keycloakClient.AssignRoleAsync(userId, roleRepresentation);
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
        await _keycloakClient.SendRequiredActionEmailAsync(keycloakUserId, ["CONFIGURE_TOTP"]);
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

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with email {email} not found.");
        }
        
        string token = _tokenService.GeneratePasswordResetToken(user);

        string resetLink = $"/reset-password?token={Uri.EscapeDataString(token)}";

        await _emailService.SendPasswordResetEmailAsync(email, resetLink);
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
    public async Task ResetUserPasswordAsync(string userId, string newPassword, bool temporary = false)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(newPassword))
        {
            throw new ArgumentException("User ID and password must not be null or empty.");
        }
        
        await _keycloakClient.ResetUserPasswordAsync(userId, newPassword, temporary);
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

    /// <inheritdoc />
    public async Task<bool> ValidateResetTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token must not be null or empty.");
        }
        
        return await _tokenService.ValidatePasswordResetToken(token);
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
        
        while (await _userRepository.ExistsByUsernameAsync(username))
        {
            suffix++;
            username = $"{baseUsername}{suffix}";
        }
    
        return username;
    }
    
    public async Task SendEmailConfirmationAsync(string keycloakUserId)
    {
        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(keycloakUserId));
        }
        
        var user = await _userRepository.GetByKeycloakIdAsync(keycloakUserId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with keycloak ID {keycloakUserId} not found.");
        }

        if (string.IsNullOrWhiteSpace(user.Username))
        {
            throw new ArgumentException("User username cannot be null or empty.", nameof(user.Username));
        }
    
        var token = _tokenService.GenerateEmailConfirmationToken(Guid.Parse(keycloakUserId), user.Email);

        var confirmationLink = $"/api/Auth/users/{keycloakUserId}/confirm-email?token={token}";

        await _emailService.SendEmailConfirmationAsync(user.Email, user.Username, confirmationLink);
    }
    
    /// <inheritdoc />
    public async Task ConfirmUserEmailAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token must not be null or empty.");
        }
        
        var email = await _tokenService.GetEmailFromEmailConfirmationToken(token);

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Invalid token.");
        }

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || string.IsNullOrWhiteSpace(user.KeycloakUserId))
        {
            throw new KeyNotFoundException($"User with email {email} not found.");
        }

        await _keycloakClient.ConfirmUserEmailAsync(user.KeycloakUserId);
    }
}