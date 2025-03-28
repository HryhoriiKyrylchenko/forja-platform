namespace Forja.API.Controllers.Analytics;

/// <summary>
/// Controller for managing analytics sessions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsSessionController : ControllerBase
{
    private readonly IAnalyticsSessionService _analyticsSessionService;

    public AnalyticsSessionController(IAnalyticsSessionService analyticsSessionService)
    {
        _analyticsSessionService = analyticsSessionService;
    }

    /// <summary>
    /// Get all analytics sessions.
    /// </summary>
    /// <returns>List of analytics sessions.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllSessions()
    {
        var sessions = await _analyticsSessionService.GetAllAsync();
        return Ok(sessions);
    }

    /// <summary>
    /// Get analytics session by ID.
    /// </summary>
    /// <param name="sessionId">The ID of the analytics session.</param>
    /// <returns>The analytics session if found.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{sessionId:guid}")]
    public async Task<IActionResult> GetSessionById([FromRoute] Guid sessionId)
    {
        if (sessionId == Guid.Empty)
        {
            return BadRequest("Session ID is required.");
        }
        try
        {
            var session = await _analyticsSessionService.GetByIdAsync(sessionId);
            if (session == null)
            {
                return NotFound($"Session with ID {sessionId} not found.");
            }

            return Ok(session);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get all analytics sessions for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>List of analytics sessions for the specified user.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetSessionsByUserId([FromRoute] Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("User ID is required.");
        }
        try
        {
            var sessions = await _analyticsSessionService.GetByUserIdAsync(userId);
            return Ok(sessions);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// End an existing analytics session.
    /// </summary>
    /// <param name="sessionId">The ID of the session to end.</param>
    /// <returns>No content if successful.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("{sessionId:guid}/end")]
    public async Task<IActionResult> EndSession([FromRoute] Guid sessionId)
    {
        if (sessionId == Guid.Empty)
        {
            return BadRequest("Session ID is required.");
        }
        try
        {
            await _analyticsSessionService.EndSessionAsync(sessionId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Delete an analytics session.
    /// </summary>
    /// <param name="sessionId">The ID of the session to delete.</param>
    /// <returns>No content if successful.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{sessionId:guid}")]
    public async Task<IActionResult> DeleteSession([FromRoute] Guid sessionId)
    {
        if (sessionId == Guid.Empty)
        {
            return BadRequest("Session ID is required.");
        }
        try
        {
            await _analyticsSessionService.DeleteSessionAsync(sessionId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}