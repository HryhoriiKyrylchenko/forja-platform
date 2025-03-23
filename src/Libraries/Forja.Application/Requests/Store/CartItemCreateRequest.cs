namespace Forja.Application.Requests.Store;

public class CartItemCreateRequest
{
    [Required]
    public Guid CartId { get; set; }
    [Required]
    public Guid ProductId { get; set; }
}