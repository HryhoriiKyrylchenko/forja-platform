

namespace Forja.Infrastructure.Interfaces;

public interface IExternalPaymentService
{
    Task<string> ProcessPaymentAsync(decimal amount, string currency, string paymentMethodToken);
    Task<bool> RefundPaymentAsync(string transactionId, decimal amount);
    Task<PaymentStatus> GetPaymentStatusAsync(string transactionId);
}