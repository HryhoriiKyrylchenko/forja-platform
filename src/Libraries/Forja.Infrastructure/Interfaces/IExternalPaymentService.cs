namespace Forja.Infrastructure.Interfaces;

/// <summary>
/// Defines the contract for an external payment service used to process, refund,
/// and retrieve the status of financial transactions.
/// </summary>
public interface IExternalPaymentService
{
    /// <summary>
    /// Processes a payment asynchronously using the specified amount, currency, and payment method token.
    /// </summary>
    /// <param name="amount">The monetary amount to be processed.</param>
    /// <param name="currency">The currency in which the payment is made (e.g., USD, EUR).</param>
    /// <param name="paymentMethodToken">A token identifying the payment method to be used (e.g., a credit card or another payment method identifier).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a unique identifier for the processed payment transaction.</returns>
    Task<string> ProcessPaymentAsync(decimal amount, string currency, string paymentMethodToken);

    /// <summary>
    /// Initiates a refund request for a specified transaction.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction to be refunded.</param>
    /// <param name="amount">The amount to be refunded.</param>
    /// <returns>A task representing the asynchronous operation. The task result is true if the refund is successfully processed, otherwise false.</returns>
    Task<bool> RefundPaymentAsync(string transactionId, decimal amount);

    /// <summary>
    /// Retrieves the current status of a payment based on the provided transaction identifier.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction whose status is being queried.</param>
    /// <returns>A <see cref="PaymentStatus"/> indicating the current status of the payment, such as Pending, Completed, Failed, or Refunded.</returns>
    Task<PaymentStatus> GetPaymentStatusAsync(string transactionId);
}