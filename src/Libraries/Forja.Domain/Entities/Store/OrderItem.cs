namespace Forja.Domain.Entities.Store;

/// <summary>
/// Represents an item in an order.
/// </summary>
[Table("OrderItems", Schema = "store")]
public class OrderItem : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the order item.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the order.
    /// </summary>
    [ForeignKey("Order")]
    public Guid OrderId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the final price of the order item.
    /// </summary>
    public decimal FinalPrice { get; set; }
    
    /// <summary>
    /// Gets or sets the order associated with the order item.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Order Order { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the product associated with the order item.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Product Product { get; set; } = null!;
}