namespace Forja.Infrastructure.Repositories.Store;

/// <summary>
/// Implementation of the Payment repository interface for managing Payment data.
/// </summary>
public class PaymentRepository : IPaymentRepository
{
    private readonly ForjaDbContext _dbContext;
    private readonly DbSet<Payment> _payments;

    /// <summary>
    /// Initializes a new instance of the PaymentRepository class.
    /// </summary>
    /// <param name="dbContext">The database context to use for operations.</param>
    public PaymentRepository(ForjaDbContext dbContext)
    {
        _dbContext = dbContext;
        _payments = dbContext.Set<Payment>();
    }

    /// <summary>
    /// Retrieves a Payment by its transaction ID.
    /// </summary>
    /// <param name="transactionId">The external transaction ID of the Payment.</param>
    /// <returns>The matching Payment object or null if not found.</returns>
    public async Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
        {
            throw new ArgumentException("Transaction ID cannot be null or empty.", nameof(transactionId));
        }

        return await _payments
            .Where(p => !p.IsDeleted)
            .FirstOrDefaultAsync(p => p.ExternalPaymentId == transactionId);
    }

    /// <summary>
    /// Retrieves a Payment by its unique identifier.
    /// </summary>
    /// <param name="paymentId">The unique identifier of the Payment.</param>
    /// <returns>The matching Payment object or null if not found.</returns>
    public async Task<Payment?> GetPaymentByIdAsync(Guid paymentId)
    {
        if (paymentId == Guid.Empty)
        {
            throw new ArgumentException("Invalid payment ID.", nameof(paymentId));
        }

        return await _payments
            .Where(p => !p.IsDeleted)
            .FirstOrDefaultAsync(p => p.Id == paymentId);
    }

    /// <summary>
    /// Retrieves all Payments associated with a specific Order.
    /// </summary>
    /// <param name="orderId">The unique identifier of the Order.</param>
    /// <returns>A list of Payments related to the specified Order.</returns>
    public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Invalid order ID.", nameof(orderId));
        }

        return await _payments
            .Where(p => !p.IsDeleted)
            .Where(p => p.OrderId == orderId)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new Payment to the database.
    /// </summary>
    /// <param name="payment">The Payment object to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddPaymentAsync(Payment payment)
    {
        if (!StoreModelValidator.ValidatePaymentModel(payment, out string errors))
        {
            throw new ArgumentException(errors);
        }

        await _payments.AddAsync(payment);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing Payment in the database.
    /// </summary>
    /// <param name="payment">The Payment object to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdatePaymentAsync(Payment payment)
    {
        if (!StoreModelValidator.ValidatePaymentModel(payment, out string errors))
        {
            throw new ArgumentException(errors);
        }

        _payments.Update(payment);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a Payment from the database by its unique identifier.
    /// </summary>
    /// <param name="paymentId">The unique identifier of the Payment to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeletePaymentAsync(Guid paymentId)
    {
        if (paymentId == Guid.Empty)
        {
            throw new ArgumentException("Invalid payment ID.", nameof(paymentId));
        }

        var payment = await _payments.FindAsync(paymentId);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
        }

        _payments.Remove(payment);
        await _dbContext.SaveChangesAsync();
    }
}
