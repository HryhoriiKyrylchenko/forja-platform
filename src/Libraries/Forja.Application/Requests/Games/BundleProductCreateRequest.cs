namespace Forja.Application.Requests.Games;

public class BundleProductsCreateRequest
{
    [Required]
    public Guid BundleId { get; set; }
    [Required] 
    public required List<Guid> ProductIds { get; set; }
    [Required]
    public decimal BundleTotalPrice { get; set; }
}