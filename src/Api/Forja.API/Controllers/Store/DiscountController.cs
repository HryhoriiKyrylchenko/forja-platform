namespace Forja.API.Controllers.Store;

[ApiController]
[Route("api/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;

    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpGet("{discountId:guid}")]
    [Authorize(Policy = "StoreManagePolicy")]
    public async Task<IActionResult> GetDiscountById([FromRoute] Guid discountId)
    {
        if (discountId == Guid.Empty) return BadRequest(new { error = "Discount ID is required." });
        try
        {
            var discount = await _discountService.GetDiscountByIdAsync(discountId);
            if (discount == null) return NotFound(new { error = $"No discount found with ID {discountId}." });
            return Ok(discount);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Policy = "StoreManagePolicy")]
    public async Task<IActionResult> GetAllDiscounts()
    {
        try
        {
            var discounts = await _discountService.GetAllDiscountsAsync();
            return Ok(discounts);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("active")]
    [Authorize]
    public async Task<IActionResult> GetActiveDiscounts()
    {
        try
        {
            DateTime currentDate = DateTime.UtcNow;
            var activeDiscounts = await _discountService.GetActiveDiscountsAsync(currentDate);
            return Ok(activeDiscounts);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Policy = "StoreManagePolicy")]
    public async Task<IActionResult> AddDiscount([FromBody] DiscountCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _discountService.AddDiscountAsync(request);
            if (result == null) return BadRequest(new { error = "Failed to create discount." });
            return CreatedAtAction(nameof(GetDiscountById), new { discountId = result.Id }, request);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut]
    [Authorize(Policy = "StoreManagePolicy")]
    public async Task<IActionResult> UpdateDiscount([FromBody] DiscountUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _discountService.UpdateDiscountAsync(request);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{discountId:guid}")]
    [Authorize(Policy = "StoreManagePolicy")]
    public async Task<IActionResult> DeleteDiscount([FromRoute] Guid discountId)
    {
        if (discountId == Guid.Empty) return BadRequest(new { error = "Discount ID is required." });
        try
        {
            await _discountService.DeleteDiscountAsync(discountId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // ------------------- ProductDiscount Endpoints -------------------

    [HttpGet("product-discounts/{productDiscountId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetProductDiscountById([FromRoute] Guid productDiscountId)
    {
        if (productDiscountId == Guid.Empty) return BadRequest(new { error = "ProductDiscount ID is required." });
        try
        {
            var productDiscount = await _discountService.GetProductDiscountByIdAsync(productDiscountId);
            if (productDiscount == null) return NotFound(new { error = $"No product discount found with ID {productDiscountId}." });
            return Ok(productDiscount);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("product/{productId:guid}/discounts")]
    [Authorize]
    public async Task<IActionResult> GetProductDiscountsByProductId([FromRoute] Guid productId)
    {
        if (productId == Guid.Empty) return BadRequest(new { error = "Product ID is required." });
        try
        {
            var productDiscounts = await _discountService.GetProductDiscountsByProductIdAsync(productId);
            return Ok(productDiscounts);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{discountId:guid}/product-discounts")]
    [Authorize(Policy = "StoreManagePolicy")]
    public async Task<IActionResult> GetProductDiscountsByDiscountId([FromRoute] Guid discountId)
    {
        if (discountId == Guid.Empty) return BadRequest(new { error = "Discount ID is required." });
        try
        {
            var productDiscounts = await _discountService.GetProductDiscountsByDiscountIdAsync(discountId);
            return Ok(productDiscounts);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("product-discounts")]
    [Authorize(Policy = "StoreManagePolicy")]
    public async Task<IActionResult> AddProductDiscount([FromBody] ProductDiscountCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _discountService.AddProductDiscountAsync(request);
            if (result == null) return BadRequest(new { error = "Failed to create discount." });
            return CreatedAtAction(nameof(GetDiscountById), new { discountId = result.Id }, request);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("product-discounts")]
    [Authorize(Policy = "StoreManagePolicy")]
    public async Task<IActionResult> UpdateProductDiscount([FromBody] ProductDiscountUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _discountService.UpdateProductDiscountAsync(request);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("/product-discounts/{discountId:guid}")]
    [Authorize(Policy = "StoreManagePolicy")]
    public async Task<IActionResult> DeleteProductDiscount([FromRoute] Guid productDiscountId)
    {
        if (productDiscountId == Guid.Empty) return BadRequest(new { error = "Discount ID is required." });
        try
        {
            await _discountService.DeleteProductDiscountAsync(productDiscountId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}