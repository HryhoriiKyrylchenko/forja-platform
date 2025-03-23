namespace Forja.Application.DTOs.Store;

public class ProductDiscountDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid DiscountId { get; set; }
}