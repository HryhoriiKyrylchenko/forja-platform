namespace Forja.Application.DTOs.Store;

public class CartItemDto
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? BundleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public decimal OriginalPrice { get; set; }
    public decimal? TotalDiscountValue { get; set; }
    public DateTime? DiscountExpirationDate { get; set; }
    public decimal TotalPrice { get; set; }
}