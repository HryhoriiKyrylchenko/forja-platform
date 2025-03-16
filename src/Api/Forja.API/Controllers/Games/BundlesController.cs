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

    public BundlesController(IBundleService bundleService, IBundleProductService bundleProductService)
    {
        _bundleService = bundleService;
        _bundleProductService = bundleProductService;
    }

    #region Bundle Endpoints

    /// <summary>
    /// Gets all bundles.
    /// </summary>
    [HttpGet("bundles")]
    public async Task<IActionResult> GetBundlesAsync()
    {
        try
        {
            var bundles = await _bundleService.GetAllAsync();
            return Ok(bundles);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Creates a new bundle.
    /// </summary>
    [HttpPost("bundles")]
    public async Task<IActionResult> CreateBundleAsync([FromBody] BundleCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var createdBundle = await _bundleService.CreateAsync(request);
            return createdBundle != null ? Ok(createdBundle) : BadRequest(new { error = "Failed to create bundle." });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing bundle by its ID.
    /// </summary>
    [HttpPut("bundles")]
    public async Task<IActionResult> UpdateBundleAsync([FromBody] BundleUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updatedBundle = await _bundleService.UpdateAsync(request);
            return updatedBundle != null ? Ok(updatedBundle) : BadRequest(new { error = "Failed to update bundle." });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Deletes a bundle by its ID.
    /// </summary>
    [HttpDelete("bundles/{id}")]
    public async Task<IActionResult> DeleteBundleAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "Id cannot be empty." });

        try
        {
            await _bundleService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    #endregion

    #region BundleProduct Endpoints

    /// <summary>
    /// Gets all bundle products.
    /// </summary>
    [HttpGet("bundle-products")]
    public async Task<IActionResult> GetBundleProductsAsync()
    {
        try
        {
            var bundleProducts = await _bundleProductService.GetAllAsync();
            return Ok(bundleProducts);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Creates a new bundle product.
    /// </summary>
    [HttpPost("bundle-products")]
    public async Task<IActionResult> CreateBundleProductAsync([FromBody] BundleProductCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var createdBundleProduct = await _bundleProductService.CreateAsync(request);
            return createdBundleProduct != null ? Ok(createdBundleProduct) : BadRequest(new { error = "Failed to create bundle product." });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing bundle product.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Deletes a bundle product by its ID.
    /// </summary>
    [HttpDelete("bundle-products/{id}")]
    public async Task<IActionResult> DeleteBundleProductAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "Id cannot be empty." });

        try
        {
            await _bundleProductService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    #endregion
}