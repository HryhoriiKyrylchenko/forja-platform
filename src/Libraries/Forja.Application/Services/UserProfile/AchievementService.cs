namespace Forja.Application.Services.UserProfile;

/// <summary>
/// Service for managing achievements in the application.
/// </summary>
public class AchievementService : IAchievementService
{
    private readonly IAchievementRepository _achievementRepository;

    public AchievementService(IAchievementRepository achievementRepository)
    {
        _achievementRepository = achievementRepository ?? throw new ArgumentNullException(nameof(achievementRepository));
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
}