namespace Forja.API.Controllers.UserProfile;

/// <summary>
/// Controller for managing UserFollower functionality.
/// Exposes endpoints for handling UserFollower-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserFollowerController : ControllerBase
{
    private readonly IUserFollowerService _userFollowerService;

    public UserFollowerController(IUserFollowerService userFollowerService)
    {
        _userFollowerService = userFollowerService;
    }

    /// <summary>
    /// Retrieves a list of all UserFollower entries.
    /// </summary>
    /// <returns>A list of UserFollowerDTO objects representing all entries.</returns>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet]
    public async Task<ActionResult<List<UserFollowerDto>>> GetAll()
    {
        try
        {
            var followers = await _userFollowerService.GetAllAsync();
            return Ok(followers);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a specific UserFollower entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the UserFollower entry to retrieve.</param>
    /// <returns>An instance of UserFollowerDto representing the UserFollower entry, or a NotFound result if the entry does not exist.</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserFollowerDto>> GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var follower = await _userFollowerService.GetByIdAsync(id);

            if (follower == null)
            {
                return NotFound(new { error = $"Follower with ID {id} not found." });
            }
            return Ok(follower);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Adds a new UserFollower entry.
    /// </summary>
    /// <param name="request">The request object containing details of the UserFollower to be added, including FollowerId and FollowedId.</param>
    /// <returns>The unique identifier (GUID) of the newly created UserFollower entry.</returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UserFollowerDto>> Add([FromBody] UserFollowerCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _userFollowerService.AddAsync(request);
            if (result == null)
            {
                return BadRequest(new { error = "Failed to create follower." });
            }
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);

        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing UserFollower relationship.
    /// </summary>
    /// <param name="id">The unique identifier of the UserFollower entry to update.</param>
    /// <param name="request">The UserFollowerUpdateRequest containing the updated IDs.</param>
    /// <returns>No content if update is successful.</returns>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserFollowerUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _userFollowerService.UpdateAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a UserFollower relationship by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the UserFollower entry to delete.</param>
    /// <returns>No content if deletion is successful.</returns>
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
            await _userFollowerService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all UserFollower entries where the specified user is the follower.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who is the follower.</param>
    /// <returns>A list of UserFollowerDTO entries where the user is the follower.</returns>
    [Authorize]
    [HttpGet("followers/{userId}")]
    public async Task<ActionResult<List<UserFollowerDto>>> GetFollowersByUserId(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var followers = await _userFollowerService.GetFollowersByUserIdAsync(userId); // підписані на мене
            return Ok(followers);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all UserFollower entries where the specified user is being followed.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who is being followed.</param>
    /// <returns>A list of UserFollowerDTO entries where the user is being followed.</returns>
    [Authorize]
    [HttpGet("followed/{userId}")]
    public async Task<ActionResult<List<UserFollowerDto>>> GetFollowedByUserId(Guid userId) // на кого я підписан
    {
        if (userId == Guid.Empty)
        {
            return BadRequest(new { error = "Invalid ID." });
        }

        try
        {
            var followedUsers = await _userFollowerService.GetFollowedByUserIdAsync(userId);
            return Ok(followedUsers);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}