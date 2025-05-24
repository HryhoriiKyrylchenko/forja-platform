namespace Forja.Application.Requests.Games;

public class BundleCreateRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public DateTime? ExpirationDate { get; set; }
}