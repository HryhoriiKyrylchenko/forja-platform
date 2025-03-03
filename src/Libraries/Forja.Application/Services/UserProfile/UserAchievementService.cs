namespace Forja.Application.Services.UserProfile;

/// <summary>
/// A service class for managing user achievements in the application.
/// </summary>
public class UserAchievementService : IUserAchievementService
{
    private readonly IUserAchievementRepository _userAchievementRepository;
    private readonly IUserRepository _userRepository;

    public UserAchievementService(IUserAchievementRepository userAchievementRepository, IUserRepository userRepository)
    {
        _userAchievementRepository = userAchievementRepository ?? throw new ArgumentNullException(nameof(userAchievementRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }
    
    /// <inheritdoc />
    public async Task AddUserAchievementAsync(UserAchievementDto userAchievementDto)
    {
        if (!ApplicationDtoValidator.ValidateUserAchievementDto(userAchievementDto))
        {
            throw new ArgumentException("User achievement DTO is invalid.", nameof(userAchievementDto));
        }

        var userAchievement = new UserAchievement()
        {
            Id = userAchievementDto.Id,
            UserId = userAchievementDto.User.Id,
            AchievementId = userAchievementDto.Achievement.Id,
            AchievedAt = userAchievementDto.AchievedAt
        };
        
        await _userAchievementRepository.AddAsync(userAchievement);
    }

    /// <inheritdoc />
    public async Task<UserAchievementDto> GetUserAchievementByIdAsync(Guid userAchievementId)
    {
        if (userAchievementId == Guid.Empty)
        {
            throw new ArgumentException("User achievement ID cannot be empty.", nameof(userAchievementId));
        }
        
        var userAchievement = await _userAchievementRepository.GetByIdAsync(userAchievementId);
        if (userAchievement == null)
        {
            throw new KeyNotFoundException($"User achievement with ID '{userAchievementId}' was not found.");
        }

        return new UserAchievementDto()
        {
            Id = userAchievement.Id,
            User = userAchievement.User,
            Achievement = userAchievement.Achievement,
            AchievedAt = userAchievement.AchievedAt
        };
    }

    /// <inheritdoc />
    public async Task UpdateUserAchievement(UserAchievementDto userAchievementDto)
    {
        if (!ApplicationDtoValidator.ValidateUserAchievementDto(userAchievementDto))
        {
            throw new ArgumentException("User achievement DTO is invalid.", nameof(userAchievementDto));
        }
        
        var userAchievement = await _userAchievementRepository.GetByIdAsync(userAchievementDto.Id);
        if (userAchievement == null)
        {
            throw new InvalidOperationException($"User achievement with ID {userAchievementDto.Id} does not exist.");
        }
        
        userAchievement.AchievedAt = userAchievementDto.AchievedAt;
        userAchievement.UserId = userAchievementDto.User.Id;
        userAchievement.AchievementId = userAchievementDto.Achievement.Id;
        
        await _userAchievementRepository.UpdateAsync(userAchievement);
    }

    /// <inheritdoc />
    public async Task DeleteUserAchievementAsync(Guid userAchievementId)
    {
        if (userAchievementId == Guid.Empty)
        {
            throw new ArgumentException("User achievement ID cannot be an empty Guid.", nameof(userAchievementId));
        }
        
        var userAchievement = await _userAchievementRepository.GetByIdAsync(userAchievementId);
        if (userAchievement == null)
        {
            throw new KeyNotFoundException($"User achievement with ID '{userAchievementId}' was not found.");
        }
        
        await _userAchievementRepository.DeleteAsync(userAchievementId);
    }

    /// <inheritdoc />
    public async Task<List<UserAchievementDto>> GetAllUserAchievementsAsync()
    {
        var allUserAchievements = await _userAchievementRepository.GetAllAsync();

        var userAchievements = allUserAchievements.ToList();
        if (!userAchievements.Any())
        {
            return new List<UserAchievementDto>();
        }
        
        return userAchievements.Select(userAchievement => new UserAchievementDto()
        {
            Id = userAchievement.Id,
            User = userAchievement.User,
            Achievement = userAchievement.Achievement,
            AchievedAt = userAchievement.AchievedAt
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserAchievementDto>> GetAllUserAchievementsByUserKeycloakIdAsync(string userKeycloakId)
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

        var allUserAchievements = await _userAchievementRepository.GetAllByUserIdAsync(user.Id);
        
        var userAchievements = allUserAchievements.ToList();
        if (!userAchievements.Any())
        {
            return new List<UserAchievementDto>();
        }
        
        return userAchievements.Select(userAchievement => new UserAchievementDto()
        {
            Id = userAchievement.Id,
            User = userAchievement.User,
            Achievement = userAchievement.Achievement,
            AchievedAt = userAchievement.AchievedAt
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserAchievementDto>> GetAllUserAchievementsByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var allUserAchievements = await _userAchievementRepository.GetAllByGameIdAsync(gameId);
        
        var userAchievements = allUserAchievements.ToList();
        if (!userAchievements.Any())
        {
            return new List<UserAchievementDto>();
        }
        
        return userAchievements.Select(userAchievement => new UserAchievementDto()
        {
            Id = userAchievement.Id,
            User = userAchievement.User,
            Achievement = userAchievement.Achievement,
            AchievedAt = userAchievement.AchievedAt
        }).ToList();
    }
}