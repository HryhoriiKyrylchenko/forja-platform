namespace Forja.API.Controllers.UserProfile;

[ApiController]
[Route("api/[controller]")]
public class AchievementController : ControllerBase
{
    private readonly IAchievementService _achievementService;

    public AchievementController(IAchievementService achievementService)
    {
        _achievementService = achievementService;
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("{keycloakId}")]
    public async Task<IActionResult> AddAchievement(string keycloakId, [FromBody] AchievementDto achievementDto)
    {
        await _achievementService.AddAchievementAsync(keycloakId, achievementDto);
        return NoContent();
    }

    [HttpGet("{achievementId}")]
    public async Task<IActionResult> GetAchievementById(Guid achievementId)
    {
        var result = await _achievementService.GetAchievementByIdAsync(achievementId);
        return Ok(result);
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateAchievement([FromBody] AchievementDto achievementDto)
    {
        await _achievementService.UpdateAchievementAsync(achievementDto);
        return NoContent();
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("{achievementId}")]
    public async Task<IActionResult> DeleteAchievement(Guid achievementId)
    {
        await _achievementService.DeleteAchievementAsync(achievementId);
        return NoContent();
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("{achievementId}/restore")]
    public async Task<IActionResult> RestoreAchievement(Guid achievementId)
    {
        var result = await _achievementService.RestoreAchievementAsync(achievementId);
        return Ok(result);
    }

    [HttpGet("game/{gameId}/all")]
    public async Task<IActionResult> GetAllGameAchievements(Guid gameId)
    {
        var result = await _achievementService.GetAllGameAchievementsAsync(gameId);
        return Ok(result);
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("games/{gameId}/deleted")]
    public async Task<IActionResult> GetAllGameDeletedAchievements(Guid gameId)
    {
        var result = await _achievementService.GetAllGameDeletedAchievementsAsync(gameId);
        return Ok(result);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAchievements()
    {
        var result = await _achievementService.GetAllAchievementsAsync();
        return Ok(result);
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("deleted")]
    public async Task<IActionResult> GetAllDeletedAchievements()
    {
        var result = await _achievementService.GetAllDeletedAchievementsAsync();
        return Ok(result);
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("user-achievements")]
    public async Task<IActionResult> AddUserAchievement([FromBody] UserAchievementDto userAchievementDto)
    {
        await _achievementService.AddUserAchievementAsync(userAchievementDto);
        return NoContent();
    }

    [HttpGet("user-achievements/{userAchievementId}")]
    public async Task<IActionResult> GetUserAchievementById(Guid userAchievementId)
    {
        var result = await _achievementService.GetUserAchievementByIdAsync(userAchievementId);
        return Ok(result);
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("user-achievements")]
    public async Task<IActionResult> UpdateUserAchievement([FromBody] UserAchievementDto userAchievementDto)
    {
        await _achievementService.UpdateUserAchievement(userAchievementDto);
        return NoContent();
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("user-achievements/{userAchievementId}")]
    public async Task<IActionResult> DeleteUserAchievement(Guid userAchievementId)
    {
        await _achievementService.DeleteUserAchievementAsync(userAchievementId);
        return NoContent();
    }

    [HttpGet("user-achievements/all")]
    public async Task<IActionResult> GetAllUserAchievements()
    {
        var result = await _achievementService.GetAllUserAchievementsAsync();
        return Ok(result);
    }

    [HttpGet("{keycloakId}/user-achievements")]
    public async Task<IActionResult> GetAllUserAchievementsByUserKeycloakId(string keycloakId)
    {
        var result = await _achievementService.GetAllUserAchievementsByUserKeycloakIdAsync(keycloakId);
        return Ok(result);
    }

    [HttpGet("games/{gameId}/user-achievements")]
    public async Task<IActionResult> GetAllUserAchievementsByGameId(Guid gameId)
    {
        var result = await _achievementService.GetAllUserAchievementsByGameIdAsync(gameId);
        return Ok(result);
    }
}