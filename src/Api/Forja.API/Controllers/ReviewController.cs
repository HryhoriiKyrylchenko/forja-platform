namespace Forja.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly UserProfileService _userService;

    public ReviewController(UserProfileService userService)
    {
        _userService = userService;
    }

    [HttpPost("{keycloakId}")]
    public async Task<IActionResult> AddReview(string keycloakId, [FromBody] ReviewDto reviewDto)
    {
        await _userService.AddReviewAsync(keycloakId, reviewDto);
        return NoContent();
    }

    [HttpGet("{reviewId}")]
    public async Task<IActionResult> GetReviewById(Guid reviewId)
    {
        var result = await _userService.GetReviewByIdAsync(reviewId);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateReview([FromBody] ReviewDto reviewDto)
    {
        await _userService.UpdateReviewAsync(reviewDto);
        return NoContent();
    }

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        await _userService.DeleteReviewAsync(reviewId);
        return NoContent();
    }

    [HttpPost("{reviewId}/restore")]
    public async Task<IActionResult> RestoreReview(Guid reviewId)
    {
        var result = await _userService.RestoreReviewAsync(reviewId);
        return Ok(result);
    }
    
    [HttpGet("{keycloakId}/all")]
    public async Task<IActionResult> GetAllUserReviews(string keycloakId)
    {
        var result = await _userService.GetAllUserReviewsAsync(keycloakId);
        return Ok(result);
    }

    [HttpGet("{keycloakId}/deleted")]
    public async Task<IActionResult> GetAllUserDeletedReviews(string keycloakId)
    {
        var result = await _userService.GetAllUserDeletedReviewsAsync(keycloakId);
        return Ok(result);
    }

    [HttpGet("games/{gameId}")]
    public async Task<IActionResult> GetAllGameReviews(Guid gameId)
    {
        var result = await _userService.GetAllGameReviewsAsync(gameId);
        return Ok(result);
    }
    
    [HttpGet("games/{gameId}/deleted")]
    public async Task<IActionResult> GetAllDeletedGameReviews(Guid gameId)
    {
        var result = await _userService.GetAllDeletedGameReviewsAsync(gameId);
        return Ok(result);
    }
    
    [HttpGet("not-approved")]
    public async Task<IActionResult> GetAllNotApprovedReviews()
    {
        var result = await _userService.GetAllNotApprovedReviewsAsync();
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllReviews()
    {
        var result = await _userService.GetAllReviewsAsync();
        return Ok(result);
    }
    
    [HttpGet("deleted")]
    public async Task<IActionResult> GetAllDeletedReviews()
    {
        var result = await _userService.GetAllDeletedReviewsAsync();
        return Ok(result);
    }
}