namespace Forja.Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileUnitOfWork _unitOfWork;

    public UserProfileService(IUserProfileUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
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
        
        //TODO: Add parameters validation
        
        user.Username = userProfileDto.Username;
        user.Firstname = userProfileDto.Firstname;
        user.Lastname = userProfileDto.Lastname;
        user.Email = userProfileDto.Email;
        user.PhoneNumber = userProfileDto.PhoneNumber;
        user.AvatarUrl = userProfileDto.AvatarUrl;
        
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

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
        await _unitOfWork.SaveChangesAsync();
    }

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

    public async Task AddReviewAsync(string userKeycloakId, ReviewDto reviewDto)
    {
        var review = new Review
        {
            Id = Guid.NewGuid(),
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

    
    // public async Task AddUserAchievementAsync(Guid userId, AchievementDto achievementDto)
    // {
    //     var achievement = new UserAchievement
    //     {
    //         Id = Guid.NewGuid(),
    //         UserId = userId,
    //         AchievementId = achievementDto.Id // Assuming it's a system-defined achievement
    //     };
    //
    //     await _unitOfWork.UserAchievements.AddAsync(achievement);
    //     await _unitOfWork.SaveChangesAsync();
    // }
    //
    // public async Task<IList<GameDto>> GetUserLibraryGamesAsync(Guid userId)
    // {
    //     var libraryGames = await _unitOfWork.UserLibraryGames.GetByUserAsync(userId);
    //     return libraryGames.Select(g => new GameDto
    //     {
    //         Id = g.GameId,
    //         Name = g.Game.Name,
    //         Description = g.Game.Description
    //     }).ToList();
    // }
}