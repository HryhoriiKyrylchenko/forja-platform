namespace Forja.Application.DTOs.Store;

public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? ExternalPaymentId { get; set; }
    public string? ProviderName { get; set; }
    public string? ProviderResponse { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public bool IsRefunded { get; set; }
}