namespace Forja.Application.Interfaces.Store;

public interface IPaymentService
{
    Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId);
    Task<PaymentDto?> GetPaymentByTransactionIdAsync(string transactionId);
    Task<IEnumerable<PaymentDto>> GetPaymentsByOrderIdAsync(Guid orderId);
    Task<PaymentDto?> UpdatePaymentStatusAsync(PaymentUpdateSatusRequest satusRequest);
    Task DeletePaymentAsync(Guid paymentId);

    Task<string> ExecutePaymentAsync(PaymentRequest request);
    Task<bool> RefundPaymentAsync(Guid paymentId);
}