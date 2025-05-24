namespace Forja.Application.DTOs.Store;

public class DiscountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DiscountType { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}