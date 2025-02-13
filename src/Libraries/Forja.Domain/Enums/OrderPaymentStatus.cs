namespace Forja.Domain.Enums;

/// <summary>
/// Represents the payment status of an order.
/// </summary>
public enum OrderPaymentStatus
{
    /// <summary>
    /// The payment is pending.
    /// </summary>
    Pending,

    /// <summary>
    /// The payment is completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The payment has failed.
    /// </summary>
    Failed,
}