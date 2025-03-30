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
    private readonly IAuditLogService _auditLogService;
    private readonly IUserService _userService;
    private readonly IDistributedCache _cache;

    public GamesController(IGameService gameService, 
        IGameAddonService gameAddonService,
        IAnalyticsEventService analyticsEventService,
        IAuditLogService auditLogService,
        IUserService userService,
        IDistributedCache cache)
    {
        _gameService = gameService;
        _gameAddonService = gameAddonService;
        _analyticsEventService = analyticsEventService;
        _auditLogService = auditLogService;
        _userService = userService;
        _cache = cache;
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
            var cachedGames = await _cache.GetStringAsync("all_games");
            if (!string.IsNullOrWhiteSpace(cachedGames))
            {
                var allGames = JsonSerializer.Deserialize<List<GameDto>>(cachedGames);
                return Ok(allGames);
            }

            var games = await _gameService.GetAllAsync();
            var gamesList = games.ToList();
            if (!gamesList.Any()) return NoContent();
            
            var serializedData = JsonSerializer.Serialize(gamesList);

            await _cache.SetStringAsync("all_games", serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
            
            try
            {
                var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                UserProfileDto? user = null;
                if (!string.IsNullOrEmpty(keycloakUserId))
                {
                    user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
                }
                await _analyticsEventService.AddEventAsync(AnalyticEventType.PageView,
                    user?.Id,
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
            
            return Ok(games);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all games" }
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all deleted games" }
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
    /// Gets a game by its ID.
    /// </summary>
    [HttpGet("games/{id}")]
    public async Task<IActionResult> GetGameByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Game ID cannot be empty.");

        try
        {
            var cachedGame = await _cache.GetStringAsync($"game_{id}");
            if (!string.IsNullOrWhiteSpace(cachedGame))
            {
                var gameDto = JsonSerializer.Deserialize<GameDto>(cachedGame);
                return Ok(gameDto);
            }
            
            var cachedGames = await _cache.GetStringAsync("all_games");
            if (!string.IsNullOrWhiteSpace(cachedGames))
            {
                var allGames = JsonSerializer.Deserialize<List<GameDto>>(cachedGames);
                var gameDto = allGames?.FirstOrDefault(g => g.Id == id);
                if (gameDto != null) return Ok(gameDto);
            }
            
            var game = await _gameService.GetByIdAsync(id);
            if (game == null) return NotFound(new { error = $"Game with ID {id} not found." });
            
            var serializedData = JsonSerializer.Serialize(game);

            await _cache.SetStringAsync($"game_{id}", serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            });
            
            try
            {
                var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                UserProfileDto? user = null;
                if (!string.IsNullOrEmpty(keycloakUserId))
                {
                    user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
                }
                
                await _analyticsEventService.AddEventAsync(AnalyticEventType.PageView,
                    user?.Id,
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get game by id: {id}" }
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
            if (createdGame == null)
            {
                return BadRequest(new { error = "Failed to create game." });
            }
            
            try
            {
                var logEntry = new LogEntry<GameDto>
                {
                    State = createdGame,
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Create,
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Game created successfully" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return Ok(createdGame);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to create game with title: {request.Title}" }
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

            try
            {
                var logEntry = new LogEntry<GameDto>
                {
                    State = updatedGame,
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Game updated successfully" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return Ok(updatedGame);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update game with id: {request.Id}" }
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
            
            try
            {
                var logEntry = new LogEntry<GameDto>
                {
                    State = game,
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Delete,
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Game with id: {id} deleted successfully" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete game with id: {id}" }
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

    #region GameAddons Endpoints

    /// <summary>
    /// Gets all game addons.
    /// </summary>
    [HttpGet("game-addons")]
    public async Task<IActionResult> GetAllGameAddonsAsync()
    {
        try
        {
            var cachedGameAddons = await _cache.GetStringAsync("all_game_addons");
            if (!string.IsNullOrWhiteSpace(cachedGameAddons))
            {
                var allGameAddons = JsonSerializer.Deserialize<List<GameAddonDto>>(cachedGameAddons);
                return Ok(allGameAddons);
            }

            var addons = await _gameAddonService.GetAllAsync();
            var gameAddonsList = addons.ToList();
            if (!gameAddonsList.Any()) return NoContent();
            
            var serializedData = JsonSerializer.Serialize(gameAddonsList);

            await _cache.SetStringAsync("all_game_addons", serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            try
            {
                var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                UserProfileDto? user = null;
                if (!string.IsNullOrEmpty(keycloakUserId))
                {
                    user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
                }
                await _analyticsEventService.AddEventAsync(AnalyticEventType.PageView,
                    user?.Id,
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

            return Ok(addons);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get all game addons" }
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
    /// Gets a game addon by its ID.
    /// </summary>
    [HttpGet("game-addons/{id}")]
    public async Task<IActionResult> GetGameAddonByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "GameAddon ID cannot be empty." });

        try
        {
            var cachedAddon = await _cache.GetStringAsync($"addon_{id}");
            if (!string.IsNullOrWhiteSpace(cachedAddon))
            {
                var addonDto = JsonSerializer.Deserialize<GameDto>(cachedAddon);
                return Ok(addonDto);
            }
            
            var cachedAddons = await _cache.GetStringAsync("all_game_addons");
            if (!string.IsNullOrWhiteSpace(cachedAddons))
            {
                var allAddons = JsonSerializer.Deserialize<List<GameAddonDto>>(cachedAddons);
                var addonDto = allAddons?.FirstOrDefault(a => a.Id == id);
                if (addonDto != null) return Ok(addonDto);
            }
            
            var addon = await _gameAddonService.GetByIdAsync(id);
            if (addon == null) return NotFound(new { error = $"GameAddon with ID {id} not found." });
            
            var serializedData = JsonSerializer.Serialize(addon);

            await _cache.SetStringAsync($"addon_{id}", serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            });
            
            try
            {
                var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                UserProfileDto? user = null;
                if (!string.IsNullOrEmpty(keycloakUserId))
                {
                    user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
                }
                await _analyticsEventService.AddEventAsync(AnalyticEventType.PageView,
                    user?.Id,
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get game addon by id: {id}" }
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
                var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                UserProfileDto? user = null;
                if (!string.IsNullOrEmpty(keycloakUserId))
                {
                    user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
                }
                await _analyticsEventService.AddEventAsync(AnalyticEventType.PageView,
                    user?.Id,
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get game addon by game id: {id}" }
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
            if (createdAddon == null)
            {
                return BadRequest(new { error = "Failed to create game addon." });
            }

            try
            {
                var logEntry = new LogEntry<GameAddonDto>
                {
                    State = createdAddon,
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Create,
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Game addon created successfully" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }

            return Ok(createdAddon);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to create game addon: {request.Title}" }
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

            try
            {
                var logEntry = new LogEntry<GameAddonDto>
                {
                    State = updatedAddon,
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Game addon updated successfully" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return Ok(updatedAddon);
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update game addon with id: {request.Id}" }
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
            
            try
            {
                var logEntry = new LogEntry<GameAddonDto>
                {
                    State = addon,
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Delete,
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Game addon with id: {id} deleted successfully" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
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
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete game addon with id: {id}" }
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