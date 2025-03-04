namespace Forja.API.Controllers.UserProfile;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost("{keycloakId}")]
    public async Task<IActionResult> AddReview(string keycloakId, [FromBody] ReviewDto reviewDto)
    {
        await _reviewService.AddReviewAsync(keycloakId, reviewDto);
        return NoContent();
    }

    [HttpGet("{reviewId}")]
    public async Task<IActionResult> GetReviewById(Guid reviewId)
    {
        var result = await _reviewService.GetReviewByIdAsync(reviewId);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateReview([FromBody] ReviewDto reviewDto)
    {
        await _reviewService.UpdateReviewAsync(reviewDto);
        return NoContent();
    }

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        await _reviewService.DeleteReviewAsync(reviewId);
        return NoContent();
    }

    [HttpPost("{reviewId}/restore")]
    public async Task<IActionResult> RestoreReview(Guid reviewId)
    {
        var result = await _reviewService.RestoreReviewAsync(reviewId);
        return Ok(result);
    }
    
    [HttpGet("{keycloakId}/all")]
    public async Task<IActionResult> GetAllUserReviews(string keycloakId)
    {
        var result = await _reviewService.GetAllUserReviewsAsync(keycloakId);
        return Ok(result);
    }

    [HttpGet("{keycloakId}/deleted")]
    public async Task<IActionResult> GetAllUserDeletedReviews(string keycloakId)
    {
        var result = await _reviewService.GetAllUserDeletedReviewsAsync(keycloakId);
        return Ok(result);
    }

    [HttpGet("games/{gameId}")]
    public async Task<IActionResult> GetAllGameReviews(Guid gameId)
    {
        var result = await _reviewService.GetAllGameReviewsAsync(gameId);
        return Ok(result);
    }
    
    [HttpGet("games/{gameId}/deleted")]
    public async Task<IActionResult> GetAllDeletedGameReviews(Guid gameId)
    {
        var result = await _reviewService.GetAllDeletedGameReviewsAsync(gameId);
        return Ok(result);
    }
    
    [HttpGet("not-approved")]
    public async Task<IActionResult> GetAllNotApprovedReviews()
    {
        var result = await _reviewService.GetAllNotApprovedReviewsAsync();
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllReviews()
    {
        var result = await _reviewService.GetAllReviewsAsync();
        return Ok(result);
    }
    
    [HttpGet("deleted")]
    public async Task<IActionResult> GetAllDeletedReviews()
    {
        var result = await _reviewService.GetAllDeletedReviewsAsync();
        return Ok(result);
    }
}