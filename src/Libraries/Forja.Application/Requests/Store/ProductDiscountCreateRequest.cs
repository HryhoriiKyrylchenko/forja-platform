namespace Forja.Application.Requests.Store;

public class ProductDiscountCreateRequest
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public Guid DiscountId { get; set; }
}