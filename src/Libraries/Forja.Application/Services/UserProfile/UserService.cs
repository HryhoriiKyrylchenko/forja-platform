namespace Forja.Application.Services.UserProfile;

/// <summary>
/// The UserService class provides methods for managing user profiles within the application.
/// </summary>
/// <remarks>
/// This class serves as the implementation of the <see cref="IUserService"/> interface and is responsible for
/// handling user-related operations including CRUD operations, retrieving all users, and managing deleted users.
/// </remarks>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IKeycloakClient _keycloakClient;

    public UserService(IUserRepository userRepository, IKeycloakClient keycloakClient)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _keycloakClient = keycloakClient ?? throw new ArgumentNullException(nameof(keycloakClient));
    }
    
    /// <inheritdoc />
    public async Task<UserProfileDto?> GetUserByIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        var user = await _userRepository.GetByIdAsync(userId);
        
        return user == null ? null : UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
    }

    /// <inheritdoc />
    public async Task<UserProfileDto?> GetDeletedUserByIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        var user = await _userRepository.GetDeletedByIdAsync(userId);
        
        return user == null ? null : UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
    }

    /// <inheritdoc />
    public async Task<UserProfileDto?> GetUserByKeycloakIdAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _userRepository.GetByKeycloakIdAsync(userKeycloakId);

        return user == null ? null : UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
    }

    /// <inheritdoc />
    public async Task<UserProfileDto?> GetDeletedUserByKeycloakIdAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _userRepository.GetDeletedByKeycloakIdAsync(userKeycloakId);

        return user == null ? null : UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
    }

    /// <inheritdoc />
    public async Task<UserProfileDto?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
        }
        
        var user = await _userRepository.GetByEmailAsync(email);
        
        return user == null ? null : UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
    }

    /// <inheritdoc />
    public async Task<UserProfileDto?> GetDeletedUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
        }
        
        var user = await _userRepository.GetDeletedByEmailAsync(email);
        
        return user == null ? null : UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
    }
    
    /// <inheritdoc />
    public async Task<List<UserProfileDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        var usersList = users.ToList();
        if (!usersList.Any())
        {
            return new List<UserProfileDto>();
        }
        
        return usersList.Select(UserProfileEntityToDtoMapper.MapToUserProfileDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserProfileDto>> GetAllDeletedUsersAsync()
    {
        var deletedUsers = await _userRepository.GetAllDeletedAsync();

        var deletedUsersList = deletedUsers.ToList();
        if (!deletedUsersList.Any())
        {
            return new List<UserProfileDto>();
        }
        
        return deletedUsersList.Select(UserProfileEntityToDtoMapper.MapToUserProfileDto).ToList();
    }

    /// <inheritdoc />
    public async Task<UserProfileDto?> AddUserAsync(UserCreateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateUserCreateRequest(request))
        {
            throw new ArgumentException("Invalid user create request.");
        }

        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with the same email already exists.");
        }

        if (string.IsNullOrWhiteSpace(request.Username) || 
            await _userRepository.ExistsByUsernameAsync(request.Username))
        {
            var baseUsername = request.Email.Split('@')[0];
            request.Username = await GenerateUniqueUsernameAsync(baseUsername);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            KeycloakUserId = request.KeycloakUserId,
            Username = request.Username,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            AvatarUrl = request.AvatarUrl,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            Country = request.Country,
            City = request.City,
            SelfDescription = request.SelfDescription,
            ShowPersonalInfo = request.ShowPersonalInfo,
            CreatedAt = request.CreatedAt,
            CustomUrl = request.CustomUrl,
            ProfileHatVariant = request.ProfileHatVariant
        };

        var result = await _userRepository.AddAsync(user);
        await _keycloakClient.EnableDisableUserAsync(user.KeycloakUserId, true);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserProfileDto(result);
    }

    /// <inheritdoc />
    public async Task<UserProfileDto?> UpdateUserAsync(UserUpdateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateUserUpdateRequest(request))
        {
            throw new ArgumentException("Invalid user update request.");
        }
        
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        user.KeycloakUserId = request.KeycloakUserId;
        user.Username = request.Username;
        user.Firstname = request.Firstname;
        user.Lastname = request.Lastname;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        user.BirthDate = request.BirthDate;
        user.Gender = request.Gender;
        user.Country = request.Country;
        user.City = request.City;
        user.SelfDescription = request.SelfDescription;
        user.ShowPersonalInfo = request.ShowPersonalInfo;
        user.ModifiedAt = request.ModifiedAt;
        user.CustomUrl = request.CustomUrl;
        user.ProfileHatVariant = request.ProfileHatVariant;
        
        await _userRepository.UpdateAsync(user);
        
        return UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
    }

    /// <inheritdoc />
    public async Task<UserProfileDto?> UpdateUserAvatarAsync(UserUpdateAvatarRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateUserUpdateAvatarRequest(request))
        {
            throw new ArgumentException("Invalid user update avatar request.");
        }
        
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        user.AvatarUrl = request.AvatarUrl;
        
        await _userRepository.UpdateAsync(user);
        
        return UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
    }

    /// <inheritdoc />
    public async Task<UserProfileDto?> UpdateProfileHatVariant(UserUpdateProfileHatVariantRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateUserUpdateProfileHatVariantRequest(request))
        {
            throw new ArgumentException("Invalid user update profile hat variant request.");
        }
        
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        user.ProfileHatVariant = request.Variant;
        
        await _userRepository.UpdateAsync(user);
        
        return UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
    }

    /// <inheritdoc />
    public async Task<UserProfileDto?> ConfirmEmailAsync(string keycloakUserId, bool confirmed)
    {
        if (string.IsNullOrWhiteSpace(keycloakUserId))
        {
            throw new ArgumentNullException(nameof(keycloakUserId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _userRepository.GetByKeycloakIdAsync(keycloakUserId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        user.IsEmailConfirmed = confirmed;
        
        await _userRepository.UpdateAsync(user);
        
        return UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
    }

    /// <inheritdoc />
    public async Task DeleteUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        await _userRepository.DeleteAsync(user.Id);
        await _keycloakClient.EnableDisableUserAsync(user.KeycloakUserId, false);
    }
    
    /// <inheritdoc />
    public async Task DeleteUserAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _userRepository.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        await _userRepository.DeleteAsync(user.Id);
        await _keycloakClient.EnableDisableUserAsync(user.KeycloakUserId, false);
    }

    /// <inheritdoc />
    public async Task<UserProfileDto?> RestoreUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        var user = await _userRepository.GetDeletedByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        await _userRepository.RestoreAsync(user.Id);
        await _keycloakClient.EnableDisableUserAsync(user.KeycloakUserId, true);
        
        return UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
    }

    /// <inheritdoc />
    public UserProfileDto HidePersonalData(UserProfileDto user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (!user.ShowPersonalInfo)
        {
            user.Firstname = null;
            user.Lastname = null;
            user.Email = "";
            user.PhoneNumber = null;
            user.BirthDate = null;
            user.Gender = null;
            user.Country = null;
            user.City = null;
            
            return user;
        }
        
        return user;
    }

    /// <summary>
    /// Asynchronously generates a unique username based on a provided base username.
    /// </summary>
    /// <param name="baseUsername">The base username to use for generating a unique username.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the generated unique username as a string.
    /// </returns>
    private async Task<string> GenerateUniqueUsernameAsync(string baseUsername)
    {
        if (string.IsNullOrWhiteSpace(baseUsername))
        {
            throw new ArgumentException("Base username must not be null or empty.", nameof(baseUsername));
        }
        
        return await _userRepository.GenerateUniqueUsernameAsync(baseUsername);
    }
}