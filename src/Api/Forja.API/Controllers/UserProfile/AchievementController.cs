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

    /// <summary>
    /// Adds a new achievement with the provided details.
    /// </summary>
    /// <param name="request">
    /// An instance of <see cref="AchievementCreateRequest"/> containing the required data to create a new achievement.
    /// </param>
    /// <returns>
    /// An <see cref="ActionResult"/> indicating the result of the operation. Returns a status of 200 (OK) along with the created achievement details
    /// if the process is successful, or a status of 400 (Bad Request) if the provided data is invalid.
    /// </returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost]
    public async Task<ActionResult<AchievementDto>> AddAchievement([FromBody] AchievementCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    
        try
        {
            var result = await _achievementService.AddAchievementAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves the details of an achievement based on the provided achievement ID.
    /// </summary>
    /// <param name="achievementId">
    /// The unique identifier of the achievement to retrieve.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the requested achievement details if the operation is successful.
    /// Returns a status of 200 (OK) with the achievement data, or a status of 400 (Bad Request) if the provided ID is invalid.
    /// </returns>
    [HttpGet("{achievementId}")]
    public async Task<ActionResult<AchievementDto>> GetAchievementById([FromRoute] Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid achievement ID." });
        }
        
        try
        {
            var result = await _achievementService.GetAchievementByIdAsync(achievementId);
            if (result == null)
            {
                return NotFound(new { error = "Achievement not found." });
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing achievement based on the provided request data.
    /// </summary>
    /// <param name="request">
    /// An instance of <see cref="AchievementUpdateRequest"/> containing the updated details for the achievement.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the outcome of the update operation.
    /// If successful, returns a status of 200 (OK) with a message indicating the success of the update.
    /// If unsuccessful, returns a status of 400 (Bad Request) with error details.
    /// </returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateAchievement([FromBody] AchievementUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _achievementService.UpdateAchievementAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes an existing achievement identified by its unique ID.
    /// </summary>
    /// <param name="achievementId">
    /// The unique identifier of the achievement to be deleted.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the outcome of the operation.
    /// Returns a status of 200 (OK) if the achievement is successfully deleted.
    /// Returns a status of 400 (Bad Request) if the ID is invalid or an error occurs during the deletion process.
    /// </returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("{achievementId}")]
    public async Task<IActionResult> DeleteAchievement([FromRoute] Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid achievement ID." });
        }
        
        try
        {
            await _achievementService.DeleteAchievementAsync(achievementId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Restores a previously deleted achievement identified by the provided achievement ID.
    /// </summary>
    /// <param name="achievementId">
    /// The unique identifier of the achievement to be restored.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the result of the restoration operation.
    /// If successful, returns a status of 200 (OK) with the restored achievement details.
    /// In case of errors, returns an appropriate error message and status code.
    /// </returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("{achievementId}/restore")]
    public async Task<ActionResult<AchievementDto>> RestoreAchievement([FromRoute] Guid achievementId)
    {
        if (achievementId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid achievement ID." });
        }
        
        try
        {
            var result = await _achievementService.RestoreAchievementAsync(achievementId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all achievements associated with a specific game based on the provided game ID.
    /// </summary>
    /// <param name="gameId">
    /// The unique identifier of the game whose achievements are to be retrieved.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a collection of <see cref="AchievementDto"/> objects representing the achievements.
    /// If successful, returns a status of 200 (OK) with the list of achievements.
    /// If the game ID is invalid or an error occurs, returns an appropriate error response.
    /// </returns>
    [HttpGet("game/{gameId}/all")]
    public async Task<ActionResult<List<AchievementDto>>> GetAllGameAchievements([FromRoute] Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid game ID." });
        }

        try
        {
            var result = await _achievementService.GetAllGameAchievementsAsync(gameId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all deleted achievements associated with a specific game.
    /// </summary>
    /// <param name="gameId">
    /// The unique identifier for the game whose deleted achievements are to be retrieved.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a list of deleted achievements for the specified game.
    /// If successful, returns a status of 200 (OK) with the data.
    /// If the game ID is invalid, returns a 400 (Bad Request) status with an appropriate error message.
    /// </returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("games/{gameId}/deleted")]
    public async Task<ActionResult<List<AchievementDto>>> GetAllGameDeletedAchievements([FromRoute] Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid game ID." });
        }

        try
        {
            var result = await _achievementService.GetAllGameDeletedAchievementsAsync(gameId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a list of all achievements stored in the system.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a list of all achievements.
    /// If successful, returns a status of 200 (OK) with the retrieved data.
    /// If an error occurs, returns a status of 400 (Bad Request) with an error message.
    /// </returns>
    [HttpGet("all")]
    public async Task<ActionResult<List<AchievementDto>>> GetAllAchievements()
    {
        try
        {
            var result = await _achievementService.GetAllAchievementsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a list of all deleted achievements.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a list of deleted achievements if the operation is successful.
    /// If an error occurs, returns a status of 400 (Bad Request) with an error message.
    /// </returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("deleted")]
    public async Task<ActionResult<List<AchievementDto>>> GetAllDeletedAchievements()
    {
        try
        {
            var result = await _achievementService.GetAllDeletedAchievementsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Adds a new user achievement based on the provided request data.
    /// </summary>
    /// <param name="request">
    /// An instance of <see cref="UserAchievementCreateRequest"/> containing the details required to create a user achievement.
    /// </param>
    /// <returns>
    /// An <see cref="ActionResult"/> indicating the outcome of the operation.
    /// If successful, returns a status of 200 (OK) with the ID of the newly added user achievement.
    /// </returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("user-achievements")]
    public async Task<ActionResult<UserAchievementDto>> AddUserAchievement([FromBody] UserAchievementCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _achievementService.AddUserAchievementAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a specific user achievement by its unique identifier.
    /// </summary>
    /// <param name="userAchievementId">
    /// The unique identifier of the user achievement to retrieve.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the user achievement details if found.
    /// If the identifier is invalid or an error occurs, returns an appropriate error response.
    /// </returns>
    [HttpGet("user-achievements/{userAchievementId}")]
    public async Task<ActionResult<UserAchievementDto>> GetUserAchievementById([FromRoute] Guid userAchievementId)
    {
        if (userAchievementId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid user achievement ID." });
        }

        try
        {
            var result = await _achievementService.GetUserAchievementByIdAsync(userAchievementId);
            if (result == null)
            {
                return NotFound(new { error = "User achievement not found." });
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Updates the information of an existing user achievement based on the provided update request data.
    /// </summary>
    /// <param name="request">
    /// An instance of <see cref="UserAchievementUpdateRequest"/> containing the updated details of the user achievement.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the outcome of the operation.
    /// If successful, returns a status of 200 (OK) with a success message.
    /// If an error occurs, returns a status of 400 (Bad Request) with the corresponding error message.
    /// </returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("user-achievements")]
    public async Task<IActionResult> UpdateUserAchievement([FromBody] UserAchievementUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _achievementService.UpdateUserAchievement(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a user achievement identified by the provided ID.
    /// </summary>
    /// <param name="userAchievementId">
    /// The unique identifier of the user achievement to be deleted.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the outcome of the operation.
    /// If successful, returns a status of 200 (OK) with a confirmation message.
    /// If the ID is invalid or an exception occurs, returns a status of 400 (Bad Request) with an error message.
    /// </returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("user-achievements/{userAchievementId}")]
    public async Task<IActionResult> DeleteUserAchievement([FromRoute] Guid userAchievementId)
    {
        if (userAchievementId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid user achievement ID." });
        }

        try
        {
            await _achievementService.DeleteUserAchievementAsync(userAchievementId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves the list of all user achievements.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a list of all user achievements.
    /// If successful, returns a status of 200 (OK) with the list of achievements.
    /// In case of an error, returns a status of 400 (Bad Request) with the error message.
    /// </returns>
    [HttpGet("user-achievements/all")]
    public async Task<ActionResult<List<UserAchievementDto>>> GetAllUserAchievements()
    {
        try
        {
            var result = await _achievementService.GetAllUserAchievementsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all user achievements associated with the specified Keycloak user ID.
    /// </summary>
    /// <param name="keycloakId">
    /// The unique identifier corresponding to the user's Keycloak account.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the list of user achievements associated with the specified Keycloak user ID.
    /// Returns a status of 200 (OK) with the achievements if successful.
    /// Returns a 400 (Bad Request) if the Keycloak ID is invalid or if an error occurs.
    /// </returns>
    [HttpGet("{keycloakId}/user-achievements")]
    public async Task<ActionResult<List<UserAchievementDto>>> GetAllUserAchievementsByUserKeycloakId([FromRoute] string keycloakId)
    {
        if (string.IsNullOrEmpty(keycloakId))
        {
            return BadRequest(new { error = "Invalid keycloak ID." });
        }

        try
        {
            var result = await _achievementService.GetAllUserAchievementsByUserKeycloakIdAsync(keycloakId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all user achievements associated with a specific game using the provided game ID.
    /// </summary>
    /// <param name="gameId">
    /// The unique identifier of the game for which user achievements are being retrieved.
    /// </param>
    /// <returns>
    /// An <see cref="ActionResult"/> containing a list of user achievements for the specified game.
    /// Returns a status of 200 (OK) with the retrieved achievements if successful.
    /// If the provided game ID is invalid or an exception occurs, returns a status of 400 (Bad Request) with an error message.
    /// </returns>
    [HttpGet("games/{gameId}/user-achievements")]
    public async Task<ActionResult<List<UserAchievementDto>>> GetAllUserAchievementsByGameId([FromRoute] Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid game ID." });
        }

        try
        {
            var result = await _achievementService.GetAllUserAchievementsByGameIdAsync(gameId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}