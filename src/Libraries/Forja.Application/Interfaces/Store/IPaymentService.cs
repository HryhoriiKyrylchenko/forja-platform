namespace Forja.Application.Interfaces.Store;

public interface IPaymentService
{
    Task<Payment?> GetPaymentByIdAsync(Guid paymentId);
    Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId);
    Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(Guid orderId);
    Task AddPaymentAsync(PaymentCreateRequest request);
    Task UpdatePaymentAsync(PaymentUpdateRequest request);
    Task DeletePaymentAsync(Guid paymentId);
}