namespace Forja.Domain.Entities.Store;

/// <summary>
/// Represents an order in the store.
/// </summary>
[Table("Orders", Schema = "store")]
public class Order : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the order.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the cart placed the order.
    /// </summary>
    [ForeignKey("Cart")]
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the order was placed.
    /// </summary>
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the payment status of the order.
    /// </summary>
    [Required]
    public OrderPaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Gets or sets the shopping cart associated with the order.
    /// </summary>
    public virtual Cart Cart { get; set; } = null!;

    /// <summary>
    /// Gets or sets the payment details for the order.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Payment? Payment { get; set; }
}