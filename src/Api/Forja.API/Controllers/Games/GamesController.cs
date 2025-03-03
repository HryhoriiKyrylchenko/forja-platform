namespace Forja.API.Controllers.Games;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;
        
    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }
        
    // GET: api/games
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var games = await _gameService.GetAllGamesAsync();
        return Ok(games);
    }
        
    // GET: api/games/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var game = await _gameService.GetGameByIdAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(game);
    }
        
    // POST: api/games
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Game game)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
            
        await _gameService.CreateGameAsync(game);
        // Return a 201 Created response with a link to the new resource.
        return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
    }
}