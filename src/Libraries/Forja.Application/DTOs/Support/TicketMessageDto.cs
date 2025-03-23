namespace Forja.Application.DTOs.Support;

public class TicketMessageDto
{
    public Guid Id { get; set; }
    public Guid SupportTicketId { get; set; }
    public Guid SenderId { get; set; }
    public bool IsSupportAgent { get; set; } = false;
    public string Message { get; set; } = String.Empty;
    public DateTime SentAt { get; set; }
}