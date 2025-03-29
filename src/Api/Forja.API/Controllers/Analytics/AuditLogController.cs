using Forja.Domain.Entities.Analytics;

namespace Forja.API.Controllers.Analytics;

[ApiController]
[Route("api/[controller]")]
public class AuditLogController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Retrieves all audit logs.
    /// </summary>
    /// <returns>A list of all audit logs.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditLog>>> GetAllLogsAsync()
    {
        var logs = await _auditLogService.GetAllLogsAsync();
        return Ok(logs); 
    }

    /// <summary>
    /// Retrieves audit logs based on filter criteria.
    /// </summary>
    /// <param name="userId">An optional filter for UserId.</param>
    /// <param name="entityType">An optional filter for EntityType.</param>
    /// <param name="actionType">An optional filter for ActionType.</param>
    /// <returns>A filtered list of audit logs.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<AuditLog>>> GetLogsByFilterAsync(
        [FromQuery] Guid? userId = null,
        [FromQuery] AuditEntityType? entityType = null,
        [FromQuery] AuditActionType? actionType = null)
    {
        var logs = await _auditLogService.GetLogsByFilterAsync(userId, entityType, actionType);
        return Ok(logs); 
    }

    /// <summary>
    /// Deletes an audit log by its ID.
    /// </summary>
    /// <param name="logId">The ID of the audit log to delete.</param>
    /// <returns>An HTTP response indicating the result of the operation.</returns>
    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{logId:guid}")]
    public async Task<IActionResult> DeleteLogAsync(Guid logId)
    {
        if (logId == Guid.Empty)
        {
            return BadRequest(new { message = "Log ID is required." });
        }
        try
        {
            await _auditLogService.DeleteLogAsync(logId);
            return NoContent(); 
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message }); // Return 400 Bad Request for invalid input
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Log with ID '{logId}' was not found." }); // Return 404 Not Found
        }
    }
}