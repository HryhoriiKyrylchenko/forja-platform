namespace Forja.Application.Interfaces.UserProfile;

/// <summary>
/// Defines methods for managing and interacting with user reviews.
/// </summary>
public interface IReviewService
{
    /// <summary>
    /// Adds a new review for the specified user.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user adding the review.</param>
    /// <param name="reviewDto">The ReviewDto object containing the details of the review to be added.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task AddReviewAsync(string userKeycloakId, ReviewDto reviewDto);

    /// <summary>
    /// Retrieves the review associated with the specified review ID.
    /// </summary>
    /// <param name="reviewId">The unique identifier of the review to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the ReviewDto associated with the specified review ID.</returns>
    Task<ReviewDto> GetReviewByIdAsync(Guid reviewId);

    /// <summary>
    /// Updates the review based on the provided review details.
    /// </summary>
    /// <param name="reviewDto">The ReviewDto object containing the review details to be updated, such as rating, user ID, and game ID.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task UpdateReviewAsync(ReviewDto reviewDto);

    /// <summary>
    /// Deletes the review associated with the specified review ID.
    /// </summary>
    /// <param name="reviewId">The unique identifier of the review to be deleted.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteReviewAsync(Guid reviewId);

    /// <summary>
    /// Restores a previously deleted review identified by the given review ID.
    /// </summary>
    /// <param name="reviewId">The unique identifier of the review to restore.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the restored ReviewDto associated with the specified review ID.</returns>
    Task<ReviewDto> RestoreReviewAsync(Guid reviewId);

    /// <summary>
    /// Retrieves all reviews associated with the specified user's Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user whose reviews are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing the reviews associated with the specified user.</returns>
    Task<List<ReviewDto>> GetAllUserReviewsAsync(string userKeycloakId);

    /// <summary>
    /// Retrieves all deleted reviews associated with the specified user's Keycloak ID.
    /// </summary>
    /// <param name="userKeycloakId">The unique Keycloak ID of the user whose deleted reviews are to be retrieved.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing the deleted reviews associated with the specified Keycloak ID.</returns>
    Task<List<ReviewDto>> GetAllUserDeletedReviewsAsync(string userKeycloakId);

    /// <summary>
    /// Retrieves all reviews associated with a specified game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game to retrieve reviews for.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects associated with the specified game.</returns>
    Task<List<ReviewDto>> GetAllGameReviewsAsync(Guid gameId);

    /// <summary>
    /// Retrieves all deleted game reviews associated with the specified game ID.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game to retrieve deleted reviews for.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing the deleted reviews for the specified game.</returns>
    Task<List<ReviewDto>> GetAllDeletedGameReviewsAsync(Guid gameId);

    /// <summary>
    /// Retrieves all reviews present in the system.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing all reviews.</returns>
    Task<List<ReviewDto>> GetAllReviewsAsync();

    /// <summary>
    /// Retrieves all reviews that have not been approved.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing the reviews that are not approved.</returns>
    Task<List<ReviewDto>> GetAllNotApprovedReviewsAsync();

    /// <summary>
    /// Retrieves a list of all deleted reviews.
    /// </summary>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing the deleted reviews.</returns>
    Task<List<ReviewDto>> GetAllDeletedReviewsAsync();
}