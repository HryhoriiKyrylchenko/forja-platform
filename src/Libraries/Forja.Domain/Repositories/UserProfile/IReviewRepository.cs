namespace Forja.Domain.Repositories.UserProfile;

/// <summary>
/// Represents a repository for managing Review entities.
/// Provides methods for common operations such as retrieval, addition, update, and deletion of reviews.
/// </summary>
public interface IReviewRepository
{
    /// <summary>
    /// Asynchronously retrieves a review by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the review to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <see cref="Review"/> if found; otherwise, null.
    /// </returns>
    Task<Review?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all reviews associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose reviews are to be retrieved.</param>
    /// <returns>A collection of reviews associated with the specified user.</returns>
    Task<IEnumerable<Review>> GetAllByUserIdAsync(Guid userId);

    /// <summary>
    /// Asynchronously retrieves all deleted reviews associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose deleted reviews are to be retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a collection of deleted reviews associated with the specified user.
    /// </returns>
    Task<IEnumerable<Review>> GetAllDeletedByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves all reviews associated with a specific game.
    /// </summary>
    /// <param name="productId">The unique identifier of the product whose reviews are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of reviews associated with the specified game.</returns>
    Task<IEnumerable<Review>> GetAllByProductIdAsync(Guid productId);

    /// <summary>
    /// Asynchronously retrieves all deleted reviews associated with a specific game.
    /// </summary>
    /// <param name="productId">The unique identifier of the product whose deleted reviews are to be retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a collection of deleted <see cref="Review"/> entities associated with the specified game.
    /// </returns>
    Task<IEnumerable<Review>> GetAllDeletedByProductIdAsync(Guid productId);

    /// <summary>
    /// Asynchronously retrieves the total count of approved positive and negative reviews for a specific product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product for which to retrieve the approved review counts.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a tuple with the total positive and total negative approved review counts.
    /// </returns>
    Task<(int positive, int negative)> GetProductApprovedReviewsCountAsync(Guid productId); 

    /// <summary>
    /// Retrieves all reviews from the data source.
    /// </summary>
    /// <returns>An asynchronous task that returns an enumerable containing all reviews.</returns>
    Task<IEnumerable<Review>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all reviews that have not been approved.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a collection of reviews that are not approved.
    /// </returns>
    Task<IEnumerable<Review>> GetAllNotApprovedAsync();

    /// <summary>
    /// Asynchronously retrieves all deleted reviews.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a collection of deleted <see cref="Review"/> entities.
    /// </returns>
    Task<IEnumerable<Review>> GetAllDeletedAsync();

    /// <summary>
    /// Asynchronously adds a new review to the repository.
    /// </summary>
    /// <param name="review">The review object to be added. Must not be null.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Review?> AddAsync(Review review);

    /// <summary>
    /// Updates an existing review in the repository.
    /// </summary>
    /// <param name="review">The updated review object to be persisted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<Review?> UpdateAsync(Review review);

    /// <summary>
    /// Deletes the review corresponding to the specified identifier.
    /// </summary>
    /// <param name="reviewId">The unique identifier of the review to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(Guid reviewId);

    /// <summary>
    /// Asynchronously restores a soft-deleted review by its unique identifier.
    /// </summary>
    /// <param name="reviewId">The unique identifier of the review to restore.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the restored <see cref="Review"/> if successfully restored.
    /// </returns>
    Task<Review?> RestoreAsync(Guid reviewId);
}