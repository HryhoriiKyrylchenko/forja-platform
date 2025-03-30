namespace Forja.API.Controllers.UserProfile;

/// <summary>
/// Provides endpoints for managing reviews, including creation, retrieval, update, deletion, restoration,
/// and fetching reviews associated with users or products.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IAnalyticsEventService _analyticsEventService;
    private readonly IAuditLogService _auditLogService;

    public ReviewController(ReviewService reviewService,
        IAnalyticsEventService analyticsEventService,
        IAuditLogService auditLogService)
    {
        _reviewService = reviewService;
        _analyticsEventService = analyticsEventService;
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Adds a new review for a product by a user.
    /// </summary>
    /// <param name="request">An object containing the details of the review to be created, including user ID, product ID, rating, and comment.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created review as a <see cref="ReviewDto"/> object.</returns>
    /// <response code="200">Returns the created review details.</response>
    /// <response code="400">Returns if the input data is invalid or an error occurs during processing.</response>
    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<ActionResult<ReviewDto>> AddReview([FromBody] ReviewCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _reviewService.AddReviewAsync(request);

            if (result == null)
            {
                return BadRequest(new { error = "Failed to create review." });
            }
            
            try
            {
                await _analyticsEventService.AddEventAsync(AnalyticEventType.ReviewSubmitted,
                    result.UserId,
                    new Dictionary<string, string>
                {
                    { "ReviewId", result.Id.ToString() },
                    { "ProductId", request.ProductId.ToString() },
                    { "UserId", request.UserId.ToString() },
                    { "Rating", request.PositiveRating.ToString() },
                    { "Comment", request.Comment },
                    { "CreatedAt", result.CreatedAt.ToString(CultureInfo.InvariantCulture) }
                });
            }
            catch (Exception)
            {
                Console.WriteLine("Analytics event failed to create");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Create,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to create review for product with id: {request.ProductId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
           return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a review by its unique identifier.
    /// </summary>
    /// <param name="reviewId">
    /// The unique identifier of the review to retrieve. Must be a non-empty Guid.
    /// </param>
    /// <returns>
    /// An <see cref="ActionResult"/> containing the review data as <see cref="ReviewDto"/> if found,
    /// or an appropriate HTTP response if the review is not found or an error occurs.
    /// </returns>
    [HttpGet("{reviewId}")]
    public async Task<ActionResult<ReviewDto>> GetReviewById([FromRoute] Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _reviewService.GetReviewByIdAsync(reviewId);
            if (result == null)
            {
                return NotFound(new { error = $"Review with ID {reviewId} not found." });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Create,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get review with id: {reviewId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing review with the provided information.
    /// </summary>
    /// <param name="request">The review update request containing the updated details of the review.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated review details as a <see cref="ReviewDto"/> object, or a bad request result if the operation fails.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpPut]
    public async Task<ActionResult<ReviewDto>> UpdateReview([FromBody] ReviewUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _reviewService.UpdateReviewAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update review with id: {request.Id}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Approves or rejects a specified review based on the provided request data.
    /// </summary>
    /// <param name="request">The data required to approve or reject the review, including its identifier and approval status.</param>
    /// <returns>
    /// Returns the updated review as <see cref="ReviewDto"/> upon successful approval or rejection.
    /// If the operation fails, an appropriate error response is returned.
    /// </returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpPut("approve")]
    public async Task<ActionResult<ReviewDto>> ApproveReview([FromBody] ReviewApproveRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _reviewService.ApproveReviewAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to approve review with id: {request.Id}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a review by its ID.
    /// </summary>
    /// <param name="reviewId">
    /// The unique identifier of the review to be deleted.
    /// </param>
    /// <returns>
    /// An IActionResult indicating the result of the operation. Returns NoContent on successful deletion or BadRequest in case of an error.
    /// </returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReview([FromRoute] Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            await _reviewService.DeleteReviewAsync(reviewId);
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Delete,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete review with id: {reviewId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Restores a previously deleted review based on the given review ID.
    /// </summary>
    /// <param name="reviewId">The unique identifier of the review to be restored.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the restored review as a <see cref="ReviewDto"/> object, or a BadRequest response if the operation fails.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpPost("{reviewId}/restore")]
    public async Task<ActionResult<ReviewDto>> RestoreReview([FromRoute] Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _reviewService.RestoreReviewAsync(reviewId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to restore review with id: {reviewId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all reviews created by a specific user identified by their Keycloak ID.
    /// </summary>
    /// <param name="keycloakId">The unique identifier of the user from Keycloak.</param>
    /// <returns>A list of <see cref="ReviewDto"/> objects containing all reviews associated with the specified user, or an error response if the operation fails.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpGet("{keycloakId}/all")]
    public async Task<ActionResult<List<ReviewDto>>> GetAllUserReviews([FromRoute] string keycloakId)
    {
        if (string.IsNullOrEmpty(keycloakId))
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _reviewService.GetAllUserReviewsAsync(keycloakId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all user reviews" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all deleted reviews for a specific user based on their Keycloak ID.
    /// </summary>
    /// <param name="keycloakId">The Keycloak ID of the user whose deleted reviews are being retrieved.</param>
    /// <returns>A list of deleted reviews associated with the specified user, wrapped in an <see cref="ActionResult"/>.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpGet("{keycloakId}/deleted")]
    public async Task<ActionResult<List<ReviewDto>>> GetAllUserDeletedReviews([FromRoute] string keycloakId)
    {
        if (string.IsNullOrEmpty(keycloakId))
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _reviewService.GetAllUserDeletedReviewsAsync(keycloakId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all deleted user reviews" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all reviews associated with a specific product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product for which to retrieve the reviews.</param>
    /// <returns>A list of <see cref="ReviewDto"/> objects representing the reviews of the specified product.</returns>
    [HttpGet("products/{productId}")]
    public async Task<ActionResult<List<ReviewDto>>> GetAllProductReviews([FromRoute] Guid productId)
    {
        if (productId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _reviewService.GetAllProductReviewsAsync(productId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all product reviews for product with id: {productId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all deleted reviews for a specific product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product for which the deleted reviews are to be retrieved.</param>
    /// <returns>A task representing an asynchronous operation that returns a list of <see cref="ReviewDto"/> objects representing the deleted reviews of the specified product.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpGet("products/{productId}/deleted")]
    public async Task<ActionResult<List<ReviewDto>>> GetAllDeletedProductReviews([FromRoute] Guid productId)
    {
        if (productId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _reviewService.GetAllDeletedProductReviewsAsync(productId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all deleted product reviews for product with id: {productId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves the count of approved product reviews, categorized as positive and negative.
    /// </summary>
    /// <param name="productId">The unique identifier of the product for which the approved reviews are being counted.</param>
    /// <returns>A tuple containing the count of positive and negative approved reviews for the specified product.</returns>
    [HttpGet("count/{productId}")]
    public async Task<ActionResult<(int positive, int negative)>> GetProductApprovedReviewsCountAsync([FromRoute] Guid productId)
    {
        if (productId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var result = await _reviewService.GetProductApprovedReviewsCountAsync(productId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get product approved reviews count for product with id: {productId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a list of all reviews that are not approved.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing an ActionResult
    /// with a list of ReviewDto objects that have not been approved.
    /// </returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpGet("not-approved")]
    public async Task<ActionResult<List<ReviewDto>>> GetAllNotApprovedReviews()
    {
        try
        {
            var result = await _reviewService.GetAllNotApprovedReviewsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all not approved reviews" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all reviews present in the system.
    /// </summary>
    /// <returns>A list of <see cref="ReviewDto"/> objects representing all reviews, or an error response if an exception occurs.</returns>
    [HttpGet("all")]
    public async Task<ActionResult<List<ReviewDto>>> GetAllReviews()
    {
        try
        {
            var result = await _reviewService.GetAllReviewsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all reviews" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a list of all deleted reviews.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an ActionResult of a list of ReviewDto objects representing the deleted reviews.
    /// </returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpGet("deleted")]
    public async Task<ActionResult<List<ReviewDto>>> GetAllDeletedReviews()
    {
        try
        {
            var result = await _reviewService.GetAllDeletedReviewsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all deleted reviews" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }
}