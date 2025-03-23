namespace Forja.Domain.Enums;

/// <summary>
/// Represents the payment status of an order.
/// </summary>
public enum OrderStatus
{
    Pending,

    Completed,
    
    Canceled,

    Failed,
}