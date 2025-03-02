namespace Forja.Domain.Enums;

/// <summary>
/// Represents the status of a payment.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// The payment is pending and has not been completed yet.
    /// </summary>
    Pending,

    /// <summary>
    /// The payment has been completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// The payment has failed.
    /// </summary>
    Failed,
    
    /// <summary>
    /// Indicates that the payment has been refunded to the payer.
    /// </summary>
    Refunded
}