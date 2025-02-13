namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a product in the games domain.
/// </summary>
[Table("Products", Schema = "games")]
public abstract class Product : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the product.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Gets or sets the date and time when the product was last modified.
    /// </summary>
    public DateTime? ModifiedAt { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the product is active.
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of discounts associated with the product.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<Discount> Discounts { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of cart items associated with the product.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<CartItem> CartItems { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of order items associated with the product.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];    
    
    /// <summary>
    /// Gets or sets the collection of product discounts associated with the product.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = [];
}