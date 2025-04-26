namespace Forja.Application.Services.UserProfile;

/// <summary>
/// The UserService class provides methods for managing user profiles within the application.
/// </summary>
/// <remarks>
/// This class serves as the implementation of the <see cref="IUserService"/> interface and is responsible for
/// handling user-related operations including CRUD operations, retrieving all users, and managing deleted users.
/// </remarks>
public class UserService(
    IUserRepository userRepository,
    IKeycloakClient keycloakClient,
    IFileManagerService fileManagerService
) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IKeycloakClient _keycloakClient = keycloakClient ?? throw new ArgumentNullException(nameof(keycloakClient));
    private readonly IFileManagerService _fileManagerService = fileManagerService ?? throw new ArgumentNullException(nameof(fileManagerService));

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
        if (user == null)
            return null;

        var userWithAvatarUrl = UserProfileEntityToDtoMapper.MapToUserProfileDto(user); 
        userWithAvatarUrl.AvatarUrl = await _fileManagerService.GetPresignedUserAvatarUrlAsync(user.Id);
        
        return userWithAvatarUrl;        
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
        
        if (request.Username != null)
            user.Username = request.Username;
        if (request.Firstname != null)
            user.Firstname = request.Firstname;
        if (request.Lastname != null)
            user.Lastname = request.Lastname;
        if (request.Email != null)
            user.Email = request.Email;
        if (request.PhoneNumber != null)
            user.PhoneNumber = request.PhoneNumber;
        if (request.BirthDate.HasValue)
            user.BirthDate = request.BirthDate;
        if (request.Gender != null)
            user.Gender = request.Gender;
        if (request.Country != null)
            user.Country = request.Country;
        if (request.City != null)
            user.City = request.City;
        if (request.SelfDescription != null)
            user.SelfDescription = request.SelfDescription;
        if (request.CustomUrl != null)
            user.CustomUrl = request.CustomUrl;
        
        if (request.ShowPersonalInfo != null)
            user.ShowPersonalInfo = (bool)request.ShowPersonalInfo;
        if (request.ProfileHatVariant != null)
            user.ProfileHatVariant = (short)request.ProfileHatVariant;
        
        user.ModifiedAt = DateTime.UtcNow;
        
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
    public async Task<UserProfileDto?> ConfirmEmailAsync(string userId, bool confirmed)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");
        }
        
        var user = await _userRepository.GetByKeycloakIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        user.IsEmailConfirmed = confirmed;
        
        var result = await _userRepository.UpdateAsync(user);
        if (result == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        return UserProfileEntityToDtoMapper.MapToUserProfileDto(result);
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
        }
        
        return user;
    }

    /// <inheritdoc />
    public string GetKeycloakUserIdById(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        var user = _userRepository.GetByIdAsync(userId).Result;
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        return user.KeycloakUserId;
    }

    ///<inheritdoc/>
    public async Task<UserProfileDto?> GetUserByIdentifierAsync(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            throw new ArgumentNullException(nameof(identifier), "Identifier cannot be null or empty.");
        }

        var user = await _userRepository.GetByIdentifierAsync(identifier);
    
        return user == null ? null : UserProfileEntityToDtoMapper.MapToUserProfileDto(user);
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