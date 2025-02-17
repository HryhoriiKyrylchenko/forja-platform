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
    /// Gets or sets the unique identifier for the user who placed the order.
    /// </summary>
    [ForeignKey("User")]
    public Guid UserId { get; set; }

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
    /// Gets or sets the total amount for the order.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the payment details for the order.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Payment? Payment { get; set; }

    /// <summary>
    /// Gets or sets the collection of order items.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
}