namespace Forja.API.Controllers.Games;

/// <summary>
/// Controller for managing Item Images and Product Images associations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IItemImageService _itemImageService;
    private readonly IProductImagesService _productImagesService;
    private readonly IAuditLogService _auditLogService;

    public ImagesController(IItemImageService itemImageService, 
        IProductImagesService productImagesService,
        IAuditLogService auditLogService)
    {
        _itemImageService = itemImageService;
        _productImagesService = productImagesService;
        _auditLogService = auditLogService;
    }

    #region ItemImages Endpoints

    /// <summary>
    /// Gets all item images.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("item-images")]
    public async Task<IActionResult> GetAllItemImagesAsync()
    {
        try
        {
            var itemImages = await _itemImageService.GetAllAsync();
            if (!itemImages.Any())
            {
                return NoContent();
            }

            return Ok(itemImages);
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
                        { "Message", $"Failed get all item images" }
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
    /// Gets an item image by its ID.
    /// </summary>
    [HttpGet("item-images/{id}")]
    public async Task<IActionResult> GetItemImageByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The provided item image ID is invalid." });

        try
        {
            var itemImage = await _itemImageService.GetByIdAsync(id);
            if (itemImage == null)
                return NotFound(new { error = $"ItemImage with ID {id} does not exist." });

            return Ok(itemImage);
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
                        { "Message", $"Failed get item image by id: {id}" }
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
    /// Creates a new item image.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("item-images")]
    public async Task<IActionResult> CreateItemImageAsync([FromBody] ItemImageCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdItemImage = await _itemImageService.CreateAsync(request);
            return createdItemImage != null ? Ok(createdItemImage) : BadRequest(new { error = "Failed to create item image." });
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
                        { "Message", $"Failed create item image: {request.ImageAlt}" }
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
    /// Updates an existing item image.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("item-images")]
    public async Task<IActionResult> UpdateItemImageAsync([FromBody] ItemImageUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedItemImage = await _itemImageService.UpdateAsync(request);
            if (updatedItemImage == null)
                return NotFound(new { error = $"ItemImage with ID {request.Id} does not exist." });

            return Ok(updatedItemImage);
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
                        { "Message", $"Failed upgrade item image with id: {request.Id}" }
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
    /// Deletes an existing item image.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("item-images/{id}")]
    public async Task<IActionResult> DeleteItemImageAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The provided item image ID is invalid." });

        try
        {
            var itemImage = await _itemImageService.GetByIdAsync(id);
            if (itemImage == null)
                return NotFound(new { error = $"ItemImage with ID {id} does not exist." });

            await _itemImageService.DeleteAsync(id);
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
                        { "Message", $"Failed delete item image with id: {id}" }
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

    #region ProductImages Endpoints

    /// <summary>
    /// Gets all product images.
    /// </summary>
    [HttpGet("product-images")]
    public async Task<IActionResult> GetAllProductImagesAsync()
    {
        try
        {
            var productImages = await _productImagesService.GetAllAsync();
            if (!productImages.Any())
            {
                return NoContent();
            }

            return Ok(productImages);
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
                        { "Message", $"Failed to get all product images" }
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
    /// Gets a product image by its ID.
    /// </summary>
    [HttpGet("product-images/{id}")]
    public async Task<IActionResult> GetProductImageByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The provided product image ID is invalid." });

        try
        {
            var productImage = await _productImagesService.GetByIdAsync(id);
            if (productImage == null)
                return NotFound(new { error = $"ProductImage with ID {id} does not exist." });

            return Ok(productImage);
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
                        { "Message", $"Failed to get product image by id: {id}" }
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
    /// Gets all product images by product ID.
    /// </summary>
    [HttpGet("product-images/by-product/{productId}")]
    public async Task<IActionResult> GetProductImagesByProductIdAsync([FromRoute] Guid productId)
    {
        if (productId == Guid.Empty)
            return BadRequest(new { error = "The provided product ID is invalid." });

        try
        {
            var productImages = await _productImagesService.GetByProductIdAsync(productId);
            if (!productImages.Any())
                return NoContent();

            return Ok(productImages);
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
                        { "Message", $"Failed to get product image by product id: {productId}" }
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
    /// Gets all product images by item image ID.
    /// </summary>
    [HttpGet("product-images/by-item-image/{itemImageId}")]
    public async Task<IActionResult> GetProductImagesByItemImageIdAsync([FromRoute] Guid itemImageId)
    {
        if (itemImageId == Guid.Empty)
            return BadRequest(new { error = "The provided item image ID is invalid." });

        try
        {
            var productImages = await _productImagesService.GetByItemImageIdAsync(itemImageId);
            if (!productImages.Any())
                return NoContent();

            return Ok(productImages);
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
                        { "Message", $"Failed to get product image by item image id: {itemImageId}" }
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
    /// Creates a new product image association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("product-images")]
    public async Task<IActionResult> CreateProductImageAsync([FromBody] ProductImagesCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdProductImage = await _productImagesService.CreateAsync(request);
            return createdProductImage != null ? Ok(createdProductImage) : BadRequest(new { error = "Failed to create product image association." });
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
                        { "Message", $"Failed to create product image with product id: {request.ProductId}, image id: {request.ItemImageId}" }
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
    /// Updates an existing product image association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("product-images")]
    public async Task<IActionResult> UpdateProductImageAsync([FromBody] ProductImagesUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedProductImage = await _productImagesService.UpdateAsync(request);
            if (updatedProductImage == null)
                return NotFound(new { error = $"ProductImage with ID {request.Id} does not exist." });

            return Ok(updatedProductImage);
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
                        { "Message", $"Failed to update product image with id: {request.Id}" }
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
    /// Deletes an existing product image association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("product-images/{id}")]
    public async Task<IActionResult> DeleteProductImageAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The provided product image ID is invalid." });

        try
        {
            var productImage = await _productImagesService.GetByIdAsync(id);
            if (productImage == null)
                return NotFound(new { error = $"ProductImage with ID {id} does not exist." });

            await _productImagesService.DeleteAsync(id);
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
                        { "Message", $"Failed to delete product image with id: {id}" }
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