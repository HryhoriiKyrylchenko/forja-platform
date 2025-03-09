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
    public async Task<ReviewDto?> AddReviewAsync(ReviewCreateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateReviewCreateRequest(request))
        {
            throw new ArgumentException("Review DTO is invalid.", nameof(request));
        }
        
        var review = new Review
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ProductId = request.ProductId,
            PositiveRating = request.PositiveRating,
            Comment = request.Comment,
            CreatedAt = request.CreatedAt,
            IsApproved = false,
            IsDeleted = false
        };
    
        var result = await _reviewRepository.AddAsync(review);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToReviewDto(result);
    }

    /// <inheritdoc />
    public async Task<ReviewDto?> GetReviewByIdAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review ID cannot be empty.", nameof(reviewId));
        }

        var review = await _reviewRepository.GetByIdAsync(reviewId);

        return review == null ? null : UserProfileEntityToDtoMapper.MapToReviewDto(review);
    }

    /// <inheritdoc />
    public async Task<ReviewDto?> UpdateReviewAsync(ReviewUpdateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateReviewUpdateRequest(request))
        {
            throw new ArgumentException("Review DTO is invalid.", nameof(request));
        }

        var existingReview = await _reviewRepository.GetByIdAsync(request.Id);
        if (existingReview == null)
        {
            throw new InvalidOperationException($"Review with ID {request.Id} does not exist.");
        }

        existingReview.UserId = request.UserId;
        existingReview.ProductId = request.ProductId;
        existingReview.PositiveRating = request.PositiveRating;
        existingReview.Comment = request.Comment;

        await _reviewRepository.UpdateAsync(existingReview);
        
        return UserProfileEntityToDtoMapper.MapToReviewDto(existingReview);
    }

    /// <inheritdoc />
    public async Task<ReviewDto?> ApproveReviewAsync(ReviewApproveRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateReviewApproveRequest(request))
        {
            throw new ArgumentException("Review DTO is invalid.", nameof(request));
        }

        var review = await _reviewRepository.GetByIdAsync(request.Id);
        if (review == null)
        {
            throw new InvalidOperationException($"Review with ID {request.Id} does not exist.");
        }
        
        review.IsApproved = true;
        
        await _reviewRepository.UpdateAsync(review);
        
        return UserProfileEntityToDtoMapper.MapToReviewDto(review);
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
    public async Task<ReviewDto?> RestoreReviewAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review ID cannot be an empty Guid.", nameof(reviewId));
        }
        
        var review = await _reviewRepository.RestoreAsync(reviewId);

        return review == null ? null : UserProfileEntityToDtoMapper.MapToReviewDto(review);
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
        
        return userReviewsList.Select(UserProfileEntityToDtoMapper.MapToReviewDto).ToList();
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
        
        return userReviewsList.Select(UserProfileEntityToDtoMapper.MapToReviewDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<ReviewDto>> GetAllProductReviewsAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(productId));
        }
        
        var gameReviews = await _reviewRepository.GetAllByProductIdAsync(productId);

        var gameReviewsList = gameReviews.ToList();
        if (!gameReviewsList.Any())
        {
            return new List<ReviewDto>();
        }
        
        return gameReviewsList.Select(UserProfileEntityToDtoMapper.MapToReviewDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<ReviewDto>> GetAllDeletedProductReviewsAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(productId));
        }
        
        var deletedGameReviews = await _reviewRepository.GetAllDeletedByProductIdAsync(productId);

        var deletedGameReviewsList = deletedGameReviews.ToList();
        if (!deletedGameReviewsList.Any())
        {
            return new List<ReviewDto>();
        }
        
        return deletedGameReviewsList.Select(UserProfileEntityToDtoMapper.MapToReviewDto).ToList();
    }

    /// <inheritdoc />
    public async Task<(int positive, int negative)> GetProductApprovedReviewsCountAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(productId));
        }
        
        return await _reviewRepository.GetProductApprovedReviewsCountAsync(productId);
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
        
        return allReviewsList.Select(UserProfileEntityToDtoMapper.MapToReviewDto).ToList();
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
        
        return notApprovedReviewsList.Select(UserProfileEntityToDtoMapper.MapToReviewDto).ToList();
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
        
        return allDeletedReviewsList.Select(UserProfileEntityToDtoMapper.MapToReviewDto).ToList();
    }
}