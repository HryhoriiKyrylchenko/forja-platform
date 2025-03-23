namespace Forja.API.Controllers.Support;

/// <summary>
/// Controller for managing FAQ entities.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FAQController : ControllerBase
{
    private readonly IFAQService _faqService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FAQController"/> class.
    /// </summary>
    /// <param name="faqService">The service for managing FAQs.</param>
    public FAQController(IFAQService faqService)
    {
        _faqService = faqService;
    }

    /// <summary>
    /// Retrieves all FAQs.
    /// </summary>
    /// <returns>A list of FAQ DTOs.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllFAQs()
    {
        try
        {
            var faqs = await _faqService.GetAllAsync();
            return Ok(faqs);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a specific FAQ by its ID.
    /// </summary>
    /// <param name="id">The ID of the FAQ to retrieve.</param>
    /// <returns>A FAQ DTO.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetFAQById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "FAQ ID is required." });

        try
        {
            var faq = await _faqService.GetByIdAsync(id);
            if (faq == null) return NotFound(new { error = $"No FAQ found with ID {id}." });
            return Ok(faq);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new FAQ.
    /// </summary>
    /// <param name="request">The FAQ creation request payload.</param>
    /// <returns>The created FAQ DTO.</returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost]
    public async Task<IActionResult> CreateFAQ([FromBody] FAQCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var faq = await _faqService.CreateAsync(request);
            return CreatedAtAction(nameof(GetFAQById), new { id = faq?.Id }, faq);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing FAQ.
    /// </summary>
    /// <param name="request">The FAQ update request payload.</param>
    /// <returns>The updated FAQ DTO.</returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateFAQ([FromBody] FAQUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedFAQ = await _faqService.UpdateAsync(request);
            if (updatedFAQ == null) return NotFound(new { error = "FAQ not found." });
            return Ok(updatedFAQ);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes an FAQ by its ID.
    /// </summary>
    /// <param name="id">The ID of the FAQ to delete.</param>
    /// <returns>No content when successful.</returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFAQ([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "FAQ ID is required." });

        try
        {
            await _faqService.DeleteAsync(id);
            return NoContent();
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
}