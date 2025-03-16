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

    public ImagesController(IItemImageService itemImageService, IProductImagesService productImagesService)
    {
        _itemImageService = itemImageService;
        _productImagesService = productImagesService;
    }

    #region ItemImages Endpoints

    /// <summary>
    /// Gets all item images.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Creates a new item image.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing item image.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Deletes an existing item image.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Creates a new product image association.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing product image association.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Deletes an existing product image association.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    #endregion
}