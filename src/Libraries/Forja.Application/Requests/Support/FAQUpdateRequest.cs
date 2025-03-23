namespace Forja.Application.Requests.Support;

public class FAQUpdateRequest
{
    public Guid Id { get; set; }
    [Required]
    public string Question { get; set; } = String.Empty;
    [Required]
    public string Answer { get; set; } = String.Empty;
    public int Order { get; set; }
}