namespace Forja.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserLibraryController : ControllerBase
{
    private readonly UserProfileService _userService;

    public UserLibraryController(UserProfileService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }
    
    [HttpPost("userlibrarygames")]
    public async Task<IActionResult> AddUserLibraryGame([FromBody] UserLibraryGameDto userLibraryGameDto)
    {
        await _userService.AddUserLibraryGameAsync(userLibraryGameDto);
        return NoContent();
    }

    [HttpPut("userlibrarygames")]
    public async Task<IActionResult> UpdateUserLibraryGame([FromBody] UserLibraryGameDto userLibraryGameDto)
    {
        await _userService.UpdateUserLibraryGameAsync(userLibraryGameDto);
        return NoContent();
    }

    [HttpDelete("userlibrarygames/{userLibraryGameId}")]
    public async Task<IActionResult> DeleteUserLibraryGame(Guid userLibraryGameId)
    {
        await _userService.DeleteUserLibraryGameAsync(userLibraryGameId);
        return NoContent();
    }

    [HttpPost("userlibrarygames/{userLibraryGameId}/restore")]
    public async Task<IActionResult> RestoreUserLibraryGame(Guid userLibraryGameId)
    {
        var result = await _userService.RestoreUserLibraryGameAsync(userLibraryGameId);
        return Ok(result);
    }

    [HttpGet("userlibrarygames/{userLibraryGameId}")]
    public async Task<IActionResult> GetUserLibraryGameById(Guid userLibraryGameId)
    {
        var result = await _userService.GetUserLibraryGameByIdAsync(userLibraryGameId);
        return Ok(result);
    }

    [HttpGet("userlibrarygames/{userLibraryGameId}/deleted")]
    public async Task<IActionResult> GetDeletedUserLibraryGameById(Guid userLibraryGameId)
    {
        var result = await _userService.GetDeletedUserLibraryGameByIdAsync(userLibraryGameId);
        return Ok(result);
    }

    [HttpGet("userlibrarygames/all")]
    public async Task<IActionResult> GetAllUserLibraryGames()
    {
        var result = await _userService.GetAllUserLibraryGamesAsync();
        return Ok(result);
    }

    [HttpGet("userlibrarygames/deleted")]
    public async Task<IActionResult> GetAllDeletedUserLibraryGames()
    {
        var result = await _userService.GetAllDeletedUserLibraryGamesAsync();
        return Ok(result);
    }

    [HttpGet("{keycloakId}/userlibrarygames")]
    public async Task<IActionResult> GetAllUserLibraryGamesByUserKeycloakId(string keycloakId)
    {
        var result = await _userService.GetAllUserLibraryGamesByUserKeycloakIdAsync(keycloakId);
        return Ok(result);
    }

    [HttpGet("{keycloakId}/userlibrarygames/deleted")]
    public async Task<IActionResult> GetAllDeletedUserLibraryGamesByUserKeycloakId(string keycloakId)
    {
        var result = await _userService.GetAllDeletedUserLibraryGamesByUserKeycloakIdAsync(keycloakId);
        return Ok(result);
    }

    [HttpPost("userlibraryaddons")]
    public async Task<IActionResult> AddUserLibraryAddon([FromBody] UserLibraryAddonDto userLibraryAddonDto)
    {
        await _userService.AddUserLibraryAddonAsync(userLibraryAddonDto);
        return NoContent();
    }

    [HttpPut("userlibraryaddons")]
    public async Task<IActionResult> UpdateUserLibraryAddon([FromBody] UserLibraryAddonDto userLibraryAddonDto)
    {
        await _userService.UpdateUserLibraryAddonAsync(userLibraryAddonDto);
        return NoContent();
    }

    [HttpDelete("userlibraryaddons/{userLibraryAddonId}")]
    public async Task<IActionResult> DeleteUserLibraryAddon(Guid userLibraryAddonId)
    {
        await _userService.DeleteUserLibraryAddonAsync(userLibraryAddonId);
        return NoContent();
    }

    [HttpPost("userlibraryaddons/{userLibraryAddonId}/restore")]
    public async Task<IActionResult> RestoreUserLibraryAddon(Guid userLibraryAddonId)
    {
        var result = await _userService.RestoreUserLibraryAddonAsync(userLibraryAddonId);
        return Ok(result);
    }

    [HttpGet("userlibraryaddons/{userLibraryAddonId}")]
    public async Task<IActionResult> GetUserLibraryAddonById(Guid userLibraryAddonId)
    {
        var result = await _userService.GetUserLibraryAddonByIdAsync(userLibraryAddonId);
        return Ok(result);
    }

    [HttpGet("userlibraryaddons/{userLibraryAddonId}/deleted")]
    public async Task<IActionResult> GetDeletedUserLibraryAddonById(Guid userLibraryAddonId)
    {
        var result = await _userService.GetDeletedUserLibraryAddonByIdAsync(userLibraryAddonId);
        return Ok(result);
    }

    [HttpGet("userlibraryaddons/all")]
    public async Task<IActionResult> GetAllUserLibraryAddons()
    {
        var result = await _userService.GetAllUserLibraryAddonsAsync();
        return Ok(result);
    }

    [HttpGet("userlibraryaddons/deleted")]
    public async Task<IActionResult> GetAllDeletedUserLibraryAddons()
    {
        var result = await _userService.GetAllDeletedUserLibraryAddonsAsync();
        return Ok(result);
    }

    [HttpGet("games/{gameId}/userlibraryaddons")]
    public async Task<IActionResult> GetAllUserLibraryAddonsByGameId(Guid gameId)
    {
        var result = await _userService.GetAllUserLibraryAddonsByGameIdAsync(gameId);
        return Ok(result);
    }

    [HttpGet("games/{gameId}/userlibraryaddons/deleted")]
    public async Task<IActionResult> GetAllDeletedUserLibraryAddonsByGameId(Guid gameId)
    {
        var result = await _userService.GetAllDeletedUserLibraryAddonsByGameIdAsync(gameId);
        return Ok(result);
    }
}