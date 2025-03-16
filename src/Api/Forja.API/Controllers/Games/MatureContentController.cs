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

    public MatureContentController(IMatureContentService matureContentService, IProductMatureContentService productMatureContentService)
    {
        _matureContentService = matureContentService ?? throw new ArgumentNullException(nameof(matureContentService));
        _productMatureContentService = productMatureContentService ?? throw new ArgumentNullException(nameof(productMatureContentService));
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Creates a new mature content record.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing mature content record.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Deletes an existing mature content record.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Creates a new product-mature content association.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing product-mature content association.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Deletes an existing product-mature content association.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    #endregion
}