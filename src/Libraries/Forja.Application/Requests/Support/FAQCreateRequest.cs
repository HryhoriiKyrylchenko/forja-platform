namespace Forja.Application.Requests.Support;

public class FAQCreateRequest
{
    [Required]
    public string Question { get; set; } = String.Empty;
    [Required]
    public string Answer { get; set; } = String.Empty;
    public int Order { get; set; }
}