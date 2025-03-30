namespace Forja.API.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
public class LegalDocumentController : ControllerBase
{
    private readonly ILegalDocumentService _legalDocumentService;
    private readonly IAuditLogService _auditLogService;

    public LegalDocumentController(ILegalDocumentService legalDocumentService,
        IAuditLogService auditLogService)
    {
        _legalDocumentService = legalDocumentService;
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Get a legal document by its ID.
    /// </summary>
    [Authorize(Policy = "DocumentReadPolicy")]
    [HttpGet("{documentId:guid}")]
    public async Task<IActionResult> GetLegalDocumentById([FromRoute] Guid documentId)
    {
        if (documentId == Guid.Empty) return BadRequest(new { error = "Document ID is required." });
        try
        {
            var document = await _legalDocumentService.GetLegalDocumentByIdAsync(documentId);
            if (document == null) return NotFound(new { error = $"No document found with ID {documentId}." });
            return Ok(document);
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
                        { "Message", $"Failed to get legal document {documentId}" }
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
    /// Get legal documents by effective date.
    /// </summary>
    [Authorize(Policy = "DocumentReadPolicy")]
    [HttpGet("date")]
    public async Task<IActionResult> GetLegalDocumentsByEffectiveDate([FromQuery] DateTime? effectiveDate)
    {
        try
        {
            var documents = await _legalDocumentService.GetLegalDocumentsByEffectiveDateAsync(effectiveDate);
            return Ok(documents);
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
                        { "Message", $"Failed to get legal documents by date {effectiveDate}" }
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
    /// Get a legal document by its title.
    /// </summary>
    [Authorize(Policy = "DocumentReadPolicy")]
    [HttpGet("title")]
    public async Task<IActionResult> GetLegalDocumentByTitle([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return BadRequest(new { error = "Title is required." });
        try
        {
            var document = await _legalDocumentService.GetLegalDocumentByTitleAsync(title);
            if (document == null) return NotFound(new { error = $"No document found with the title '{title}'." });
            return Ok(document);
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
                        { "Message", $"Failed to get legal document by title {title}" }
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
    /// Create a new legal document.
    /// </summary>
    [Authorize(Policy = "DocumentWritePolicy")]
    [HttpPost]
    public async Task<IActionResult> CreateLegalDocument([FromBody] LegalDocumentCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var createdDocument = await _legalDocumentService.CreateLegalDocumentAsync(request);
            return CreatedAtAction(nameof(GetLegalDocumentById), new { documentId = createdDocument?.Id }, createdDocument);
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
                        { "Message", $"Failed to create legal document {request.Title}" }
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
    /// Update an existing legal document.
    /// </summary>
    [Authorize(Policy = "DocumentWritePolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateLegalDocument([FromBody] LegalDocumentUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var updatedDocument = await _legalDocumentService.UpdateLegalDocumentAsync(request);
            return Ok(updatedDocument);
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
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update legal document {request.Id}" }
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
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update legal document {request.Id}" }
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
    /// Delete a legal document by its ID.
    /// </summary>
    [Authorize(Policy = "DocumentWritePolicy")]
    [HttpDelete("{documentId:guid}")]
    public async Task<IActionResult> DeleteLegalDocument([FromRoute] Guid documentId)
    {
        if (documentId == Guid.Empty) return BadRequest(new { error = "Document ID is required." });
        try
        {
            await _legalDocumentService.DeleteLegalDocumentAsync(documentId);
            return NoContent();
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
                        { "Message", $"Failed to delete legal document {documentId}" }
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
                        { "Message", $"Failed to delete legal document {documentId}" }
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