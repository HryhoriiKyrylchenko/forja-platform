namespace Forja.API.Controllers.Support;

/// <summary>
/// Controller for managing ticket messages.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TicketMessageController : ControllerBase
{
    private readonly ITicketMessageService _messageService;
    private readonly IUserService _userService;
    private readonly IUserAuthService _userAuthService;
    private readonly ISupportTicketService _supportTicketService;

    public TicketMessageController(ITicketMessageService messageService,
        IUserService userService,
        IUserAuthService userAuthService,
        ISupportTicketService supportTicketService)
    {
        _messageService = messageService;
        _userService = userService;
        _userAuthService = userAuthService;
        _supportTicketService = supportTicketService;
    }

    /// <summary>
    /// Retrieves all ticket messages.
    /// </summary>
    /// <returns>A list of ticket message DTOs.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllMessages()
    {
        try
        {
            var messages = await _messageService.GetAllAsync();
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a specific ticket message by its ID.
    /// </summary>
    /// <param name="id">The ID of the ticket message to retrieve.</param>
    /// <returns>The ticket message DTO.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMessageById([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "Message ID is required." });

        try
        {
            var message = await _messageService.GetByIdAsync(id);
            if (message == null)
                return NotFound(new { error = $"No message found with ID {id}." });
            return Ok(message);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves all messages for a specific support ticket by its ID.
    /// </summary>
    /// <param name="supportTicketId">The ID of the support ticket.</param>
    /// <returns>A list of ticket message DTOs for the specified support ticket.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpGet("ticket/{supportTicketId:guid}")]
    public async Task<IActionResult> GetMessagesBySupportTicketId([FromRoute] Guid supportTicketId)
    {
        if (supportTicketId == Guid.Empty)
            return BadRequest(new { error = "Support Ticket ID is required." });

        try
        {
            var messages = await _messageService.GetBySupportTicketIdAsync(supportTicketId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves messages associated with a specific support ticket for the authenticated user.
    /// </summary>
    /// <param name="supportTicketId">The unique identifier of the support ticket.</param>
    /// <returns>A list of ticket message DTOs if the user is authorized and the ticket exists, or an appropriate error response.</returns>
    [Authorize]
    [HttpGet("ticket/{supportTicketId:guid}/self")]
    public async Task<IActionResult> GetSelfMessagesBySupportTicketId([FromRoute] Guid supportTicketId)
    {
        if (supportTicketId == Guid.Empty)
            return BadRequest(new { error = "Support Ticket ID is required." });

        try
        {
            var ticket = await _supportTicketService.GetByIdAsync(supportTicketId);
            if (ticket == null)
            {
                return NotFound(new { error = $"No ticket found with ID {supportTicketId}." });
            }
            
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var result = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            if (result == null)
            {
                return NotFound(new { error = $"User with Keycloak ID {keycloakUserId} not found." });
            }

            if (result.Id != ticket.UserId)
            {
                return Unauthorized(new { error = "User is not authorized to view this ticket." });
            }
            
            var messages = await _messageService.GetBySupportTicketIdAsync(supportTicketId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new ticket message.
    /// </summary>
    /// <param name="request">The request containing information to create the ticket message.</param>
    /// <returns>The created ticket message DTO.</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] TicketMessageCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdMessage = await _messageService.CreateAsync(request);
            
            if (createdMessage == null)
            {
                return BadRequest(new { error = "Message not created." });
            }
            
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            if (await _userAuthService.CheckUserRoleAsync(keycloakUserId, "Administrator")
                || await _userAuthService.CheckUserRoleAsync(keycloakUserId, "SystemAdministrator")
                || await _userAuthService.CheckUserRoleAsync(keycloakUserId, "Moderator"))
            {
                var updatedMessage = await _messageService.UpdateAsync(new TicketMessageUpdateRequest
                    {
                        Id = createdMessage.Id,
                        Message = createdMessage.Message,
                        IsSupportAgent = true
                    });

                if (updatedMessage == null)
                {
                    return BadRequest(new { error = "Message for support agent not updated." });
                }
                
                return CreatedAtAction(nameof(GetMessageById), new { id = updatedMessage.Id }, updatedMessage);
            }
            
            return CreatedAtAction(nameof(GetMessageById), new { id = createdMessage.Id }, createdMessage);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing ticket message.
    /// </summary>
    /// <param name="request">The request containing the updated message data.</param>
    /// <returns>The updated ticket message DTO.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateMessage([FromBody] TicketMessageUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedMessage = await _messageService.UpdateAsync(request);
            if (updatedMessage == null)
                return NotFound(new { error = "Message not found." });
            return Ok(updatedMessage);
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
    /// Deletes an existing ticket message by its ID.
    /// </summary>
    /// <param name="id">The ID of the ticket message to delete.</param>
    /// <returns>No content if the message is successfully deleted.</returns>
    [Authorize(Policy = "ModeratePolicy")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMessage([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "Message ID is required." });

        try
        {
            await _messageService.DeleteAsync(id);
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