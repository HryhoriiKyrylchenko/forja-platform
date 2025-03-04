namespace Forja.Application.Services.UserProfile;

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
    public async Task<UserProfileDto> GetUserProfileAsync(string userKeycloakId)
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

        return new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = user.AvatarUrl,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            Country = user.Country,
            City = user.City,
            SelfDescription = user.SelfDescription,
            ShowPersonalInfo = user.ShowPersonalInfo,
            CustomUrl = user.CustomUrl
        };
    }

    /// <inheritdoc />
    public async Task UpdateUserProfileAsync(UserProfileDto userProfileDto)
    {
        if (userProfileDto == null)
        {
            throw new ArgumentNullException(nameof(userProfileDto), "User profile cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(userProfileDto.Id.ToString()))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userProfileDto.Id));
        }

        if (string.IsNullOrWhiteSpace(userProfileDto.Username))
        {
            throw new ArgumentException("User username cannot be null or empty.", nameof(userProfileDto.Username));
        }

        if (string.IsNullOrWhiteSpace(userProfileDto.Email))
        {
            throw new ArgumentException("User email cannot be null or empty.", nameof(userProfileDto.Email));
        }
        
        var user = await _userRepository.GetByIdAsync(userProfileDto.Id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (!ApplicationDtoValidator.ValidateUserProfileDto(userProfileDto))
        {
            throw new ArgumentException("Invalid user profile data.");
        }
        
        user.Username = userProfileDto.Username;
        user.Firstname = userProfileDto.Firstname;
        user.Lastname = userProfileDto.Lastname;
        user.Email = userProfileDto.Email;
        user.PhoneNumber = userProfileDto.PhoneNumber;
        user.AvatarUrl = userProfileDto.AvatarUrl;
        user.BirthDate = userProfileDto.BirthDate;
        user.Gender = userProfileDto.Gender;
        user.Country = userProfileDto.Country;
        user.City = userProfileDto.City;
        user.SelfDescription = userProfileDto.SelfDescription;
        user.ShowPersonalInfo = userProfileDto.ShowPersonalInfo;
        user.CustomUrl = userProfileDto.CustomUrl;
        
        await _userRepository.UpdateAsync(user);
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
        await _keycloakClient.EnableDisableUserAsync(userKeycloakId, false);
    }

    /// <inheritdoc />
    public async Task RestoreUserAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _userRepository.GetDeletedByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        await _userRepository.RestoreAsync(user.Id);
        await _keycloakClient.EnableDisableUserAsync(userKeycloakId, true);
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
        
        return usersList.Select(user => new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = user.AvatarUrl,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            Country = user.Country,
            City = user.City,
            SelfDescription = user.SelfDescription,
            ShowPersonalInfo = user.ShowPersonalInfo,
            CustomUrl = user.CustomUrl
        }).ToList();
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
        
        return deletedUsersList.Select(user => new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = user.AvatarUrl,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            Country = user.Country,
            City = user.City,
            SelfDescription = user.SelfDescription,
            ShowPersonalInfo = user.ShowPersonalInfo,
            CustomUrl = user.CustomUrl
        }).ToList();
    }
}