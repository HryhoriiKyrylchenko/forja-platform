namespace Forja.API.Controllers.UserProfile;

/// <summary>
/// Handles CRUD operations for game saves within the system.
/// </summary>
/// <remarks>
/// This controller provides endpoints to manage game save data, including creating,
/// retrieving, updating, and deleting game saves. All operations are performed through
/// interaction with the IGameSaveService.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class GameSaveController : ControllerBase
{
    /// <summary>
    /// Dependency injection field for accessing the game save service functionality.
    /// </summary>
    private readonly IGameSaveService _gameSaveService;

    /// <summary>
    /// Controller responsible for handling GameSave-related API endpoints.
    /// </summary>
    public GameSaveController(IGameSaveService gameSaveService)
    {
        _gameSaveService = gameSaveService;
    }

    /// <summary>
    /// Retrieves all game save records from the system.
    /// </summary>
    /// <returns>
    /// An asynchronous operation that returns an HTTP ActionResult. On success, it contains a list of GameSaveDto
    /// objects representing all game saves. On failure, it includes error details.
    /// </returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet]
    public async Task<ActionResult<List<GameSaveDto>>> GetAll()
    {
        try
        {
            var gameSaves = await _gameSaveService.GetAllAsync();
            return Ok(gameSaves);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a game save by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game save.</param>
    /// <returns>
    /// An <see cref="ActionResult{T}"/> containing the <see cref="GameSaveDto"/> if found, or an appropriate status code with error information if not.
    /// </returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<GameSaveDto>> GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var gameSave = await _gameSaveService.GetByIdAsync(id);
            if (gameSave == null)
            {
                return NotFound(new { error = $"GameSave with ID {id} not found." });
            }

            return Ok(gameSave);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Adds a new game save to the system.
    /// </summary>
    /// <param name="request">A <see cref="GameSaveCreateRequest"/> object containing the details of the game save to be added.</param>
    /// <returns>A newly created <see cref="GameSaveDto"/> with details of the added game save, including its unique ID, or a <see cref="BadRequestResult"/> if the operation fails.</returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<GameSaveDto>> Add(GameSaveCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var addedGameSave = await _gameSaveService.AddAsync(request);
            if (addedGameSave == null)
            {
                return BadRequest(new { error = "Failed to create game save." });
            }
            return CreatedAtAction(nameof(GetById), new { id = addedGameSave.Id }, addedGameSave);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing game save with the specified details provided in the request.
    /// </summary>
    /// <param name="request">An object containing the updated game save information, including ID, name, user information, and file details.</param>
    /// <returns>A task representing the asynchronous operation. Upon success, it returns an <see cref="ActionResult{GameSaveDto}"/> containing the updated game save details. If the operation fails, it returns a bad request result with error information.</returns>
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<GameSaveDto>> Update(GameSaveUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedGameSave = await _gameSaveService.UpdateAsync(request);
            return Ok(updatedGameSave);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a game save by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game save to be deleted.</param>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            await _gameSaveService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}