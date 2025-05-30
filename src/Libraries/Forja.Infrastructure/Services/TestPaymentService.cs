using Forja.Domain.Enums;

namespace Forja.Infrastructure.Services;

public class TestPaymentService : ITestPaymentService
{
    ///<inheritdoc/>
    public async Task<string> ProcessPaymentAsync(decimal amount, string currency, string paymentMethodToken)
    {
        await Task.Delay(1000);
        return Guid.NewGuid().ToString();
    }

    ///<inheritdoc/>
    public async Task<bool> RefundPaymentAsync(string transactionId, decimal amount)
    {
        await Task.Delay(1000);
        return true;
    }

    ///<inheritdoc/>
    public async Task<PaymentStatus> GetPaymentStatusAsync(string transactionId)
    {
        await Task.Delay(1000);
        return PaymentStatus.Completed;
    }
}