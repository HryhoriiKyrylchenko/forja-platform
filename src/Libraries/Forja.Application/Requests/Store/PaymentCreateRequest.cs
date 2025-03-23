namespace Forja.Application.Requests.Store;

public class PaymentCreateRequest
{
    [Required]
    public Guid OrderId { get; set; }
    [Required]
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string? ExternalPaymentId { get; set; }
    public string? ProviderName { get; set; }
    public string? ProviderResponse { get; set; }
    [Required]
    public PaymentStatus PaymentStatus { get; set; }
}