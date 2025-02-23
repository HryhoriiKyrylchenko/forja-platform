using Forja.Application.DTOs.UserProfile;

namespace Forja.Application.Interfaces;

public interface IUserProfileService
{
    Task<UserProfileDto> GetUserProfileAsync(string userKeycloakId);
    Task UpdateUserProfileAsync(UserProfileDto userProfileDto);
    Task DeleteUserAsync(string userKeycloakId);
    Task<List<UserProfileDto>> GetAllUsersAsync();
    Task<List<UserProfileDto>> GetAllDeletedUsersAsync();
    
    
    Task AddReviewAsync(string userKeycloakId, ReviewDto reviewDto);
    Task<ReviewDto> GetReviewByIdAsync(Guid reviewId);
    Task UpdateReviewAsync(ReviewDto reviewDto);
    Task DeleteReviewAsync(Guid reviewId);
    Task<ReviewDto> RestoreReviewAsync(Guid reviewId);
    Task<List<ReviewDto>> GetAllUserReviewsAsync(string userKeycloakId);
    Task<List<ReviewDto>> GetAllUserDeletedReviewsAsync(string userKeycloakId);
    Task<List<ReviewDto>> GetAllGameReviewsAsync(Guid gameId);
    Task<List<ReviewDto>> GetAllDeletedGameReviewsAsync(Guid gameId);
    Task<List<ReviewDto>> GetAllReviewsAsync();
    Task<List<ReviewDto>> GetAllNotApprovedReviewsAsync();
    Task<List<ReviewDto>> GetAllDeletedReviewsAsync();
    
    
    
    
    
    // Task AddUserAchievementAsync(Guid userId, AchievementDto achievementDto);
    // Task<IList<GameDto>> GetUserLibraryGamesAsync(Guid userId);

}