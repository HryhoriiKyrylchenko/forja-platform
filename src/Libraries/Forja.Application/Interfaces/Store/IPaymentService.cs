namespace Forja.Application.Interfaces.Store;

/// <summary>
/// Provides methods for payment-related operations within the system.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Retrieves a payment record by its unique identifier.
    /// </summary>
    /// <param name="paymentId">The unique identifier of the payment.</param>
    /// <returns>A <see cref="PaymentDto"/> object containing the details of the payment if found; otherwise, null.</returns>
    Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId);

    /// <summary>
    /// Retrieves the payment details based on the provided transaction ID.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction for which the payment details are to be retrieved.</param>
    /// <returns>An instance of <see cref="PaymentDto"/> containing the payment details if found; otherwise, null.</returns>
    Task<PaymentDto?> GetPaymentByTransactionIdAsync(string transactionId);

    /// <summary>
    /// Retrieves a collection of payments associated with a specific order ID.
    /// </summary>
    /// <param name="orderId">
    /// The unique identifier of the order for which payments are to be retrieved.
    /// This value must not be an empty GUID.
    /// </param>
    /// <returns>
    /// An enumerable collection of <see cref="PaymentDto"/> representing the payments associated
    /// with the specified order ID. If no payments are found, an empty collection is returned.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided <paramref name="orderId"/> is an empty GUID.
    /// </exception>
    Task<IEnumerable<PaymentDto>> GetPaymentsByOrderIdAsync(Guid orderId);

    /// <summary>
    /// Updates the status of a payment based on the provided request.
    /// </summary>
    /// <param name="satusRequest">The request object containing payment details and the identifier of the payment to update.</param>
    /// <returns>A <see cref="PaymentDto"/> object representing the updated payment if the operation succeeds, or null if the payment could not be found.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided request is invalid.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a payment with the provided ID is not found.</exception>
    Task<PaymentDto?> UpdatePaymentStatusAsync(PaymentUpdateSatusRequest satusRequest);

    /// <summary>
    /// Deletes a payment record asynchronously based on the provided payment ID.
    /// </summary>
    /// <param name="paymentId">The unique identifier of the payment to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeletePaymentAsync(Guid paymentId);

    /// <summary>
    /// Executes a payment process using the provided payment request details.
    /// This method validates the payment request, processes the payment with the specified
    /// payment method, and records the payment details in the system.
    /// </summary>
    /// <param name="request">The <see cref="PaymentRequest"/> object containing payment details such as order ID, amount, currency, payment provider, and payment token.</param>
    /// <returns>A <see cref="string"/> representing the transaction ID of the executed payment.</returns>
    /// <exception cref="ArgumentException">Thrown when the payment method in the request is invalid.</exception>
    /// <exception cref="Exception">Thrown when there is an error during the payment process or data persistence.</exception>
    Task<string> ExecutePaymentAsync(PaymentRequest request);

    /// <summary>
    /// Initiates a refund for a payment with the specified payment ID.
    /// </summary>
    /// <param name="paymentId">The unique identifier of the payment to be refunded.</param>
    /// <returns>
    /// A boolean indicating whether the refund operation was successful.
    /// </returns>
    Task<bool> RefundPaymentAsync(Guid paymentId);
}