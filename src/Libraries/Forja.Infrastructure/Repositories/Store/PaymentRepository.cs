namespace Forja.Infrastructure.Repositories.Store;

public class PaymentRepository : IPaymentRepository
{
    private readonly ForjaDbContext _dbContext;

    public PaymentRepository(ForjaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId)
    {
        return await _dbContext.Payments
            .FirstOrDefaultAsync(p => p.ExternalPaymentId == transactionId);
    }

    public async Task UpdatePaymentAsync(Payment payment)
    {
        _dbContext.Payments.Update(payment);
        await _dbContext.SaveChangesAsync();
    }
}
