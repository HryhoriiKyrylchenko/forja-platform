namespace Forja.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AchievementController : ControllerBase
{
    private readonly UserProfileService _userService;

    public AchievementController(UserProfileService userService)
    {
        _userService = userService;
    }

    [HttpPost("{keycloakId}")]
    public async Task<IActionResult> AddAchievement(string keycloakId, [FromBody] AchievementDto achievementDto)
    {
        await _userService.AddAchievementAsync(keycloakId, achievementDto);
        return NoContent();
    }

    [HttpGet("{achievementId}")]
    public async Task<IActionResult> GetAchievementById(Guid achievementId)
    {
        var result = await _userService.GetAchievementByIdAsync(achievementId);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAchievement([FromBody] AchievementDto achievementDto)
    {
        await _userService.UpdateAchievementAsync(achievementDto);
        return NoContent();
    }

    [HttpDelete("{achievementId}")]
    public async Task<IActionResult> DeleteAchievement(Guid achievementId)
    {
        await _userService.DeleteAchievementAsync(achievementId);
        return NoContent();
    }

    [HttpPost("{achievementId}/restore")]
    public async Task<IActionResult> RestoreAchievement(Guid achievementId)
    {
        var result = await _userService.RestoreAchievementAsync(achievementId);
        return Ok(result);
    }

    [HttpGet("game/{gameId}/all")]
    public async Task<IActionResult> GetAllGameAchievements(Guid gameId)
    {
        var result = await _userService.GetAllGameAchievementsAsync(gameId);
        return Ok(result);
    }
    
    [HttpGet("games/{gameId}/deleted")]
    public async Task<IActionResult> GetAllGameDeletedAchievements(Guid gameId)
    {
        var result = await _userService.GetAllGameDeletedAchievementsAsync(gameId);
        return Ok(result);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAchievements()
    {
        var result = await _userService.GetAllAchievementsAsync();
        return Ok(result);
    }

    [HttpGet("deleted")]
    public async Task<IActionResult> GetAllDeletedAchievements()
    {
        var result = await _userService.GetAllDeletedAchievementsAsync();
        return Ok(result);
    }

    [HttpPost("user-achievements")]
    public async Task<IActionResult> AddUserAchievement([FromBody] UserAchievementDto userAchievementDto)
    {
        await _userService.AddUserAchievementAsync(userAchievementDto);
        return NoContent();
    }

    [HttpGet("user-achievements/{userAchievementId}")]
    public async Task<IActionResult> GetUserAchievementById(Guid userAchievementId)
    {
        var result = await _userService.GetUserAchievementByIdAsync(userAchievementId);
        return Ok(result);
    }

    [HttpPut("user-achievements")]
    public async Task<IActionResult> UpdateUserAchievement([FromBody] UserAchievementDto userAchievementDto)
    {
        await _userService.UpdateUserAchievement(userAchievementDto);
        return NoContent();
    }

    [HttpDelete("user-achievements/{userAchievementId}")]
    public async Task<IActionResult> DeleteUserAchievement(Guid userAchievementId)
    {
        await _userService.DeleteUserAchievementAsync(userAchievementId);
        return NoContent();
    }

    [HttpGet("user-achievements/all")]
    public async Task<IActionResult> GetAllUserAchievements()
    {
        var result = await _userService.GetAllUserAchievementsAsync();
        return Ok(result);
    }

    [HttpGet("{keycloakId}/user-achievements")]
    public async Task<IActionResult> GetAllUserAchievementsByUserKeycloakId(string keycloakId)
    {
        var result = await _userService.GetAllUserAchievementsByUserKeycloakIdAsync(keycloakId);
        return Ok(result);
    }

    [HttpGet("games/{gameId}/user-achievements")]
    public async Task<IActionResult> GetAllUserAchievementsByGameId(Guid gameId)
    {
        var result = await _userService.GetAllUserAchievementsByGameIdAsync(gameId);
        return Ok(result);
    }
}