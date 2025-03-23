namespace Forja.Application.Requests.Store;

public class DiscountCreateRequest
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public DiscountType DiscountType { get; set; }
    [Range(0, double.MaxValue)]
    public decimal DiscountValue { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}