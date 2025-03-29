namespace Forja.API.Controllers.Games;

/// <summary>
/// Controller for managing Bundles and BundleProducts in the games domain.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BundlesController : ControllerBase
{
    private readonly IBundleService _bundleService;
    private readonly IBundleProductService _bundleProductService;
    private readonly IAuditLogService _auditLogService;

    public BundlesController(IBundleService bundleService, 
        IBundleProductService bundleProductService,
        IAuditLogService auditLogService)
    {
        _bundleService = bundleService;
        _bundleProductService = bundleProductService;
        _auditLogService = auditLogService;
    }

    #region Bundle Endpoints

    /// <summary>
    /// Gets all bundles.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("bundles")]
    public async Task<IActionResult> GetBundlesAsync()
    {
        try
        {
            var bundles = await _bundleService.GetAllAsync();
            return Ok(bundles);
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
                        { "Message", "Failed to get all bundles" }
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
    /// Gets all active bundles.
    /// </summary>
    [HttpGet("bundles/active")]
    public async Task<IActionResult> GetActiveBundlesAsync()
    {
        try
        {
            var activeBundles = await _bundleService.GetActiveBundlesAsync();
            return Ok(activeBundles);
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
                        { "Message", "Failed to get active bundles" }
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
    /// Gets a bundle by its ID.
    /// </summary>
    [HttpGet("bundles/{id}")]
    public async Task<IActionResult> GetBundleByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "Id cannot be empty." });
            
        try
        {
            var bundle = await _bundleService.GetByIdAsync(id);
            return bundle == null ? NotFound(new { error = $"Bundle with ID {id} not found." }) : Ok(bundle);
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
                        { "Message", $"Failed to get bundle by id: {id}" }
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
    /// Creates a new bundle.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("bundles")]
    public async Task<IActionResult> CreateBundleAsync([FromBody] BundleCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var createdBundle = await _bundleService.CreateAsync(request);
            return createdBundle != null ? Ok(createdBundle) : BadRequest(new { error = "Failed to create bundle." });
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
                        { "Message", $"Failed to create bundle: {request.Title}" }
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
    /// Updates an existing bundle by its ID.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("bundles")]
    public async Task<IActionResult> UpdateBundleAsync([FromBody] BundleUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updatedBundle = await _bundleService.UpdateAsync(request);
            return updatedBundle != null ? Ok(updatedBundle) : BadRequest(new { error = "Failed to update bundle." });
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
                        { "Message", $"Failed to update bundle: {request.Id}" }
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
    /// Deletes a bundle by its ID.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("bundles/{id}")]
    public async Task<IActionResult> DeleteBundleAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "Id cannot be empty." });

        try
        {
            await _bundleService.DeleteAsync(id);
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
                        { "Message", $"Failed to update bundle: {id}" }
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

    #region BundleProduct Endpoints

    /// <summary>
    /// Gets all bundle products.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("bundle-products")]
    public async Task<IActionResult> GetBundleProductsAsync()
    {
        try
        {
            var bundleProducts = await _bundleProductService.GetAllAsync();
            return Ok(bundleProducts);
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
                        { "Message", $"Failed to get bundle products" }
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
    /// Gets a bundle product by its ID.
    /// </summary>
    [HttpGet("bundle-products/{id}")]
    public async Task<IActionResult> GetBundleProductByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Id cannot be empty.");

        try
        {
            var bundleProduct = await _bundleProductService.GetByIdAsync(id);
            return bundleProduct == null ? NotFound($"BundleProduct with ID {id} not found.") : Ok(bundleProduct);
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
                        { "Message", $"Failed to get bundle product by id: {id}" }
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
    /// Gets all bundle products by bundle ID.
    /// </summary>
    [HttpGet("bundle-products/by-bundle/{bundleId}")]
    public async Task<IActionResult> GetBundleProductsByBundleIdAsync([FromRoute] Guid bundleId)
    {
        if (bundleId == Guid.Empty) return BadRequest("Bundle ID cannot be empty.");
        try
        {
            var bundleProducts = await _bundleProductService.GetByBundleIdAsync(bundleId);
            return Ok(bundleProducts);
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
                        { "Message", $"Failed to get bundle product by bundle id: {bundleId}" }
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
    /// Creates a new bundle product.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("bundle-products")]
    public async Task<IActionResult> CreateBundleProductAsync([FromBody] BundleProductCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var createdBundleProduct = await _bundleProductService.CreateAsync(request);
            return createdBundleProduct != null ? Ok(createdBundleProduct) : BadRequest(new { error = "Failed to create bundle product." });
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
                        { "Message", $"Failed to create bundle product with product id: {request.ProductId} and bundle id: {request.BundleId}" }
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
    /// Updates an existing bundle product.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("bundle-products")]
    public async Task<IActionResult> UpdateBundleProductAsync([FromBody] BundleProductUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updatedBundleProduct = await _bundleProductService.UpdateAsync(request);
            if (updatedBundleProduct == null) 
                return NotFound(new { error = $"BundleProduct with ID {request.Id} not found." });
            
            return Ok(updatedBundleProduct);
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
                        { "Message", $"Failed to update bundle product with id: {request.Id}" }
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
    /// Deletes a bundle product by its ID.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("bundle-products/{id}")]
    public async Task<IActionResult> DeleteBundleProductAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "Id cannot be empty." });

        try
        {
            await _bundleProductService.DeleteAsync(id);
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
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete bundle product with id: {id}" }
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