namespace Forja.API.Controllers.UserProfile;

[ApiController]
[Route("api/[controller]")]
public class GameSaveController : ControllerBase
{
    private readonly IGameSaveService _gameSaveService;

    public GameSaveController(IGameSaveService gameSaveService)
    {
        _gameSaveService = gameSaveService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var gameSaves = await _gameSaveService.GetAllAsync();
        return Ok(gameSaves);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var gameSave = await _gameSaveService.GetByIdAsync(id);
        if (gameSave == null)
        {
            return NotFound();
        }

        return Ok(gameSave);
    }

    [HttpPost]
    public async Task<IActionResult> Add(GameSaveDto gameSave)
    {
        var addedGameSave = await _gameSaveService.AddAsync(gameSave);
        return CreatedAtAction(nameof(GetById), new { id = addedGameSave.Id }, addedGameSave);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, GameSaveDto gameSave)
    {
        if (id != gameSave.Id)
        {
            return BadRequest("Mismatched GameSave ID.");
        }

        var updatedGameSave = await _gameSaveService.UpdateAsync(gameSave);
        return Ok(updatedGameSave);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _gameSaveService.DeleteAsync(id);
        return NoContent();
    }
}