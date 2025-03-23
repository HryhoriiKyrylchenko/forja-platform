namespace Forja.Domain.Entities.Store;

/// <summary>
/// Represents a payment entity in the store.
/// </summary>
[Table("Payments", Schema = "store")]
public class Payment : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the payment.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the associated order.
    /// </summary>
    [ForeignKey("Order")]
    public Guid OrderId { get; set; }
    
    /// <summary>
    /// Gets or sets the method of payment.
    /// </summary>
    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Gets or sets the amount of the payment.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the payment was made.
    /// </summary>
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the external identifier associated with the payment, typically provided by the payment provider.
    /// </summary>
    [Required]
    public string ExternalPaymentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the payment provider used for processing the payment.
    /// </summary>
    public PaymentMethod ProviderName { get; set; }

    /// <summary>
    /// Gets or sets the response returned by the payment provider.
    /// </summary>
    public PaymentStatus ProviderResponse { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the payment has been refunded.
    /// </summary>
    public bool IsRefunded { get; set; }
    
    /// <summary>
    /// Gets or sets the associated order.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Order Order { get; set; } = null!;
}