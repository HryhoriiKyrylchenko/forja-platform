namespace Forja.API.Controllers.Games;

/// <summary>
/// Controller for managing Games and GameAddons.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly IGameAddonService _gameAddonService;
    private readonly IAnalyticsEventService _analyticsEventService;

    public GamesController(IGameService gameService, 
        IGameAddonService gameAddonService,
        IAnalyticsEventService analyticsEventService)
    {
        _gameService = gameService;
        _gameAddonService = gameAddonService;
        _analyticsEventService = analyticsEventService;
    }

    #region Games Endpoints

    /// <summary>
    /// Gets all games.
    /// </summary>
    [HttpGet("games")]
    public async Task<IActionResult> GetAllGamesAsync()
    {
        try
        {
            var games = await _gameService.GetAllAsync();
            
            try
            {
                await _analyticsEventService.AddEventAsync(AnalyticEventType.PageView,
                    null,
                    new Dictionary<string, string>
                    {
                        { "Page", "AllGames" },
                        { "Date", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) }
                    });
            }
            catch (Exception)
            {
                Console.WriteLine("Analytics event creation failed.");
            }
    
            if (!games.Any()) return NoContent();
            
            return Ok(games);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Gets all deleted games.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("games/deleted")]
    public async Task<IActionResult> GetAllDeletedGamesAsync()
    {
        try
        {
            var deletedGames = await _gameService.GetAllDeletedAsync();
            if (!deletedGames.Any())
                return NoContent();

            return Ok(deletedGames);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Gets a game by its ID.
    /// </summary>
    [HttpGet("games/{id}")]
    public async Task<IActionResult> GetGameByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Game ID cannot be empty.");

        try
        {
            var game = await _gameService.GetByIdAsync(id);
            if (game == null) return NotFound(new { error = $"Game with ID {id} not found." });
            
            try
            {
                await _analyticsEventService.AddEventAsync(AnalyticEventType.PageView,
                    null,
                    new Dictionary<string, string>
                    {
                        { "Page", "Game" },
                        { "GameId", game.Id.ToString() },
                        { "GameTitle", game.Title },
                        { "Date", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) }
                    });
            }
            catch (Exception)
            {
                Console.WriteLine("Analytics event creation failed.");
            }

            return Ok(game);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Creates a new game.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("games")]
    public async Task<IActionResult> CreateGameAsync([FromBody] GameCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdGame = await _gameService.AddAsync(request);
            
            return createdGame != null ? Ok(createdGame) : BadRequest(new { error = "Failed to create game." });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing game.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("games")]
    public async Task<IActionResult> UpdateGameAsync([FromBody] GameUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedGame = await _gameService.UpdateAsync(request);
            if (updatedGame == null)
                return NotFound(new { error = $"Game with ID {request.Id} not found." });

            return Ok(updatedGame);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Deletes a game by its ID.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("games/{id}")]
    public async Task<IActionResult> DeleteGameAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "Game ID cannot be empty." });

        try
        {
            var game = await _gameService.GetByIdAsync(id);
            if (game == null)
                return NotFound(new { error = $"Game with ID {id} not found." });

            await _gameService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    #endregion

    #region GameAddons Endpoints

    /// <summary>
    /// Gets all game addons.
    /// </summary>
    [HttpGet("game-addons")]
    public async Task<IActionResult> GetAllGameAddonsAsync()
    {
        try
        {
            var addons = await _gameAddonService.GetAllAsync();
            
            try
            {
                await _analyticsEventService.AddEventAsync(AnalyticEventType.PageView,
                    null,
                    new Dictionary<string, string>
                    {
                        { "Page", "AllAddons" },
                        { "Date", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) }
                    });
            }
            catch (Exception)
            {
                Console.WriteLine("Analytics event creation failed.");
            }
            
            if (!addons.Any()) return NoContent();

            return Ok(addons);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Gets a game addon by its ID.
    /// </summary>
    [HttpGet("game-addons/{id}")]
    public async Task<IActionResult> GetGameAddonByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "GameAddon ID cannot be empty." });

        try
        {
            var addon = await _gameAddonService.GetByIdAsync(id);
            if (addon == null) return NotFound(new { error = $"GameAddon with ID {id} not found." });
            
            try
            {
                await _analyticsEventService.AddEventAsync(AnalyticEventType.PageView,
                    null,
                    new Dictionary<string, string>
                    {
                        { "Page", "Addon" },
                        { "AddonId", addon.Id.ToString() },
                        { "AddonTitle", addon.Title },
                        { "RelatedGameId", addon.GameId.ToString() },
                        { "Date", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) }
                    });
            }
            catch (Exception)
            {
                Console.WriteLine("Analytics event creation failed.");
            }

            return Ok(addon);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
    
    [HttpGet("game-addons/games/{id}")]
    public async Task<IActionResult> GetGameAddonsByGameIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "Game ID cannot be empty." });

        try
        {
            var addons = await _gameAddonService.GetByGameIdAsync(id);
            
            try
            {
                await _analyticsEventService.AddEventAsync(AnalyticEventType.PageView,
                    null,
                    new Dictionary<string, string>
                    {
                        { "Page", "AllAddonsByGame" },
                        { "GameId", id.ToString() },
                        { "Date", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) }
                    });
            }
            catch (Exception)
            {
                Console.WriteLine("Analytics event creation failed.");
            }

            if (!addons.Any()) return NoContent();
            return Ok(addons);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Creates a new game addon.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("game-addons")]
    public async Task<IActionResult> CreateGameAddonAsync([FromBody] GameAddonCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdAddon = await _gameAddonService.CreateAsync(request);
            
            return createdAddon != null ? Ok(createdAddon) : BadRequest(new { error = "Failed to create game addon." });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates a game addon by its ID.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("game-addons")]
    public async Task<IActionResult> UpdateGameAddonAsync([FromBody] GameAddonUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedAddon = await _gameAddonService.UpdateAsync(request);
            if (updatedAddon == null)
                return NotFound(new { error = $"GameAddon with ID {request.Id} not found." });

            return Ok(updatedAddon);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Deletes a game addon by its ID.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("game-addons/{id}")]
    public async Task<IActionResult> DeleteGameAddonAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("GameAddon ID cannot be empty.");

        try
        {
            var addon = await _gameAddonService.GetByIdAsync(id);
            if (addon == null)
                return NotFound($"GameAddon with ID {id} not found.");

            await _gameAddonService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    #endregion
}