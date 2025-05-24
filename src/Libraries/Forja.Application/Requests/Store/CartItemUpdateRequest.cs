namespace Forja.Application.Requests.Store;

public class CartItemUpdateRequest
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid CartId { get; set; }
    [Required]
    public Guid ProductId { get; set; }
    public Guid? BundleId { get; set; }
}