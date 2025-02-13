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
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    public DateTime PaymentDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the status of the payment.
    /// </summary>
    [Required]
    public PaymentStatus PaymentStatus { get; set; }
    
    /// <summary>
    /// Gets or sets the associated order.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Order Order { get; set; } = null!;
}