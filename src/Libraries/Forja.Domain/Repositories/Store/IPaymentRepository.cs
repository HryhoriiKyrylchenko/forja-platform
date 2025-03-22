namespace Forja.Domain.Repositories.Store;

/// <summary>
/// Interface for the Payment repository, defining methods for Payment CRUD operations.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Retrieves a Payment by its transaction ID.
    /// </summary>
    /// <param name="transactionId">The external transaction ID of the Payment.</param>
    /// <returns>The matching Payment object or null if not found.</returns>
    Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId);

    /// <summary>
    /// Retrieves a Payment by its unique identifier.
    /// </summary>
    /// <param name="paymentId">The unique identifier of the Payment.</param>
    /// <returns>The matching Payment object or null if not found.</returns>
    Task<Payment?> GetPaymentByIdAsync(Guid paymentId);

    /// <summary>
    /// Retrieves all Payments associated with a specific Order.
    /// </summary>
    /// <param name="orderId">The unique identifier of the Order.</param>
    /// <returns>A list of Payments related to the specified Order.</returns>
    Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(Guid orderId);

    /// <summary>
    /// Adds a new Payment to the database.
    /// </summary>
    /// <param name="payment">The Payment object to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddPaymentAsync(Payment payment);

    /// <summary>
    /// Updates an existing Payment in the database.
    /// </summary>
    /// <param name="payment">The Payment object to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdatePaymentAsync(Payment payment);

    /// <summary>
    /// Deletes a Payment from the database by its unique identifier.
    /// </summary>
    /// <param name="paymentId">The unique identifier of the Payment to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeletePaymentAsync(Guid paymentId);
}