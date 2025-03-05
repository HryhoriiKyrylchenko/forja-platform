namespace Forja.API.Controllers.UserProfile;

/// <summary>
/// Controller for managing UserWishList-related functionality.
/// Exposes UserWishList operations through API endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserWishListController : ControllerBase
{
    private readonly IUserWishListService _userWishListService;

    public UserWishListController(IUserWishListService userWishListService)
    {
        _userWishListService = userWishListService;
    }

    /// <summary>
    /// Retrieves all UserWishList entries.
    /// </summary>
    /// <returns>A list of UserWishListDTO.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserWishListDto>>> GetAll()
    {
        var wishLists = await _userWishListService.GetAllAsync();
        return Ok(wishLists);
    }

    /// <summary>
    /// Retrieves a UserWishList entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the UserWishList entry.</param>
    /// <returns>A UserWishListDTO if found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserWishListDto>> GetById(Guid id)
    {
        var wishList = await _userWishListService.GetByIdAsync(id);
        if (wishList == null)
            return NotFound($"UserWishList entry with ID {id} not found.");

        return Ok(wishList);
    }

    /// <summary>
    /// Adds a new product to the user's wishlist.
    /// </summary>
    /// <param name="request">The UserWishListCreateRequest containing user and product IDs.</param>
    /// <returns>The created UserWishListDTO.</returns>
    [HttpPost]
    public async Task<ActionResult<UserWishListDto>> Add([FromBody] UserWishListCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdWishList = await _userWishListService.AddAsync(request.UserId, request.ProductId);
        return CreatedAtAction(nameof(GetById), new { id = createdWishList.Id }, createdWishList);
    }

    /// <summary>
    /// Updates an existing UserWishList entry.
    /// </summary>
    /// <param name="id">The unique identifier of the entry to update.</param>
    /// <param name="request">The UserWishListUpdateRequest containing updated user and product IDs.</param>
    /// <returns>No content if update is successful.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserWishListUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _userWishListService.UpdateAsync(id, request.UserId, request.ProductId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a UserWishList entry by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entry to delete.</param>
    /// <returns>No content if deletion is successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userWishListService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Retrieves all UserWishList entries for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of UserWishListDTO entries associated with the user.</returns>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<UserWishListDto>>> GetByUserId(Guid userId)
    {
        var wishLists = await _userWishListService.GetByUserIdAsync(userId);
        return Ok(wishLists);
    }
}