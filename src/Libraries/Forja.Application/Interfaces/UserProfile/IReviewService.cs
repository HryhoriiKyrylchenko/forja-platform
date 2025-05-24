namespace Forja.Application.Interfaces.UserProfile;

/// <summary>
/// Defines methods for managing and interacting with user reviews.
/// </summary>
public interface IReviewService
{
    /// <summary>
    /// Adds a new review for the specified user.
    /// </summary>
    /// <param name="request">The ReviewCreateRequest object containing the details of the review to be added.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task<ReviewDto?> AddReviewAsync(ReviewCreateRequest request);

    /// <summary>
    /// Retrieves the review associated with the specified review ID.
    /// </summary>
    /// <param name="reviewId">The unique identifier of the review to retrieve.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains the ReviewDto associated with the specified review ID.</returns>
    Task<ReviewDto?> GetReviewByIdAsync(Guid reviewId);

    /// <summary>
    /// Updates the review based on the provided review details.
    /// </summary>
    /// <param name="request">The ReviewUpdateRequest object containing the review details to be updated, such as rating, user ID, and game ID.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task<ReviewDto?> UpdateReviewAsync(ReviewUpdateRequest request);

    /// <summary>
    /// Approves or rejects a review based on the details provided in the request.
    /// </summary>
    /// <param name="request">The ReviewApproveRequest object containing the ID of the review and the approval status.</param>
    /// <returns>A Task representing the asynchronous operation that returns the updated ReviewDto.</returns>
    Task<ReviewDto?> ApproveReviewAsync(ReviewApproveRequest request);

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
    Task<ReviewDto?> RestoreReviewAsync(Guid reviewId);

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
    /// <param name="productId">The unique identifier of the product to retrieve reviews for.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects associated with the specified game.</returns>
    Task<List<ReviewDto>> GetAllProductReviewsAsync(Guid productId);

    /// <summary>
    /// Retrieves all deleted game reviews associated with the specified game ID.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to retrieve deleted reviews for.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result contains a list of ReviewDto objects representing the deleted reviews for the specified game.</returns>
    Task<List<ReviewDto>> GetAllDeletedProductReviewsAsync(Guid productId);


    /// <summary>
    /// Retrieves all reviews for a specified product, including user information for each review.
    /// </summary>
    /// <param name="productId">The unique identifier of the product whose reviews are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of ReviewExtendedDto objects with detailed review and user information.</returns>
    Task<List<ReviewExtendedDto>> GetAllProductReviewsWithUserInfoAsync(Guid productId);

    /// <summary>
    /// Retrieves the count of approved positive and negative reviews for a specified product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to retrieve approved reviews for.</param>
    /// <returns>A Task representing the result of the asynchronous operation. The task result is a tuple containing the count of positive and negative approved reviews (int positive, int negative).</returns>
    Task<(int positive, int negative)> GetProductApprovedReviewsCountAsync(Guid productId); 

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