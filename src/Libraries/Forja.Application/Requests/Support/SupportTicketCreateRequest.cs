namespace Forja.Application.Requests.Support;

public class SupportTicketCreateRequest
{
    public Guid UserId { get; set; }
    [Required]
    [MaxLength(50)]
    public string Subject { get; set; } = string.Empty;
    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = String.Empty;
}