namespace Forja.Domain.Entities.Store;

/// <summary>
/// Represents a shopping cart in the store.
/// </summary>
[Table("Carts", Schema = "store")]
public class Cart
{
    /// <summary>
    /// Gets or sets the unique identifier for the cart.
    /// </summary>
    [Key]
    public Guid Id { get; set; } 
    
    /// <summary>
    /// Gets or sets the unique identifier for the user associated with the cart.
    /// </summary>
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the total amount of the cart.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the status of the cart, indicating its current state such as Active, Archived, or Abandoned.
    /// </summary>
    public CartStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the cart was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the cart was last modified.
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the user associated with the cart.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual User User { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the collection of cart items in the cart.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<CartItem> CartItems { get; set; } = [];
}