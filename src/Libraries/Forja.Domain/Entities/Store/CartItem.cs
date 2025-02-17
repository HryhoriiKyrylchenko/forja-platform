namespace Forja.Domain.Entities.Store;

/// <summary>
/// Represents an item in a shopping cart.
/// </summary>
[Table("CartItems", Schema = "store")]
public class CartItem : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the cart item.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the cart.
    /// </summary>
    [ForeignKey("Cart")]
    public Guid CartId { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the price of the cart item.
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Gets or sets the cart associated with the cart item.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Cart Cart { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product associated with the cart item.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual Product Product { get; set; } = null!;
}