namespace Forja.Application.Requests.Store;

public class CartAddBundleRequest
{
    [Required]
    public Guid CartId { get; set; }
    [Required]
    public Guid BundleId { get; set; }
}