namespace Forja.Application.Services;

/// <summary>
/// Provides services for user registration, authentication, and role management operations.
/// </summary>
public class UserRegistrationService : IUserRegistrationService
{
    private readonly IKeycloakClient _keycloakClient;
    private readonly IUserProfileUnitOfWork _userProfileUnitOfWork;
    
    public UserRegistrationService(IKeycloakClient keycloakClient, IUserProfileUnitOfWork userProfileUnitOfWork)
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
}