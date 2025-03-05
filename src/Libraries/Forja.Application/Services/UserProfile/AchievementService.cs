namespace Forja.Application.Services.UserProfile;

/// <summary>
/// Service for managing achievements in the application.
/// </summary>
public class AchievementService : IAchievementService
{
    private readonly IAchievementRepository _achievementRepository;
    private readonly IUserAchievementRepository _userAchievementRepository;
    private readonly IUserRepository _userRepository;

    public AchievementService(IAchievementRepository achievementRepository, IUserAchievementRepository userAchievementRepository, IUserRepository userRepository)
    {
        _achievementRepository = achievementRepository ?? throw new ArgumentNullException(nameof(achievementRepository));
        _userAchievementRepository = userAchievementRepository ?? throw new ArgumentNullException(nameof(userAchievementRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }
    
    /// <inheritdoc />
    public async Task AddAchievementAsync(string userKeycloakId, AchievementDto achievementDto)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }

        if (!ApplicationDtoValidator.ValidateAchievementDto(achievementDto))
        {
            throw new ArgumentException("Achievement DTO is invalid.", nameof(achievementDto));
        }
        
        var achievement = new Achievement()
        {
            Id = achievementDto.Id,
            GameId = achievementDto.Game.Id,
            Name = achievementDto.Name,
            Description = achievementDto.Description,
            Points = achievementDto.Points,
            LogoUrl = achievementDto.LogoUrl
        };
    
        await _achievementRepository.AddAsync(achievement);
    }

    /// <inheritdoc />
    public async Task<AchievementDto> GetAchievementByIdAsync(Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement ID cannot be empty.", nameof(achievementId));
        }
        
        var achievement = await _achievementRepository.GetByIdAsync(achievementId);
        if (achievement == null)
        {
            throw new KeyNotFoundException($"Achievement with ID '{achievementId}' was not found.");
        }

        return new AchievementDto()
        {
            Id = achievement.Id,
            Name = achievement.Name,
            Description = achievement.Description,
            Points = achievement.Points,
            LogoUrl = achievement.LogoUrl,
            Game = achievement.Game
        };
    }

    /// <inheritdoc />
    public async Task UpdateAchievementAsync(AchievementDto achievementDto)
    {
        if (achievementDto == null)
        {
            throw new ArgumentNullException(nameof(achievementDto), "Achievement cannot be null.");
        }
        
        if (!ApplicationDtoValidator.ValidateAchievementDto(achievementDto))
        {
            throw new ArgumentException("Achievement DTO is invalid.", nameof(achievementDto));
        }

        if (achievementDto.Id == Guid.Empty)
        {
            throw new ArgumentException("Achievement ID must be provided.", nameof(achievementDto.Id));
        }
        
        var existingAchievement = await _achievementRepository.GetByIdAsync(achievementDto.Id);
        if (existingAchievement == null)
        {
            throw new InvalidOperationException($"Achievement with ID {achievementDto.Id} does not exist.");
        }
        
        existingAchievement.Name = achievementDto.Name;
        existingAchievement.Description = achievementDto.Description;
        existingAchievement.Points = achievementDto.Points;
        existingAchievement.LogoUrl = achievementDto.LogoUrl;
        existingAchievement.GameId = achievementDto.Game.Id;
        
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
    public async Task<AchievementDto> RestoreAchievementAsync(Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement ID cannot be an empty Guid.", nameof(achievementId));
        }
        
        var achievement = await _achievementRepository.RestoreAsync(achievementId);
        
        return new AchievementDto()
        {
            Id = achievement.Id,
            Name = achievement.Name,
            Description = achievement.Description,
            Points = achievement.Points,
            LogoUrl = achievement.LogoUrl,
            Game = achievement.Game
        };
    }

    /// <inheritdoc />
    public async Task<List<AchievementDto>> GetAllGameAchievementsAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var gameAchievements = await _achievementRepository.GetAllByGameIdAsync(gameId);

        var achievements = gameAchievements.ToList();
        if (!achievements.Any())
        {
            return new List<AchievementDto>();
        }
        
        return achievements.Select(achievement => new AchievementDto()
            {
                Id = achievement.Id,
                Name = achievement.Name,
                Description = achievement.Description,
                Points = achievement.Points,
                LogoUrl = achievement.LogoUrl,
                Game = achievement.Game
            }
        ).ToList();
    }

    /// <inheritdoc />
    public async Task<List<AchievementDto>> GetAllGameDeletedAchievementsAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var deletedGameAchievements = await _achievementRepository.GetAllDeletedByGameIdAsync(gameId);

        return deletedGameAchievements.Select(achievement => new AchievementDto()
            {
                Id = achievement.Id,
                Name = achievement.Name,
                Description = achievement.Description,
                Points = achievement.Points,
                LogoUrl = achievement.LogoUrl,
                Game = achievement.Game
            }
        ).ToList();
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
        
        return achievements.Select(achievement => new AchievementDto()
        {
            Id = achievement.Id,
            Name = achievement.Name,
            Description = achievement.Description,
            Points = achievement.Points,
            LogoUrl = achievement.LogoUrl,
            Game = achievement.Game
        }).ToList();
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
        
        return achievements.Select(achievement => new AchievementDto()
        {
            Id = achievement.Id,
            Name = achievement.Name,
            Description = achievement.Description,
            Points = achievement.Points,
            LogoUrl = achievement.LogoUrl,
            Game = achievement.Game
        }).ToList();
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