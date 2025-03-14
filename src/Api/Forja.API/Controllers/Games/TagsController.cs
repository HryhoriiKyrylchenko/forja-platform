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

    public TagsController(ITagService tagService, IGameTagService gameTagService)
    {
        _tagService = tagService;
        _gameTagService = gameTagService;
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Creates a new tag.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing tag.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Deletes an existing tag.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Creates a new game tag.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing game tag.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Deletes an existing game tag.
    /// </summary>
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
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    #endregion
}