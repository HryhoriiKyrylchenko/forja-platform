namespace Forja.API.Controllers.Analytics;

[Route("api/[controller]")]
[ApiController]
public class AnalyticsEventController : ControllerBase
{
    private readonly IAnalyticsEventService _analyticsEventService;
    private readonly IAuditLogService _auditLogService;

    public AnalyticsEventController(IAnalyticsEventService analyticsEventService,
        IAuditLogService auditLogService)
    {
        _analyticsEventService = analyticsEventService;
        _auditLogService = auditLogService;
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
                        { "Message", $"Failed to get analytics event by id: {id}." }
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

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var analyticsEvents = await _analyticsEventService.GetAllAsync();
            return Ok(analyticsEvents);
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
                        { "Message", $"Failed to get all analytics events." }
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
                        { "Message", $"Failed to get analytics events by user id: {userId}." }
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

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "Event ID is required." });
        try
        {
            await _analyticsEventService.DeleteEventAsync(id);
        }
        catch (KeyNotFoundException ex)
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
                        { "Message", $"Failed to delete analytics event with id: {id}." }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return NotFound(new { error = ex.Message });
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
                        { "Message", $"Failed to delete analytics event with id: {id}." }
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

        return NoContent();
    }
}