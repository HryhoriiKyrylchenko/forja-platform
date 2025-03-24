namespace Forja.Application.Requests.Support;

public class SupportTicketUpdateStatusRequest
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public SupportTicketStatus Status { get; set; }
}