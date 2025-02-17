namespace Forja.Application.Services;

public class UserRegistrationService : IUserRegistrationService
{
    private readonly IKeycloakClient _keycloakClient;
    private readonly IUserRepository _userRepository;
    
    public UserRegistrationService(IKeycloakClient keycloakClient, IUserRepository userRepository)
    {
        _keycloakClient = keycloakClient;
        _userRepository = userRepository;
    }
    
    public async Task RegisterUserAsync(RegisterUserCommand request)
    {
        // 1. Call Keycloak to create a new user with minimal data
        string keycloakId = await _keycloakClient.CreateUserAsync(new KeycloakUser
        {
            Email = request.Email,
            Password = request.Password
        });
        
        // 2. Create domain entity for the application user
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
        
        // 3. Save the new user in the local database
        await _userRepository.AddAsync(appUser);
    }
    
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
}