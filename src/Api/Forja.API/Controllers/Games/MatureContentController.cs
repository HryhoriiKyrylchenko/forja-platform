namespace Forja.API.Controllers.Games;

/// <summary>
/// Controller for managing mature content data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MatureContentController : ControllerBase
{
    private readonly IMatureContentService _matureContentService;
    private readonly IProductMatureContentService _productMatureContentService;
    private readonly IAuditLogService _auditLogService;

    public MatureContentController(IMatureContentService matureContentService, 
        IProductMatureContentService productMatureContentService,
        IAuditLogService auditLogService)
    {
        _matureContentService = matureContentService;
        _productMatureContentService = productMatureContentService;
        _auditLogService = auditLogService;
    }

    #region MatureContent Endpoints

    /// <summary>
    /// Gets all mature content records.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllMatureContentAsync()
    {
        try
        {
            var contents = await _matureContentService.GetAllAsync();
            if (!contents.Any())
            {
                return NoContent();
            }

            return Ok(contents);
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
                        { "Message", $"Failed to get all mature content records" }
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
    /// Gets a mature content record by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMatureContentByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The provided content ID is invalid." });

        try
        {
            var matureContent = await _matureContentService.GetByIdAsync(id);
            if (matureContent == null)
                return NotFound(new { error = $"MatureContent with ID {id} does not exist." });

            return Ok(matureContent);
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
                        { "Message", $"Failed to get mature content record by id: {id}" }
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
    /// Creates a new mature content record.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost]
    public async Task<IActionResult> CreateMatureContentAsync([FromBody] MatureContentCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdContent = await _matureContentService.CreateAsync(request);
            
            return createdContent != null ? Ok(createdContent) : BadRequest(new { error = "Failed to create mature content." });
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
                        { "Message", $"Failed to create mature content record: {request.Name}" }
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
    /// Updates an existing mature content record.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateMatureContentAsync([FromBody] MatureContentUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedContent = await _matureContentService.UpdateAsync(request);
            if (updatedContent == null)
                return NotFound(new { error = $"MatureContent with ID {request.Id} does not exist." });

            return Ok(updatedContent);
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
                        { "Message", $"Failed to update mature content record: {request.Id}" }
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
    /// Deletes an existing mature content record.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatureContentAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The provided content ID is invalid." });

        try
        {
            var matureContent = await _matureContentService.GetByIdAsync(id);
            if (matureContent == null)
                return NotFound(new { error = $"MatureContent with ID {id} does not exist." });

            await _matureContentService.DeleteAsync(id);
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
                        { "Message", $"Failed to delete mature content record: {id}" }
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

    #endregion
    
    #region ProductMatureContent Endpoints

    /// <summary>
    /// Gets all product-mature content associations.
    /// </summary>
    [HttpGet("product-mature-content")]
    public async Task<IActionResult> GetAllProductMatureContentAsync()
    {
        try
        {
            var associations = await _productMatureContentService.GetAllAsync();
            if (!associations.Any())
            {
                return NoContent();
            }

            return Ok(associations);
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
                        { "Message", $"Failed to get all product mature content" }
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
    /// Gets a product-mature content association by its ID.
    /// </summary>
    [HttpGet("product-mature-content/{id}")]
    public async Task<IActionResult> GetProductMatureContentByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The provided association ID is invalid." });

        try
        {
            var association = await _productMatureContentService.GetByIdAsync(id);
            if (association == null)
                return NotFound(new { error = $"ProductMatureContent with ID {id} does not exist." });

            return Ok(association);
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
                        { "Message", $"Failed to get product mature content by id: {id}" }
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
    /// Gets all product-mature content associations by product ID.
    /// </summary>
    [HttpGet("product-mature-content/by-product/{productId}")]
    public async Task<IActionResult> GetProductMatureContentByProductIdAsync([FromRoute] Guid productId)
    {
        if (productId == Guid.Empty)
            return BadRequest(new { error = "The provided product ID is invalid." });

        try
        {
            var associations = await _productMatureContentService.GetByProductIdAsync(productId);
            if (!associations.Any())
                return NoContent();

            return Ok(associations);
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
                        { "Message", $"Failed to get product mature content by product id: {productId}" }
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
    /// Gets all product-mature content associations by mature content ID.
    /// </summary>
    [HttpGet("product-mature-content/by-mature-content/{matureContentId}")]
    public async Task<IActionResult> GetProductMatureContentByMatureContentIdAsync([FromRoute] Guid matureContentId)
    {
        if (matureContentId == Guid.Empty)
            return BadRequest(new { error = "The provided mature content ID is invalid." });

        try
        {
            var associations = await _productMatureContentService.GetByMatureContentIdAsync(matureContentId);
            if (!associations.Any())
                return NoContent();

            return Ok(associations);
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
                        { "Message", $"Failed to get product mature content by mature content id: {matureContentId}" }
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
    /// Creates a new product-mature content association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("product-mature-content")]
    public async Task<IActionResult> CreateProductMatureContentAsync([FromBody] ProductMatureContentCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdAssociation = await _productMatureContentService.CreateAsync(request);
            
            return createdAssociation != null ? Ok(createdAssociation) : BadRequest(new { error = "Failed to create product-mature content association." });
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
                        { "Message", $"Failed to create product mature content with product id: {request.ProductId} and mature content id: {request.MatureContentId}" }
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
    /// Updates an existing product-mature content association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("product-mature-content/{id}")]
    public async Task<IActionResult> UpdateProductMatureContentAsync([FromBody] ProductMatureContentUpdateRequest request, [FromRoute] Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedAssociation = await _productMatureContentService.UpdateAsync(request);
            if (updatedAssociation == null)
                return NotFound(new { error = $"ProductMatureContent with ID {id} does not exist." });

            return Ok(updatedAssociation);
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
                        { "Message", $"Failed to update product mature content with id: {request.Id}" }
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
    /// Deletes an existing product-mature content association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("product-mature-content/{id}")]
    public async Task<IActionResult> DeleteProductMatureContentAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("The provided association ID is invalid.");

        try
        {
            var association = await _productMatureContentService.GetByIdAsync(id);
            if (association == null)
                return NotFound(new { error = $"ProductMatureContent with ID {id} does not exist." });

            await _productMatureContentService.DeleteAsync(id);
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
                        { "Message", $"Failed to delete product mature content with id: {id}" }
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

    #endregion
}