namespace Forja.API.Controllers.Games;

/// <summary>
/// Controller for managing Tags and GameTags in the games domain.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;
    private readonly IGameTagService _gameTagService;
    private readonly IAuditLogService _auditLogService;

    public TagsController(ITagService tagService, 
        IGameTagService gameTagService,
        IAuditLogService auditLogService)
    {
        _tagService = tagService;
        _gameTagService = gameTagService;
        _auditLogService = auditLogService;
    }

    #region Tag Endpoints

    /// <summary>
    /// Gets all tags.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllTagsAsync()
    {
        try
        {
            var tags = await _tagService.GetAllAsync();
            if (!tags.Any())
            {
                return NoContent();
            }

            return Ok(tags);
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
                        { "Message", $"Failed to get all tags" }
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
    /// Gets a tag by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTagByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The provided tag ID is invalid." });

        try
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null)
                return NotFound(new { error = $"Tag with ID {id} does not exist." });

            return Ok(tag);
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
                        { "Message", $"Failed to get tag by id: {id}" }
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
    /// Creates a new tag.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost]
    public async Task<IActionResult> CreateTagAsync([FromBody] TagCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdTag = await _tagService.CreateAsync(request);

            return createdTag != null ? Ok(createdTag) : BadRequest(new { error = "Failed to create tag." });
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
                        { "Message", $"Failed to create tag: {request.Title}" }
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
    /// Updates an existing tag.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateTagAsync([FromBody] TagUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedTag = await _tagService.UpdateAsync(request);
            if (updatedTag == null)
                return NotFound(new { error = $"Tag with ID {request.Id} does not exist." });

            return Ok(updatedTag);
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
                        { "Message", $"Failed to update tag: {request.Id}" }
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
    /// Deletes an existing tag.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTagAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The provided tag ID is invalid." });

        try
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null)
                return NotFound(new { error = $"Tag with ID {id} does not exist." });

            await _tagService.DeleteAsync(id);
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
                        { "Message", $"Failed to delete tag with id: {id}" }
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

    #region GameTag Endpoints

    /// <summary>
    /// Gets all game tags.
    /// </summary>
    [HttpGet("game-tags")]
    public async Task<IActionResult> GetAllGameTagsAsync()
    {
        try
        {
            var gameTags = await _gameTagService.GetAllAsync();
            if (!gameTags.Any())
            {
                return NoContent();
            }

            return Ok(gameTags);
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
                        { "Message", $"Failed to get all game tags" }
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
    /// Gets a game tag by its ID.
    /// </summary>
    [HttpGet("game-tags/{id}")]
    public async Task<IActionResult> GetGameTagByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The provided game tag ID is invalid." });

        try
        {
            var gameTag = await _gameTagService.GetByIdAsync(id);
            if (gameTag == null)
                return NotFound(new { error = $"GameTag with ID {id} does not exist." });

            return Ok(gameTag);
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
                        { "Message", $"Failed to get game tag by id: {id}" }
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
    /// Gets all game tags associated with a specific game ID.
    /// </summary>
    [HttpGet("game-tags/by-game/{gameId}")]
    public async Task<IActionResult> GetGameTagsByGameIdAsync([FromRoute] Guid gameId)
    {
        if (gameId == Guid.Empty)
            return BadRequest(new { error = "The provided game ID is invalid." });

        try
        {
            var gameTags = await _gameTagService.GetByGameIdAsync(gameId);
            if (!gameTags.Any())
                return NoContent();

            return Ok(gameTags);
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
                        { "Message", $"Failed to get game tag by game id: {gameId}" }
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
    /// Gets all game tags associated with a specific tag ID.
    /// </summary>
    [HttpGet("game-tags/by-tag/{tagId}")]
    public async Task<IActionResult> GetGameTagsByTagIdAsync([FromRoute] Guid tagId)
    {
        if (tagId == Guid.Empty)
            return BadRequest(new { error = "The provided tag ID is invalid." });

        try
        {
            var gameTags = await _gameTagService.GetByTagIdAsync(tagId);
            if (!gameTags.Any())
                return NoContent();

            return Ok(gameTags);
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
                        { "Message", $"Failed to get game tag by tag id: {tagId}" }
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
    /// Creates a new game tag.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("game-tags")]
    public async Task<IActionResult> CreateGameTagAsync([FromBody] GameTagCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdGameTag = await _gameTagService.CreateAsync(request);
            
            return createdGameTag != null ? Ok(createdGameTag) : BadRequest(new { error = "Failed to create game tag." });
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
                        { "Message", $"Failed to create game tag with game id: {request.GameId}, tag id: {request.TagId}" }
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
    /// Updates an existing game tag.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("game-tags")]
    public async Task<IActionResult> UpdateGameTagAsync([FromBody] GameTagUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedGameTag = await _gameTagService.UpdateAsync(request);
            if (updatedGameTag == null)
                return NotFound($"GameTag with ID {request.Id} does not exist.");

            return Ok(updatedGameTag);
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
                        { "Message", $"Failed to update game tag with id: {request.Id}" }
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
    /// Deletes an existing game tag.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("game-tags/{id}")]
    public async Task<IActionResult> DeleteGameTagAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("The provided game tag ID is invalid.");

        try
        {
            var gameTag = await _gameTagService.GetByIdAsync(id);
            if (gameTag == null)
                return NotFound($"GameTag with ID {id} does not exist.");

            await _gameTagService.DeleteAsync(id);
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
                        { "Message", $"Failed to delete game tag with id: {id}" }
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