namespace Forja.Application.Services.UserProfile;

/// <summary>
/// Service for managing achievements in the application.
/// </summary>
public class AchievementService : IAchievementService
{
    private readonly IAchievementRepository _achievementRepository;
    private readonly IUserAchievementRepository _userAchievementRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IFileManagerService _fileManagerService;

    public AchievementService(IAchievementRepository achievementRepository, 
        IUserAchievementRepository userAchievementRepository, 
        IUserRepository userRepository, 
        IGameRepository gameRepository,
        IFileManagerService fileManagerService)
    {
        _achievementRepository = achievementRepository;
        _userAchievementRepository = userAchievementRepository;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _fileManagerService = fileManagerService;
    }
    
    #region Achievement Methods
    
    /// <inheritdoc />
    public async Task<AchievementDto> AddAchievementAsync(AchievementCreateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateAchievementCreateRequest(request))
        {
            throw new ArgumentException("Achievement request is invalid.", nameof(request));
        }
        
        var achievement = new Achievement()
        {
            Id = Guid.NewGuid(),
            GameId = request.GameId,
            Name = request.Name,
            Description = request.Description,
            Points = request.Points,
            LogoUrl = request.LogoUrl
        };        
    
        await _achievementRepository.AddAsync(achievement);

        var game = await _gameRepository.GetByIdAsync(request.GameId);
        if (game == null)
        {
            throw new InvalidOperationException($"Game with ID {request.GameId} does not exist.");
        }
        achievement.Game = game;

        return UserProfileEntityToDtoMapper.MapToAchievementDto(achievement);
    }

    /// <inheritdoc />
    public async Task<AchievementDto?> GetAchievementByIdAsync(Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement ID cannot be empty.", nameof(achievementId));
        }
        
        var achievement = await _achievementRepository.GetByIdAsync(achievementId);

        return achievement == null ? null : UserProfileEntityToDtoMapper.MapToAchievementDto(achievement);
    }

    /// <inheritdoc />
    public async Task UpdateAchievementAsync(AchievementUpdateRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "Achievement cannot be null.");
        }
        
        if (!UserProfileRequestsValidator.ValidateAchievementUpdateRequest(request))
        {
            throw new ArgumentException("Achievement request is invalid.", nameof(request));
        }
        
        var existingAchievement = await _achievementRepository.GetByIdAsync(request.Id);
        if (existingAchievement == null)
        {
            throw new InvalidOperationException($"Achievement with ID {request.Id} does not exist.");
        }
        
        existingAchievement.Name = request.Name;
        existingAchievement.Description = request.Description;
        existingAchievement.Points = request.Points;
        existingAchievement.LogoUrl = request.LogoUrl;
        existingAchievement.GameId = request.GameId;
        
        await _achievementRepository.UpdateAsync(existingAchievement);
    }

    /// <inheritdoc />
    public async Task DeleteAchievementAsync(Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement ID cannot be an empty Guid.", nameof(achievementId));
        }
        
        var achievement = await _achievementRepository.GetByIdAsync(achievementId);
        if (achievement == null)
        {
            throw new KeyNotFoundException($"Achievement with ID '{achievementId}' was not found.");
        }
        
        await _achievementRepository.DeleteAsync(achievementId);
    }

    /// <inheritdoc />
    public async Task<AchievementDto?> RestoreAchievementAsync(Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement ID cannot be an empty Guid.", nameof(achievementId));
        }
        
        var achievement = await _achievementRepository.RestoreAsync(achievementId);
        
        return achievement == null ? null : UserProfileEntityToDtoMapper.MapToAchievementDto(achievement);
    }

    /// <inheritdoc />
    public async Task<List<AchievementDto>> GetAllGameAchievementsAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var gameAchievements = await _achievementRepository.GetAllByGameIdAsync(gameId);

        var achievementsList = gameAchievements.ToList();
        foreach (var achievement in achievementsList)
        {
            achievement.LogoUrl = await _fileManagerService.GetPresignedAchievementImageUrlAsync(achievement.Id);
        }

        if (!achievementsList.Any())
        {
            return new List<AchievementDto>();
        }
        
        return achievementsList.Select(UserProfileEntityToDtoMapper.MapToAchievementDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<AchievementDto>> GetAllGameDeletedAchievementsAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var deletedGameAchievements = await _achievementRepository.GetAllDeletedByGameIdAsync(gameId);

        return deletedGameAchievements.Select(UserProfileEntityToDtoMapper.MapToAchievementDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<AchievementDto>> GetAllAchievementsAsync()
    {
        var allAchievements = await _achievementRepository.GetAllAsync();

        var achievements = allAchievements.ToList();
        if (!achievements.Any())
        {
            return new List<AchievementDto>();
        }
        
        return achievements.Select(UserProfileEntityToDtoMapper.MapToAchievementDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<AchievementDto>> GetAllDeletedAchievementsAsync()
    {
        var allDeletedAchievements = await _achievementRepository.GetAllDeletedAsync();

        var achievements = allDeletedAchievements.ToList();
        if (!achievements.Any())
        {
            return new List<AchievementDto>();
        }
        
        return achievements.Select(UserProfileEntityToDtoMapper.MapToAchievementDto).ToList();
    }
    
    #endregion

    #region UserAchievement Methods
    
    /// <inheritdoc />
    public async Task<UserAchievementDto?> AddUserAchievementAsync(UserAchievementCreateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateUserAchievementCreateRequest(request))
        {
            throw new ArgumentException("User achievement DTO is invalid.", nameof(request));
        }

        var userAchievement = new UserAchievement()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            AchievementId = request.AchievementId,
            AchievedAt = request.AchievedAt
        };
        
        var addedAchievement = await _userAchievementRepository.AddAsync(userAchievement);
        
        return addedAchievement == null ? null : UserProfileEntityToDtoMapper.MapToUserAchievementDto(addedAchievement);
    }

    /// <inheritdoc />
    public async Task<UserAchievementDto?> GetUserAchievementByIdAsync(Guid userAchievementId)
    {
        if (userAchievementId == Guid.Empty)
        {
            throw new ArgumentException("User achievement ID cannot be empty.", nameof(userAchievementId));
        }
        
        var userAchievement = await _userAchievementRepository.GetByIdAsync(userAchievementId);

        return userAchievement == null ? null : UserProfileEntityToDtoMapper.MapToUserAchievementDto(userAchievement);
    }

    /// <inheritdoc />
    public async Task<UserAchievementDto?> UpdateUserAchievement(UserAchievementUpdateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateUserAchievementUpdateRequest(request))
        {
            throw new ArgumentException("User achievement request is invalid.", nameof(request));
        }
        
        var userAchievement = await _userAchievementRepository.GetByIdAsync(request.Id);
        if (userAchievement == null)
        {
            throw new InvalidOperationException($"User achievement with ID {request.Id} does not exist.");
        }
        
        userAchievement.AchievedAt = request.AchievedAt;
        userAchievement.UserId = request.UserId;
        userAchievement.AchievementId = request.AchievementId;
        
        var updatedUserAchievement = await _userAchievementRepository.UpdateAsync(userAchievement);
        
        return updatedUserAchievement == null ? null : UserProfileEntityToDtoMapper.MapToUserAchievementDto(updatedUserAchievement);
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
        
        return userAchievements.Select(UserProfileEntityToDtoMapper.MapToUserAchievementDto).ToList();
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
        
        return userAchievements.Select(UserProfileEntityToDtoMapper.MapToUserAchievementDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserAchievementDto>> GetAllAchievementsByUserIdAsync(Guid userId)
    {
        var userAchievements = await _userAchievementRepository.GetAllByUserIdAsync(userId);

        return userAchievements
            .Select(UserProfileEntityToDtoMapper.MapToUserAchievementDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserAchievementDto>> GetNumberOfAchievementsByUserIdAsync(Guid userId, int num)
    {
        var userAchievements = await _userAchievementRepository.GetNumByUserIdAsync(userId, num);
        var userAchievementsList = userAchievements
            .Select(UserProfileEntityToDtoMapper.MapToUserAchievementDto).ToList();

        foreach (var achievement in userAchievementsList)
        {
            achievement.Achievement.LogoUrl = await _fileManagerService.GetPresignedAchievementImageUrlAsync(achievement.Achievement.Id);
        }

        return userAchievementsList;
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
        
        return userAchievements.Select(UserProfileEntityToDtoMapper.MapToUserAchievementDto).ToList();
    }
    
    #endregion
}