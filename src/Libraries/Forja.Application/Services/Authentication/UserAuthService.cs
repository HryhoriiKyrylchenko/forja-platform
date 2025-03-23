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
    public async Task<UserProfileDto?> RegisterUserAsync(RegisterUserRequest request)
    {
        if (!AuthenticationRequestsValidator.ValidateRegisterUserRequest(request, out string _))
        {
            throw new ArgumentException("Invalid register user request.");
        }
        
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
            CreatedAt = DateTime.UtcNow,
            IsEmailConfirmed = false
        };

        var result = await _userRepository.AddAsync(appUser);
        
        var user = await _userRepository.GetByKeycloakIdAsync(keycloakId);
        bool emailSent = false;

        if (user != null)
        {
            try
            {
                await SendEmailConfirmationAsync(keycloakId);
                emailSent = true;

                await _userRepository.UpdateAsync(appUser);
            }
            catch (Exception ex)
            {
            }
        }
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserProfileDto(result);
    }
    
    /// <inheritdoc />
    public async Task<TokenResponse> LoginUserAsync(LoginUserRequest request)
    {
        if (!AuthenticationRequestsValidator.ValidateLoginUserRequest(request, out string _))
        {
            throw new ArgumentException("Invalid login user request.");
        }
        return await _keycloakClient.LoginAsync(request.Email, request.Password);
    }
    
    /// <inheritdoc />
    public async Task LogoutUserAsync(LogoutRequest request)
    {
        if (!AuthenticationRequestsValidator.ValidateLogoutRequest(request, out string _))
        {
            throw new ArgumentException("Invalid logout request.");
        }
        await _keycloakClient.LogoutAsync(request.RefreshToken);
    }
    
    /// <inheritdoc />
    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        if (!AuthenticationRequestsValidator.ValidateRefreshTokenRequest(request, out string _))
        {
            throw new ArgumentException("Invalid refresh token request.");
        }
        return await _keycloakClient.RequestNewTokensAsync(request.RefreshToken);
    }
    
    /// <inheritdoc />
    public async Task CreateRoleAsync(CreateRoleRequest request)
    {
        if (!AuthenticationRequestsValidator.ValidateCreateRoleRequest(request, out string _))
        {
            throw new ArgumentException("Invalid create role request.");
        }
        await _keycloakClient.CreateClientRoleAsync(request.RoleName, request.Description);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<RoleRepresentation>> GetAllRolesAsync()
    {
        return await _keycloakClient.GetClientRolesAsync();
    }
        
    /// <inheritdoc />
    public async Task<RoleRepresentation?> GetRoleByNameAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
        }
        return await _keycloakClient.GetClientRoleByNameAsync(roleName);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<RoleRepresentation>> GetUserRolesAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
        }
        return await _keycloakClient.GetUserRolesAsync(userId);
    }
        
    /// <inheritdoc />
    public async Task<bool> CheckUserRoleAsync(string userId, string roleName)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
        }
        return await _keycloakClient.CheckUserRoleAsync(userId, roleName);
    }
    /// <inheritdoc />
    public async Task AssignRolesToUserAsync(string userId, IEnumerable<RoleRepresentation> roles)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
        }

        if (roles == null)
        {
            throw new ArgumentNullException(nameof(roles), "Roles cannot be null.");
        }
        await _keycloakClient.AssignRolesAsync(userId, roles);
    }
    
    /// <inheritdoc />
    public async Task AssignRoleToUserAsync(string userId, RoleRepresentation role)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
        }

        if (role == null)
        {
            throw new ArgumentNullException(nameof(role), "Role cannot be null.");
        }
        await _keycloakClient.AssignRoleAsync(userId, role);
    }
    
    /// <inheritdoc />
    public async Task AssignRoleToUserAsync(string userId, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
        }
        
        var roleRepresentation = await _keycloakClient.GetClientRoleByNameAsync(role.ToString());
        if (roleRepresentation == null)
        {
            await CreateRoleAsync(new CreateRoleRequest
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
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
        }

        if (roles == null)
        {
            throw new ArgumentNullException(nameof(roles), "Roles cannot be null.");
        }
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
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }
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
    public async Task EnableDisableUserAsync(EnableDisableUserRequest request)
    {
        if (!AuthenticationRequestsValidator.ValidateEnableDisableUserRequest(request, out string _))
        {
            throw new ArgumentException("Invalid enable/disable user request.");
        }

        await _keycloakClient.EnableDisableUserAsync(request.KeycloakUserId, request.Enable);
    }

    /// <inheritdoc />
    public async Task ResetUserPasswordAsync(ResetUserPasswordRequest request)
    {
        if (!AuthenticationRequestsValidator.ValidateResetUserPasswordRequest(request, out string _))
        {
            throw new ArgumentException("Invalid reset user password request.");
        }
        
        await _keycloakClient.ResetUserPasswordAsync(request.KeycloakUserId, request.Password, request.Temporary);
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
    public async Task<bool> ValidateResetTokenAsync(ValidateResetTokenRequest request)
    {
        if (!AuthenticationRequestsValidator.ValidateValidateResetTokenRequest(request, out string _))
        {
            throw new ArgumentException("Invalid validate reset token request.");
        }
        
        return await _tokenService.ValidatePasswordResetToken(request.Token);
    }
    
    /// <inheritdoc />
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
    
    /// <summary>
    /// Generates a unique username by appending a numeric suffix to the provided base username
    /// if it conflicts with an existing username in the system.
    /// </summary>
    /// <param name="baseUsername">The base username that needs to be made unique.</param>
    /// <returns>A unique username string derived from the base username.</returns>
    private async Task<string> GenerateUniqueUsernameAsync(string baseUsername)
    {
        if (string.IsNullOrWhiteSpace(baseUsername))
        {
            throw new ArgumentException("Base username must not be null or empty.", nameof(baseUsername));
        }
        
        return await _userRepository.GenerateUniqueUsernameAsync(baseUsername);
    }
}