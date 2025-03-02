namespace Forja.Domain.Repositories.Store;

public interface IPaymentRepository
{
    Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId);
    Task UpdatePaymentAsync(Payment payment);

}