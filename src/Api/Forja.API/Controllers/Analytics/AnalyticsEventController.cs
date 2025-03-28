namespace Forja.API.Controllers.Analytics;

[Route("api/[controller]")]
[ApiController]
public class AnalyticsEventController : ControllerBase
{
    private readonly IAnalyticsEventService _analyticsEventService;

    public AnalyticsEventController(IAnalyticsEventService analyticsEventService)
    {
        _analyticsEventService = analyticsEventService;
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "Event ID is required." });
        try
        {
            var analyticsEvent = await _analyticsEventService.GetByIdAsync(id);
            if (analyticsEvent == null) return NotFound();

            return Ok(analyticsEvent);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var analyticsEvents = await _analyticsEventService.GetAllAsync();
            return Ok(analyticsEvents);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId([FromRoute] Guid userId)
    {
        if (userId == Guid.Empty) return BadRequest(new { error = "User ID is required." });
        try
        {
            var analyticsEvents = await _analyticsEventService.GetByUserIdAsync(userId);
            return Ok(analyticsEvents);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "Event ID is required." });
        try
        {
            await _analyticsEventService.DeleteEventAsync(id);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { error = e.Message });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }

        return NoContent();
    }
}