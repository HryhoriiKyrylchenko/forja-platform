namespace Forja.API.Controllers.Games;

/// <summary>
/// Controller for managing Mechanics and GameMechanics associations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MechanicsController : ControllerBase
{
    private readonly IMechanicService _mechanicService;
    private readonly IGameMechanicService _gameMechanicService;
    private readonly IAuditLogService _auditLogService;

    public MechanicsController(IMechanicService mechanicService, 
        IGameMechanicService gameMechanicService,
        IAuditLogService auditLogService)
    {
        _mechanicService = mechanicService;
        _gameMechanicService = gameMechanicService;
        _auditLogService = auditLogService;
    }

    #region Mechanics Endpoints

    /// <summary>
    /// Gets all mechanics.
    /// </summary>
    [HttpGet("mechanics")]
    public async Task<IActionResult> GetAllMechanicsAsync()
    {
        try
        {
            var mechanics = await _mechanicService.GetAllAsync();
            if (!mechanics.Any())
            {
                return NoContent();
            }

            return Ok(mechanics);
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
                        { "Message", $"Failed to get all mechanics" }
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
    /// Gets all deleted mechanics.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("mechanics/deleted")]
    public async Task<IActionResult> GetAllDeletedMechanicsAsync()
    {
        try
        {
            var deletedMechanics = await _mechanicService.GetAllDeletedAsync();
            if (!deletedMechanics.Any())
            {
                return NoContent();
            }

            return Ok(deletedMechanics);
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
                        { "Message", $"Failed to get all deleted mechanics" }
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
    /// Gets a mechanic by its ID.
    /// </summary>
    [HttpGet("mechanics/{id}")]
    public async Task<IActionResult> GetMechanicByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The mechanic ID cannot be empty." });

        try
        {
            var mechanic = await _mechanicService.GetByIdAsync(id);
            if (mechanic == null)
                return NotFound(new { error = $"Mechanic with ID {id} not found." });

            return Ok(mechanic);
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
                        { "Message", $"Failed to get mechanic by id: {id}" }
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
    /// Creates a new mechanic.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("mechanics")]
    public async Task<IActionResult> CreateMechanicAsync([FromBody] MechanicCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdMechanic = await _mechanicService.CreateAsync(request);
            return createdMechanic != null ? Ok(createdMechanic) : BadRequest(new { error = "Failed to create mechanic." });
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
                        { "Message", $"Failed to create mechanic: {request.Name}" }
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
    /// Updates an existing mechanic.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("mechanics")]
    public async Task<IActionResult> UpdateMechanicAsync([FromBody] MechanicUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedMechanic = await _mechanicService.UpdateAsync(request);
            if (updatedMechanic == null)
                return NotFound(new { error = $"Mechanic with ID {request.Id} not found." });

            return Ok(updatedMechanic);
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
                        { "Message", $"Failed to update mechanic with Id: {request.Id}" }
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
    /// Deletes an existing mechanic.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("mechanics/{id}")]
    public async Task<IActionResult> DeleteMechanicAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The mechanic ID cannot be empty." });

        try
        {
            var mechanic = await _mechanicService.GetByIdAsync(id);
            if (mechanic == null)
                return NotFound(new { error = $"Mechanic with ID {id} not found." });

            await _mechanicService.DeleteAsync(id);
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
                        { "Message", $"Failed to delete mechanic with Id: {id}" }
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

    #region GameMechanics Endpoints

    /// <summary>
    /// Gets all game-mechanic associations.
    /// </summary>
    [HttpGet("game-mechanics")]
    public async Task<IActionResult> GetAllGameMechanicsAsync()
    {
        try
        {
            var gameMechanics = await _gameMechanicService.GetAllAsync();
            if (!gameMechanics.Any())
            {
                return NoContent();
            }

            return Ok(gameMechanics);
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
                        { "Message", $"Failed to get all mechanics" }
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
    /// Gets a game-mechanic association by its ID.
    /// </summary>
    [HttpGet("game-mechanics/{id}")]
    public async Task<IActionResult> GetGameMechanicByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The game-mechanic ID cannot be empty." });

        try
        {
            var gameMechanic = await _gameMechanicService.GetByIdAsync(id);
            if (gameMechanic == null)
                return NotFound(new { error = $"GameMechanic with ID {id} not found." });

            return Ok(gameMechanic);
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
                        { "Message", $"Failed to get all game mechanic by id: {id}" }
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
    /// Gets game-mechanic associations by Game ID.
    /// </summary>
    [HttpGet("game-mechanics/by-game/{gameId}")]
    public async Task<IActionResult> GetGameMechanicsByGameIdAsync([FromRoute] Guid gameId)
    {
        if (gameId == Guid.Empty)
            return BadRequest(new { error = "The game ID cannot be empty." });

        try
        {
            var gameMechanics = await _gameMechanicService.GetByGameIdAsync(gameId);
            if (!gameMechanics.Any())
                return NoContent();

            return Ok(gameMechanics);
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
                        { "Message", $"Failed to get game mechanic by game id: {gameId}" }
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
    /// Gets game-mechanic associations by Mechanic ID.
    /// </summary>
    [HttpGet("game-mechanics/by-mechanic/{mechanicId}")]
    public async Task<IActionResult> GetGameMechanicsByMechanicIdAsync([FromRoute] Guid mechanicId)
    {
        if (mechanicId == Guid.Empty)
            return BadRequest(new { error = "The mechanic ID cannot be empty." });

        try
        {
            var gameMechanics = await _gameMechanicService.GetByMechanicIdAsync(mechanicId);
            if (!gameMechanics.Any())
                return NoContent();

            return Ok(gameMechanics);
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
                        { "Message", $"Failed to get game mechanic by mechanic id: {mechanicId}" }
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
    /// Creates a new game-mechanic association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("game-mechanics")]
    public async Task<IActionResult> CreateGameMechanicAsync([FromBody] GameMechanicCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdGameMechanic = await _gameMechanicService.CreateAsync(request);
            return createdGameMechanic != null ? Ok(createdGameMechanic) : BadRequest(new { error = "Failed to create game-mechanic association." });
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
                        { "Message", $"Failed to create game mechanic: {request.MechanicId} for game: {request.GameId}" }
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
    /// Updates an existing game-mechanic association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("game-mechanics")]
    public async Task<IActionResult> UpdateGameMechanicAsync([FromBody] GameMechanicUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedGameMechanic = await _gameMechanicService.UpdateAsync(request);
            if (updatedGameMechanic == null)
                return NotFound(new { error = $"GameMechanic with ID {request.Id} not found." });

            return Ok(updatedGameMechanic);
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
                        { "Message", $"Failed to update game mechanic with id: {request.Id}" }
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
    /// Deletes an existing game-mechanic association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("game-mechanics/{id}")]
    public async Task<IActionResult> DeleteGameMechanicAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The game-mechanic ID cannot be empty." });

        try
        {
            var gameMechanic = await _gameMechanicService.GetByIdAsync(id);
            if (gameMechanic == null)
                return NotFound(new { error = $"GameMechanic with ID {id} not found." });

            await _gameMechanicService.DeleteAsync(id);
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
                        { "Message", $"Failed to delete game mechanic with id: {id}" }
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