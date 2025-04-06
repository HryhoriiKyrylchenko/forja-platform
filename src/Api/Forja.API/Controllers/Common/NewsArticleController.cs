namespace Forja.API.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
public class NewsArticleController : ControllerBase
{
    private readonly INewsArticleService _newsArticleService;
    private readonly IAuditLogService _auditLogService;
    private readonly IDistributedCache _cache;

    public NewsArticleController(INewsArticleService newsArticleService,
        IAuditLogService auditLogService,
        IDistributedCache cache)
    {
        _newsArticleService = newsArticleService;
        _auditLogService = auditLogService;
        _cache = cache;
    }
    
    /// <summary>
    /// Get a news article by its ID.
    /// </summary>
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
                        { "Message", $"Failed to get news article by id: {articleId}" }
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
    /// Get news articles by publication date.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
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
                        { "Message", $"Failed to get news articles by publication date: {publicationDate}" }
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
    /// Get active news articles.
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveNewsArticles(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var cachedNewsArticles = await _cache.GetStringAsync("all_active_news_articles");
            List<NewsArticleDto>? allNewsArticles;
            if (!string.IsNullOrWhiteSpace(cachedNewsArticles))
            {
                allNewsArticles = JsonSerializer.Deserialize<List<NewsArticleDto>>(cachedNewsArticles);
            }
            else
            {
                allNewsArticles = await _newsArticleService.GetActiveNewsArticlesAsync();
                if (!allNewsArticles.Any()) return NoContent();
                
                var serializedData = JsonSerializer.Serialize(allNewsArticles);

                await _cache.SetStringAsync("all_active_news_articles", serializedData, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });
            }
            
            if (allNewsArticles == null) return NoContent();
            
            var pagedArticles = allNewsArticles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                PaginatedResult = new PaginatedResult<NewsArticleDto>(
                    pagedArticles,
                    allNewsArticles.Count,
                    pageNumber,
                    pageSize),
                PrioritizedNewsArticles = allNewsArticles.Where(a => a.IsPrioritized)
            });
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
                        { "Message", $"Failed to get active news articles" }
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
    /// Create a new news article.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
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
                        { "Message", $"Failed to create news article {request.Title}" }
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
    /// Update an existing news article.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("{articleId:guid}")]
    public async Task<IActionResult> UpdateNewsArticle([FromBody] NewsArticleUpdateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var updatedArticle = await _newsArticleService.UpdateNewsArticleAsync(request);
            return Ok(updatedArticle);
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
                        { "Message", $"Failed to update news article {request.Id}" }
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
                        { "Message", $"Failed to update news article {request.Id}" }
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
    /// Delete a news article by its ID.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
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
                        { "Message", $"Failed to delete news article {articleId}" }
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
                        { "Message", $"Failed to delete news article {articleId}" }
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