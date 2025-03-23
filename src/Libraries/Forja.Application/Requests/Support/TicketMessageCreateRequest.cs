namespace Forja.Application.Requests.Support;

public class TicketMessageCreateRequest
{
    public Guid SupportTicketId { get; set; }
    public Guid SenderId { get; set; }
    [Required]
    [MaxLength(2000)]
    public string Message { get; set; } = String.Empty;
}