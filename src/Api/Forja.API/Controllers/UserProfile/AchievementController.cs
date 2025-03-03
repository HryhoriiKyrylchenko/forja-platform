namespace Forja.API.Controllers.UserProfile;

[ApiController]
[Route("api/[controller]")]
public class AchievementController : ControllerBase
{
    private readonly IAchievementService _achievementService;
    private readonly IUserAchievementService _userAchievementService;

    public AchievementController(IAchievementService achievementService, IUserAchievementService userAchievementService)
    {
        _achievementService = achievementService;
        _userAchievementService = userAchievementService;
    }

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

    [HttpPut]
    public async Task<IActionResult> UpdateAchievement([FromBody] AchievementDto achievementDto)
    {
        await _achievementService.UpdateAchievementAsync(achievementDto);
        return NoContent();
    }

    [HttpDelete("{achievementId}")]
    public async Task<IActionResult> DeleteAchievement(Guid achievementId)
    {
        await _achievementService.DeleteAchievementAsync(achievementId);
        return NoContent();
    }

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

    [HttpGet("deleted")]
    public async Task<IActionResult> GetAllDeletedAchievements()
    {
        var result = await _achievementService.GetAllDeletedAchievementsAsync();
        return Ok(result);
    }

    [HttpPost("user-achievements")]
    public async Task<IActionResult> AddUserAchievement([FromBody] UserAchievementDto userAchievementDto)
    {
        await _userAchievementService.AddUserAchievementAsync(userAchievementDto);
        return NoContent();
    }

    [HttpGet("user-achievements/{userAchievementId}")]
    public async Task<IActionResult> GetUserAchievementById(Guid userAchievementId)
    {
        var result = await _userAchievementService.GetUserAchievementByIdAsync(userAchievementId);
        return Ok(result);
    }

    [HttpPut("user-achievements")]
    public async Task<IActionResult> UpdateUserAchievement([FromBody] UserAchievementDto userAchievementDto)
    {
        await _userAchievementService.UpdateUserAchievement(userAchievementDto);
        return NoContent();
    }

    [HttpDelete("user-achievements/{userAchievementId}")]
    public async Task<IActionResult> DeleteUserAchievement(Guid userAchievementId)
    {
        await _userAchievementService.DeleteUserAchievementAsync(userAchievementId);
        return NoContent();
    }

    [HttpGet("user-achievements/all")]
    public async Task<IActionResult> GetAllUserAchievements()
    {
        var result = await _userAchievementService.GetAllUserAchievementsAsync();
        return Ok(result);
    }

    [HttpGet("{keycloakId}/user-achievements")]
    public async Task<IActionResult> GetAllUserAchievementsByUserKeycloakId(string keycloakId)
    {
        var result = await _userAchievementService.GetAllUserAchievementsByUserKeycloakIdAsync(keycloakId);
        return Ok(result);
    }

    [HttpGet("games/{gameId}/user-achievements")]
    public async Task<IActionResult> GetAllUserAchievementsByGameId(Guid gameId)
    {
        var result = await _userAchievementService.GetAllUserAchievementsByGameIdAsync(gameId);
        return Ok(result);
    }
}