namespace Forja.Application.Requests.Support;

public class TicketMessageUpdateRequest
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(2000)]
    public string Message { get; set; } = String.Empty;
    public bool IsSupportAgent { get; set; }
}