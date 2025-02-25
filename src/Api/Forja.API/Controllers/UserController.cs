namespace Forja.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserProfileService _userService;

    public UserController(UserProfileService userService)
    {
        _userService = userService;
    }

    [HttpGet("{keycloakId}")]
    public async Task<IActionResult> GetUserProfile(string keycloakId)
    {
        var result = await _userService.GetUserProfileAsync(keycloakId);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileDto userProfileDto)
    {
        await _userService.UpdateUserProfileAsync(userProfileDto);
        return NoContent();
    }

    [HttpDelete("{keycloakId}")]
    public async Task<IActionResult> DeleteUser(string keycloakId)
    {
        await _userService.DeleteUserAsync(keycloakId);
        return NoContent();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(result);
    }

    [HttpGet("deleted")]
    public async Task<IActionResult> GetAllDeletedUsers()
    {
        var result = await _userService.GetAllDeletedUsersAsync();
        return Ok(result);
    }
}