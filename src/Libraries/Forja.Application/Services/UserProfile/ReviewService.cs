namespace Forja.Application.Services.UserProfile;

/// <summary>
/// The ReviewService class provides methods for managing user reviews within the application.
/// It allows creating, retrieving, updating, and deleting reviews. Additionally, it supports
/// operations for restoring deleted reviews and accessing review data for specific users, games,
/// or globally, both for active and deleted items.
/// </summary>
public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;

    public ReviewService(IReviewRepository reviewRepository, IUserRepository userRepository)
    {
        _reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
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
    
        await _reviewRepository.AddAsync(review);
    }

    /// <inheritdoc />
    public async Task<ReviewDto> GetReviewByIdAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review ID cannot be empty.", nameof(reviewId));
        }

        var review = await _reviewRepository.GetByIdAsync(reviewId);

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

        var existingReview = await _reviewRepository.GetByIdAsync(reviewDto.Id);
        if (existingReview == null)
        {
            throw new InvalidOperationException($"Review with ID {reviewDto.Id} does not exist.");
        }

        existingReview.Rating = reviewDto.Rating;
        existingReview.Comment = reviewDto.Comment;

        await _reviewRepository.UpdateAsync(existingReview);
    }

    /// <inheritdoc />
    public async Task DeleteReviewAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review ID cannot be an empty Guid.", nameof(reviewId));
        }
            
        var review = await _reviewRepository.GetByIdAsync(reviewId);
        if (review == null)
        {
            throw new KeyNotFoundException($"Review with ID '{reviewId}' was not found.");
        }
            
        await _reviewRepository.DeleteAsync(reviewId);
    }

    /// <inheritdoc />
    public async Task<ReviewDto> RestoreReviewAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review ID cannot be an empty Guid.", nameof(reviewId));
        }
        
        var review = await _reviewRepository.RestoreAsync(reviewId);

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
        
        var user = await _userRepository.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var userReviews = await _reviewRepository.GetAllByUserIdAsync(user.Id);

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
        
        var user = await _userRepository.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var userReviews = await _reviewRepository.GetAllDeletedByUserIdAsync(user.Id);

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
        
        var gameReviews = await _reviewRepository.GetAllByGameIdAsync(gameId);

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
        
        var deletedGameReviews = await _reviewRepository.GetAllDeletedByGameIdAsync(gameId);

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
        var allReviews = await _reviewRepository.GetAllAsync();

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
        var notApprovedReviews = await _reviewRepository.GetAllNotApprovedAsync();
        
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
        var allDeletedReviews = await _reviewRepository.GetAllDeletedAsync();

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
}