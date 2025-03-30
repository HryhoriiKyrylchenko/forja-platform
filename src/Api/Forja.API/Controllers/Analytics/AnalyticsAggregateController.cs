namespace Forja.API.Controllers.Analytics;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsAggregateController : ControllerBase
{
    private readonly IAnalyticsAggregateService _analyticsAggregateService;
    private readonly IAuditLogService _auditLogService;

    public AnalyticsAggregateController(IAnalyticsAggregateService analyticsAggregateService,
        IAuditLogService auditLogService)
    {
        _analyticsAggregateService = analyticsAggregateService;
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Gets the analytics aggregates of events for a specified event type and date range.
    /// </summary>
    /// <param name="eventType">The type of the event to aggregate (e.g., PageView, Purchase).</param>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date (optional).</param>
    /// <returns>A list of analytics aggregate DTOs.</returns>
    [HttpGet("events")]
    public async Task<IActionResult> GetAnalyticsAggregateOfEventsAsync(
        [FromQuery] AnalyticEventType eventType,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _analyticsAggregateService
                .GetAnalyticsAggregateOfEventsAsync(eventType, startDate, endDate);
            return Ok(result);
        }
        catch (ArgumentException ex)
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
                        { "Message", $"Failed to get analytics aggregate of events for event type {eventType} and date range {startDate} - {endDate}." }
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
                        { "Message", $"Failed to get analytics aggregate of events for event type {eventType} and date range {startDate} - {endDate}." }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }

    /// <summary>
    /// Gets the analytics aggregates of sessions for a specified date range.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date (optional).</param>
    /// <returns>A list of analytics aggregate DTOs.</returns>
    [HttpGet("sessions")]
    public async Task<IActionResult> GetAnalyticsAggregateOfSessionsAsync(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _analyticsAggregateService
                .GetAnalyticsAggregateOfSessionsAsync(startDate, endDate);
            return Ok(result);
        }
        catch (ArgumentException ex)
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
                        { "Message", $"Failed to get analytics aggregate of sessions for date range {startDate} - {endDate}." }
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
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
}