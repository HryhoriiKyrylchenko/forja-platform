namespace Forja.API.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
public class NewsArticleController : ControllerBase
{
    private readonly INewsArticleService _newsArticleService;

    public NewsArticleController(INewsArticleService newsArticleService)
    {
        _newsArticleService = newsArticleService;
    }
    
    /// <summary>
    /// Get a news article by its ID.
    /// </summary>
    [Authorize(Policy = "ArticleReadPolicy")]
    [HttpGet("{articleId:guid}")]
    public async Task<IActionResult> GetNewsArticleById([FromRoute] Guid articleId)
    {
        if (articleId == Guid.Empty) return BadRequest(new { error = "Article ID is required." });
        try
        {
            var article = await _newsArticleService.GetNewsArticleByIdAsync(articleId);
            if (article == null) return NotFound(new { error = $"No article found with ID {articleId}." });
            return Ok(article);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get news articles by publication date.
    /// </summary>
    [Authorize(Policy = "ArticleReadPolicy")]
    [HttpGet("date")]
    public async Task<IActionResult> GetNewsArticlesByPublicationDate([FromQuery] DateTime? publicationDate)
    {
        try
        {
            var articles = await _newsArticleService.GetNewsArticlesByPublicationDateAsync(publicationDate);
            return Ok(articles);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get active news articles.
    /// </summary>
    [Authorize(Policy = "ArticleReadPolicy")]
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveNewsArticles()
    {
        try
        {
            var articles = await _newsArticleService.GetActiveNewsArticlesAsync();
            return Ok(articles);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new news article.
    /// </summary>
    [Authorize(Policy = "ArticleWritePolicy")]
    [HttpPost]
    public async Task<IActionResult> CreateNewsArticle([FromBody] NewsArticleCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var createdArticle = await _newsArticleService.CreateNewsArticleAsync(request);
            return CreatedAtAction(nameof(GetNewsArticleById), new { articleId = createdArticle?.Id }, createdArticle);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing news article.
    /// </summary>
    [Authorize(Policy = "ArticleWritePolicy")]
    [HttpPut("{articleId:guid}")]
    public async Task<IActionResult> UpdateNewsArticle([FromRoute] Guid articleId, [FromBody] NewsArticleUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var updatedArticle = await _newsArticleService.UpdateNewsArticleAsync(request);
            return Ok(updatedArticle);
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

    /// <summary>
    /// Delete a news article by its ID.
    /// </summary>
    [Authorize(Policy = "ArticleWritePolicy")]
    [HttpDelete("{articleId:guid}")]
    public async Task<IActionResult> DeleteNewsArticle([FromRoute] Guid articleId)
    {
        if (articleId == Guid.Empty) return BadRequest(new { error = "Article ID is required." });
        try
        {
            await _newsArticleService.DeleteNewsArticleAsync(articleId);
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