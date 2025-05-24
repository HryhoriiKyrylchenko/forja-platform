namespace Forja.Application.Requests.Store;

public class ProductDiscountUpdateRequest
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public Guid DiscountId { get; set; }
}