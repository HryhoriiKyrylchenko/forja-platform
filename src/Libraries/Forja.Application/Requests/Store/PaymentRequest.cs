namespace Forja.Application.Requests.Store;

public class PaymentRequest
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
    public PaymentMethod ProviderName { get; set; } = PaymentMethod.Custom;
    public string PaymentMethodToken { get; set; } = string.Empty;
    public PaymentMethod PaymentMethod { get; set; }
}