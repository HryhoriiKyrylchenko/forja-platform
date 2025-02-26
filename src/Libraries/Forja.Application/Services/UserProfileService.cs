namespace Forja.Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileUnitOfWork _unitOfWork;
    private readonly IKeycloakClient _keycloakClient;

    public UserProfileService(IUserProfileUnitOfWork unitOfWork, IKeycloakClient keycloakClient)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _keycloakClient = keycloakClient ?? throw new ArgumentNullException(nameof(keycloakClient));
    }
    
    /// <inheritdoc />
    public async Task<UserProfileDto> GetUserProfileAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _unitOfWork.Users.GetByKeycloakIdAsync(userKeycloakId);
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
            AvatarUrl = user.AvatarUrl
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
        
        var user = await _unitOfWork.Users.GetByIdAsync(userProfileDto.Id);
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
        
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteUserAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _unitOfWork.Users.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        await _unitOfWork.Users.DeleteAsync(user.Id);
        await _keycloakClient.EnableDisableUserAsync(userKeycloakId, false);
        
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task RestoreUserAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _unitOfWork.Users.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        await _unitOfWork.Users.RestoreAsync(user.Id);
        await _keycloakClient.EnableDisableUserAsync(userKeycloakId, true);
        
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<List<UserProfileDto>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();

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
            AvatarUrl = user.AvatarUrl
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserProfileDto>> GetAllDeletedUsersAsync()
    {
        var deletedUsers = await _unitOfWork.Users.GetAllDeletedAsync();

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
            AvatarUrl = user.AvatarUrl
        }).ToList();
    }

    /// <inheritdoc />
    public async Task AddReviewAsync(string userKeycloakId, ReviewDto reviewDto)
    {
        var review = new Review
        {
            Id = reviewDto.Id,
            UserId = reviewDto.UserId,
            GameId = reviewDto.GameId,
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    
        await _unitOfWork.Reviews.AddAsync(review);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<ReviewDto> GetReviewByIdAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review ID cannot be empty.", nameof(reviewId));
        }

        var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);

        if (review == null)
        {
            throw new KeyNotFoundException($"Review with ID '{reviewId}' was not found.");
        }

        return new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            GameId = review.GameId,
            Rating = review.Rating,
            Comment = review.Comment
        };
    }

    /// <inheritdoc />
    public async Task UpdateReviewAsync(ReviewDto reviewDto)
    {
        if (reviewDto == null)
        {
            throw new ArgumentNullException(nameof(reviewDto), "Review cannot be null.");
        }

        if (reviewDto.Id == Guid.Empty)
        {
            throw new ArgumentException("Review ID must be provided.", nameof(reviewDto.Id));
        }

        if (reviewDto.Rating < 1 || reviewDto.Rating > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(reviewDto.Rating), "Review rating must be between 1 and 5.");
        }

        var existingReview = await _unitOfWork.Reviews.GetByIdAsync(reviewDto.Id);
        if (existingReview == null)
        {
            throw new InvalidOperationException($"Review with ID {reviewDto.Id} does not exist.");
        }

        existingReview.Rating = reviewDto.Rating;
        existingReview.Comment = reviewDto.Comment;

        await _unitOfWork.Reviews.UpdateAsync(existingReview);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteReviewAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review ID cannot be an empty Guid.", nameof(reviewId));
        }
            
        var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);
        if (review == null)
        {
            throw new KeyNotFoundException($"Review with ID '{reviewId}' was not found.");
        }
            
        await _unitOfWork.Reviews.DeleteAsync(reviewId);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<ReviewDto> RestoreReviewAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review ID cannot be an empty Guid.", nameof(reviewId));
        }
        
        var review = await _unitOfWork.Reviews.RestoreAsync(reviewId);
        await _unitOfWork.SaveChangesAsync();

        return new ReviewDto()
        {
            Id = review.Id,
            UserId = review.UserId,
            GameId = review.GameId,
            Rating = review.Rating,
            Comment = review.Comment
        };
    }

    /// <inheritdoc />
    public async Task<List<ReviewDto>> GetAllUserReviewsAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _unitOfWork.Users.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var userReviews = await _unitOfWork.Reviews.GetAllByUserIdAsync(user.Id);

        var userReviewsList = userReviews.ToList();
        if (!userReviewsList.Any())
        {
            return new List<ReviewDto>();
        }
        
        return userReviewsList.Select(review => new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            GameId = review.GameId,
            Rating = review.Rating,
            Comment = review.Comment
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<ReviewDto>> GetAllUserDeletedReviewsAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _unitOfWork.Users.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var userReviews = await _unitOfWork.Reviews.GetAllDeletedByUserIdAsync(user.Id);

        var userReviewsList = userReviews.ToList();
        if (!userReviewsList.Any())
        {
            return new List<ReviewDto>();
        }
        
        return userReviewsList.Select(review => new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            GameId = review.GameId,
            Rating = review.Rating,
            Comment = review.Comment
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<ReviewDto>> GetAllGameReviewsAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var gameReviews = await _unitOfWork.Reviews.GetAllByGameIdAsync(gameId);

        var gameReviewsList = gameReviews.ToList();
        if (!gameReviewsList.Any())
        {
            return new List<ReviewDto>();
        }
        
        return gameReviewsList.Select(review => new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            GameId = review.GameId,
            Rating = review.Rating,
            Comment = review.Comment
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<ReviewDto>> GetAllDeletedGameReviewsAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var deletedGameReviews = await _unitOfWork.Reviews.GetAllDeletedByGameIdAsync(gameId);

        var deletedGameReviewsList = deletedGameReviews.ToList();
        if (!deletedGameReviewsList.Any())
        {
            return new List<ReviewDto>();
        }
        
        return deletedGameReviewsList.Select(review => new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            GameId = review.GameId,
            Rating = review.Rating,
            Comment = review.Comment
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<ReviewDto>> GetAllReviewsAsync()
    {
        var allReviews = await _unitOfWork.Reviews.GetAllAsync();

        var allReviewsList = allReviews.ToList();
        if (!allReviewsList.Any())
        {
            return new List<ReviewDto>();
        }
        
        return allReviewsList.Select(review => new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            GameId = review.GameId,
            Rating = review.Rating,
            Comment = review.Comment
        }).ToList();
    }
    
    /// <inheritdoc />
    public async Task<List<ReviewDto>> GetAllNotApprovedReviewsAsync()
    {
        var notApprovedReviews = await _unitOfWork.Reviews.GetAllNotApprovedAsync();
        
        var notApprovedReviewsList = notApprovedReviews.ToList();
        if (!notApprovedReviewsList.Any())
        {
            return new List<ReviewDto>();
        }
        
        return notApprovedReviewsList.Select(review => new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            GameId = review.GameId,
            Rating = review.Rating,
            Comment = review.Comment
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<ReviewDto>> GetAllDeletedReviewsAsync()
    {
        var allDeletedReviews = await _unitOfWork.Reviews.GetAllDeletedAsync();

        var allDeletedReviewsList = allDeletedReviews.ToList();
        if (!allDeletedReviewsList.Any())
        {
            return new List<ReviewDto>();
        }
        
        return allDeletedReviewsList.Select(review => new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            GameId = review.GameId,
            Rating = review.Rating,
            Comment = review.Comment
        }).ToList();
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
    
        await _unitOfWork.Achievements.AddAsync(achievement);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<AchievementDto> GetAchievementByIdAsync(Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement ID cannot be empty.", nameof(achievementId));
        }
        
        var achievement = await _unitOfWork.Achievements.GetByIdAsync(achievementId);
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
        
        var existingAchievement = await _unitOfWork.Achievements.GetByIdAsync(achievementDto.Id);
        if (existingAchievement == null)
        {
            throw new InvalidOperationException($"Achievement with ID {achievementDto.Id} does not exist.");
        }
        
        existingAchievement.Name = achievementDto.Name;
        existingAchievement.Description = achievementDto.Description;
        existingAchievement.Points = achievementDto.Points;
        existingAchievement.LogoUrl = achievementDto.LogoUrl;
        existingAchievement.GameId = achievementDto.Game.Id;
        
        await _unitOfWork.Achievements.UpdateAsync(existingAchievement);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAchievementAsync(Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement ID cannot be an empty Guid.", nameof(achievementId));
        }
        
        var achievement = await _unitOfWork.Achievements.GetByIdAsync(achievementId);
        if (achievement == null)
        {
            throw new KeyNotFoundException($"Achievement with ID '{achievementId}' was not found.");
        }
        
        await _unitOfWork.Achievements.DeleteAsync(achievementId);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<AchievementDto> RestoreAchievementAsync(Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement ID cannot be an empty Guid.", nameof(achievementId));
        }
        
        var achievement = await _unitOfWork.Achievements.RestoreAsync(achievementId);
        await _unitOfWork.SaveChangesAsync();
        
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
        
        var gameAchievements = await _unitOfWork.Achievements.GetAllByGameIdAsync(gameId);

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
        
        var deletedGameAchievements = await _unitOfWork.Achievements.GetAllDeletedByGameIdAsync(gameId);

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
        var allAchievements = await _unitOfWork.Achievements.GetAllAsync();

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
        var allDeletedAchievements = await _unitOfWork.Achievements.GetAllDeletedAsync();

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
        
        await _unitOfWork.UserAchievements.AddAsync(userAchievement);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<UserAchievementDto> GetUserAchievementByIdAsync(Guid userAchievementId)
    {
        if (userAchievementId == Guid.Empty)
        {
            throw new ArgumentException("User achievement ID cannot be empty.", nameof(userAchievementId));
        }
        
        var userAchievement = await _unitOfWork.UserAchievements.GetByIdAsync(userAchievementId);
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
        
        var userAchievement = await _unitOfWork.UserAchievements.GetByIdAsync(userAchievementDto.Id);
        if (userAchievement == null)
        {
            throw new InvalidOperationException($"User achievement with ID {userAchievementDto.Id} does not exist.");
        }
        
        userAchievement.AchievedAt = userAchievementDto.AchievedAt;
        userAchievement.UserId = userAchievementDto.User.Id;
        userAchievement.AchievementId = userAchievementDto.Achievement.Id;
        
        await _unitOfWork.UserAchievements.UpdateAsync(userAchievement);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteUserAchievementAsync(Guid userAchievementId)
    {
        if (userAchievementId == Guid.Empty)
        {
            throw new ArgumentException("User achievement ID cannot be an empty Guid.", nameof(userAchievementId));
        }
        
        var userAchievement = await _unitOfWork.UserAchievements.GetByIdAsync(userAchievementId);
        if (userAchievement == null)
        {
            throw new KeyNotFoundException($"User achievement with ID '{userAchievementId}' was not found.");
        }
        
        await _unitOfWork.UserAchievements.DeleteAsync(userAchievementId);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<List<UserAchievementDto>> GetAllUserAchievementsAsync()
    {
        var allUserAchievements = await _unitOfWork.UserAchievements.GetAllAsync();

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
        
        var user = await _unitOfWork.Users.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var allUserAchievements = await _unitOfWork.UserAchievements.GetAllByUserIdAsync(user.Id);
        
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
        
        var allUserAchievements = await _unitOfWork.UserAchievements.GetAllByGameIdAsync(gameId);
        
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
    public async Task AddUserLibraryGameAsync(UserLibraryGameDto userLibraryGameDto)
    {
        if (userLibraryGameDto == null)
        {
            throw new ArgumentNullException(nameof(userLibraryGameDto), "User library game cannot be null.");
        }

        if (!ApplicationDtoValidator.ValidateUserLibraryGameDto(userLibraryGameDto))
        {
            throw new ArgumentException("User library game DTO is invalid.", nameof(userLibraryGameDto));
        }
        
        var userLibraryGame = new UserLibraryGame()
        {
            Id = userLibraryGameDto.Id,
            UserId = userLibraryGameDto.User.Id,
            GameId = userLibraryGameDto.Game.Id,
            PurchaseDate = userLibraryGameDto.PurchaseDate
        };
        
        await _unitOfWork.UserLibraryGames.AddAsync(userLibraryGame);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateUserLibraryGameAsync(UserLibraryGameDto userLibraryGameDto)
    {
        if (userLibraryGameDto == null)
        {
            throw new ArgumentNullException(nameof(userLibraryGameDto), "User library game cannot be null.");
        }
        
        if (!ApplicationDtoValidator.ValidateUserLibraryGameDto(userLibraryGameDto))
        {
            throw new ArgumentException("User library game DTO is invalid.", nameof(userLibraryGameDto));
        }
        
        var userLibraryGame = await _unitOfWork.UserLibraryGames.GetByIdAsync(userLibraryGameDto.Id);
        if (userLibraryGame == null)
        {
            throw new InvalidOperationException($"User library game with ID {userLibraryGameDto.Id} does not exist.");
        }
        
        userLibraryGame.PurchaseDate = userLibraryGameDto.PurchaseDate;
        userLibraryGame.UserId = userLibraryGameDto.User.Id;
        userLibraryGame.GameId = userLibraryGameDto.Game.Id;
        
        await _unitOfWork.UserLibraryGames.UpdateAsync(userLibraryGame);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteUserLibraryGameAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            
        }
        
        var userLibraryGame = await _unitOfWork.UserLibraryGames.GetByIdAsync(userLibraryGameId);
        if (userLibraryGame == null)
        {
            throw new KeyNotFoundException($"User library game with ID '{userLibraryGameId}' was not found.");
        }
        
        await _unitOfWork.UserLibraryGames.DeleteAsync(userLibraryGameId);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto> RestoreUserLibraryGameAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game ID cannot be an empty Guid.", nameof(userLibraryGameId));
        }
        
        var userLibraryGame = await _unitOfWork.UserLibraryGames.RestoreAsync(userLibraryGameId);
        await _unitOfWork.SaveChangesAsync();

        return new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
        };
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto> GetUserLibraryGameByIdAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game ID cannot be an empty Guid.", nameof(userLibraryGameId));
        }
        
        var userLibraryGame = await _unitOfWork.UserLibraryGames.GetByIdAsync(userLibraryGameId);
        if (userLibraryGame == null)
        {
            throw new KeyNotFoundException($"User library game with ID '{userLibraryGameId}' was not found.");
        }
        
        return new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
            PurchaseDate = userLibraryGame.PurchaseDate
        };
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto> GetDeletedUserLibraryGameByIdAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game ID cannot be an empty Guid.", nameof(userLibraryGameId));
        }
        
        var deletedUserLibraryGame = await _unitOfWork.UserLibraryGames.GetDeletedByIdAsync(userLibraryGameId);
        if (deletedUserLibraryGame == null)
        {
            throw new KeyNotFoundException($"User library game with ID '{userLibraryGameId}' was not found.");
        }
        
        return new UserLibraryGameDto()
        {
            Id = deletedUserLibraryGame.Id,
            User = deletedUserLibraryGame.User,
            Game = deletedUserLibraryGame.Game,
            PurchaseDate = deletedUserLibraryGame.PurchaseDate
        };
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameDto>> GetAllUserLibraryGamesAsync()
    {
        var allUserLibraryGames = await _unitOfWork.UserLibraryGames.GetAllAsync();

        var userLibraryGames = allUserLibraryGames.ToList();
        if (!userLibraryGames.Any())
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        
        return userLibraryGames.Select(userLibraryGame => new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
            PurchaseDate = userLibraryGame.PurchaseDate
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameDto>> GetAllDeletedUserLibraryGamesAsync()
    {
        var allDeletedUserLibraryGames = await _unitOfWork.UserLibraryGames.GetAllDeletedAsync();

        var userLibraryGames = allDeletedUserLibraryGames.ToList();
        if (!userLibraryGames.Any())
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        
        return userLibraryGames.Select(userLibraryGame => new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
            PurchaseDate = userLibraryGame.PurchaseDate
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameDto>> GetAllUserLibraryGamesByUserKeycloakIdAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _unitOfWork.Users.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        var allUserLibraryGames = await _unitOfWork.UserLibraryGames.GetAllByUserIdAsync(user.Id);
        
        var userLibraryGames = allUserLibraryGames.ToList();
        if (!userLibraryGames.Any())
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        
        return userLibraryGames.Select(userLibraryGame => new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
            PurchaseDate = userLibraryGame.PurchaseDate
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameDto>> GetAllDeletedUserLibraryGamesByUserKeycloakIdAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _unitOfWork.Users.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        var allDeletedUserLibraryGames = await _unitOfWork.UserLibraryGames.GetAllDeletedByUserIdAsync(user.Id);
        
        var userLibraryGames = allDeletedUserLibraryGames.ToList();
        if (!userLibraryGames.Any())
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        
        return userLibraryGames.Select(userLibraryGame => new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
            PurchaseDate = userLibraryGame.PurchaseDate
        }).ToList();
    }

    /// <inheritdoc />
    public async Task AddUserLibraryAddonAsync(UserLibraryAddonDto userLibraryAddonDto)
    {
        if (!ApplicationDtoValidator.ValidateUserLibraryAddonDto(userLibraryAddonDto))
        {
            throw new ArgumentException("User library addon DTO is invalid.", nameof(userLibraryAddonDto));
        }
        
        var userLibraryAddon = new UserLibraryAddon()
        {
            Id = userLibraryAddonDto.Id,
            UserLibraryGameId = userLibraryAddonDto.UserLibraryGame.Id,
            AddonId = userLibraryAddonDto.GameAddon.Id,
            PurchaseDate = userLibraryAddonDto.PurchaseDate
        };
        
        await _unitOfWork.UserLibraryAddons.AddAsync(userLibraryAddon);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateUserLibraryAddonAsync(UserLibraryAddonDto userLibraryAddonDto)
    {
        if (!ApplicationDtoValidator.ValidateUserLibraryAddonDto(userLibraryAddonDto))
        {
            throw new ArgumentException("User library addon DTO is invalid.", nameof(userLibraryAddonDto));
        }

        var userLibraryAddon = await _unitOfWork.UserLibraryAddons.GetByIdAsync(userLibraryAddonDto.Id);
        if (userLibraryAddon == null)
        {
            throw new InvalidOperationException($"User library addon with ID {userLibraryAddonDto.Id} does not exist.");
        }
        
        userLibraryAddon.PurchaseDate = userLibraryAddonDto.PurchaseDate;
        userLibraryAddon.UserLibraryGameId = userLibraryAddonDto.UserLibraryGame.Id;
        userLibraryAddon.AddonId = userLibraryAddonDto.GameAddon.Id;
        
        await _unitOfWork.UserLibraryAddons.UpdateAsync(userLibraryAddon);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteUserLibraryAddonAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var userLibraryAddon = await _unitOfWork.UserLibraryAddons.GetByIdAsync(userLibraryAddonId);
        if (userLibraryAddon == null)
        {
            throw new KeyNotFoundException($"User library addon with ID '{userLibraryAddonId}' was not found.");
        }
        
        await _unitOfWork.UserLibraryAddons.DeleteAsync(userLibraryAddonId);
        await _unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto> RestoreUserLibraryAddonAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var userLibraryAddon = await _unitOfWork.UserLibraryAddons.RestoreAsync(userLibraryAddonId);
        await _unitOfWork.SaveChangesAsync();
        
        return new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        };
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto> GetUserLibraryAddonByIdAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var userLibraryAddon = await _unitOfWork.UserLibraryAddons.GetByIdAsync(userLibraryAddonId);
        if (userLibraryAddon == null)
        {
            throw new KeyNotFoundException($"User library addon with ID '{userLibraryAddonId}' was not found.");
        }
        
        return new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        };
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto> GetDeletedUserLibraryAddonByIdAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var deletedUserLibraryAddon = await _unitOfWork.UserLibraryAddons.GetDeletedByIdAsync(userLibraryAddonId);
        if (deletedUserLibraryAddon == null)
        {
            throw new KeyNotFoundException($"User library addon with ID '{userLibraryAddonId}' was not found.");
        }
        
        return new UserLibraryAddonDto()
        {
            Id = deletedUserLibraryAddon.Id,
            UserLibraryGame = deletedUserLibraryAddon.UserLibraryGame,
            GameAddon = deletedUserLibraryAddon.GameAddon,
            PurchaseDate = deletedUserLibraryAddon.PurchaseDate
        };
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllUserLibraryAddonsAsync()
    {
        var allUserLibraryAddons = await _unitOfWork.UserLibraryAddons.GetAllAsync();
        
        var userLibraryAddons = allUserLibraryAddons.ToList();
        if (!userLibraryAddons.Any())
        {
            throw new KeyNotFoundException("No user library addons were found.");
        }

        return userLibraryAddons.Select(userLibraryAddon => new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllDeletedUserLibraryAddonsAsync()
    {
        var allDeletedUserLibraryAddons = await _unitOfWork.UserLibraryAddons.GetAllDeletedAsync();
        
        var userLibraryAddons = allDeletedUserLibraryAddons.ToList();
        if (!userLibraryAddons.Any())
        {
            throw new KeyNotFoundException("No user library addons were found.");
        }

        return userLibraryAddons.Select(userLibraryAddon => new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllUserLibraryAddonsByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var allUserLibraryAddons = await _unitOfWork.UserLibraryAddons.GetAllByGameIdAsync(gameId);
        
        var userLibraryAddons = allUserLibraryAddons.ToList();
        if (!userLibraryAddons.Any())
        {
            throw new KeyNotFoundException("No user library addons were found.");
        }
        
        return userLibraryAddons.Select(userLibraryAddon => new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllDeletedUserLibraryAddonsByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var allDeletedUserLibraryAddons = await _unitOfWork.UserLibraryAddons.GetAllDeletedByGameIdAsync(gameId);
        
        var userLibraryAddons = allDeletedUserLibraryAddons.ToList();
        if (!userLibraryAddons.Any())
        {
            throw new KeyNotFoundException("No user library addons were found.");
        }
        
        return userLibraryAddons.Select(userLibraryAddon => new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        }).ToList();
    }
}