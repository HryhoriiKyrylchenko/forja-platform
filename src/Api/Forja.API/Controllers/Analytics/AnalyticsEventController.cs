using Forja.Application.Interfaces.Analytics;
using Forja.Application.Requests.Analytics;

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

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AnalyticsEventUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updatedEvent = await _analyticsEventService.UpdateEventAsync(request);
            if (updatedEvent == null) return NotFound();

            return Ok(updatedEvent);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

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