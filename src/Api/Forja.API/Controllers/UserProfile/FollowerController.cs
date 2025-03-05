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
    /// Retrieves all UserFollower entries.
    /// </summary>
    /// <returns>A list of UserFollowerDTO containing all entries.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserFollowerDto>>> GetAll()
    {
        var followers = await _userFollowerService.GetAllAsync();
        return Ok(followers);
    }

    /// <summary>
    /// Retrieves a UserFollower entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the UserFollower entry.</param>
    /// <returns>The UserFollowerDTO corresponding to the given ID.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserFollowerDto>> GetById(Guid id)
    {
        var follower = await _userFollowerService.GetByIdAsync(id);
        if (follower == null)
            return NotFound($"UserFollower entry with ID {id} not found.");
            
        return Ok(follower);
    }

    /// <summary>
    /// Adds a new UserFollower relationship.
    /// </summary>
    /// <param name="request">The UserFollowerCreateRequest containing the follower and followed user IDs.</param>
    /// <returns>The created UserFollowerDTO.</returns>
    [HttpPost]
    public async Task<ActionResult<UserFollowerDto>> Add([FromBody] FollowerCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdFollower = await _userFollowerService.AddAsync(request.FollowerId, request.FollowedId);
        return CreatedAtAction(nameof(GetById), new { id = createdFollower.Id }, createdFollower);
    }

    /// <summary>
    /// Updates an existing UserFollower relationship.
    /// </summary>
    /// <param name="id">The unique identifier of the UserFollower entry to update.</param>
    /// <param name="request">The UserFollowerUpdateRequest containing the updated IDs.</param>
    /// <returns>No content if update is successful.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] FollowerUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _userFollowerService.UpdateAsync(id, request.FollowerId, request.FollowedId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a UserFollower relationship by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the UserFollower entry to delete.</param>
    /// <returns>No content if deletion is successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userFollowerService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Retrieves all UserFollower entries where the specified user is the follower.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who is the follower.</param>
    /// <returns>A list of UserFollowerDTO entries where the user is the follower.</returns>
    [HttpGet("followers/{userId}")]
    public async Task<ActionResult<IEnumerable<UserFollowerDto>>> GetFollowersByUserId(Guid userId)
    {
        var followers = await _userFollowerService.GetFollowersByUserIdAsync(userId);
        return Ok(followers);
    }

    /// <summary>
    /// Retrieves all UserFollower entries where the specified user is being followed.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who is being followed.</param>
    /// <returns>A list of UserFollowerDTO entries where the user is being followed.</returns>
    [HttpGet("followed/{userId}")]
    public async Task<ActionResult<IEnumerable<UserFollowerDto>>> GetFollowedByUserId(Guid userId)
    {
        var followedUsers = await _userFollowerService.GetFollowedByUserIdAsync(userId);
        return Ok(followedUsers);
    }
}