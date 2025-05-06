namespace Forja.Application.DTOs.Common;

public class ProdShortDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public DiscountDto? ActiveDiscount { get; set; } 
}