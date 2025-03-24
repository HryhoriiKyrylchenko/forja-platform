namespace Forja.API.Controllers.Support;

/// <summary>
/// Controller for managing support tickets.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SupportTicketController : ControllerBase
{
    private readonly ISupportTicketService _ticketService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SupportTicketController"/> class.
    /// </summary>
    /// <param name="ticketService">The service for managing support tickets.</param>
    public SupportTicketController(ISupportTicketService ticketService)
    {
        _ticketService = ticketService;
    }

    /// <summary>
    /// Retrieves all support tickets.
    /// </summary>
    /// <returns>A list of support ticket DTOs.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllTickets()
    {
        try
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a specific support ticket by its ID.
    /// </summary>
    /// <param name="id">The ID of the support ticket to retrieve.</param>
    /// <returns>The support ticket DTO.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTicketById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "Ticket ID is required." });

        try
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null) return NotFound(new { error = $"No ticket found with ID {id}." });
            return Ok(ticket);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves support tickets by user ID.
    /// </summary>
    /// <param name="userId">The user ID to filter tickets.</param>
    /// <returns>A list of support ticket DTOs for the specified user.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetTicketsByUserId([FromRoute] Guid userId)
    {
        if (userId == Guid.Empty) return BadRequest(new { error = "User ID is required." });

        try
        {
            var tickets = await _ticketService.GetByUserIdAsync(userId);
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new support ticket.
    /// </summary>
    /// <param name="request">The request containing information to create the support ticket.</param>
    /// <returns>The created support ticket DTO.</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateSupportTicket([FromBody] SupportTicketCreateRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var ticket = await _ticketService.CreateAsync(request);
            return CreatedAtAction(nameof(GetTicketById), new { id = ticket?.Id }, ticket);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Updates the status of an existing support ticket.
    /// </summary>
    /// <param name="request">The request containing the ticket ID and new status.</param>
    /// <returns>The updated support ticket DTO.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTicketStatus([FromBody] SupportTicketUpdateStatusRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updatedTicket = await _ticketService.UpdateStatusAsync(request);
            if (updatedTicket == null) return NotFound(new { error = "Ticket not found." });
            return Ok(updatedTicket);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes an existing support ticket.
    /// </summary>
    /// <param name="id">The ID of the support ticket to delete.</param>
    /// <returns>No content if the ticket is successfully deleted.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTicket([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest(new { error = "Ticket ID is required." });

        try
        {
            await _ticketService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}