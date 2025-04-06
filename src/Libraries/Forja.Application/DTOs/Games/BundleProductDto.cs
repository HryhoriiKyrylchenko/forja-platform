namespace Forja.Application.DTOs.Games;

public class BundleProductDto
{
    public Guid Id { get; set; }
    public Guid BundleId { get; set; }
    public Guid ProductId { get; set; }
    public decimal DistributedPrice { get; set; }
    public decimal OriginalProductPrice { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
}