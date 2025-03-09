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
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the product.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a brief description of the product.
    /// </summary>
    public string ShortDescription { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the developer of the product.
    /// </summary>
    public string Developer { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the minimal age category suggested for players of this product.
    /// </summary>
    [Required]
    public MinimalAge MinimalAge { get; set; }

    /// <summary>
    /// Gets or sets the platform on which the product is available.
    /// </summary>
    public string Platforms { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the URL of the logo associated with the product.
    /// </summary>
    public string LogoUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the date and time when the product was last modified.
    /// </summary>
    public DateTime? ModifiedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the release date of the product.
    /// </summary>
    [Required]
    public DateTime ReleaseDate { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the product is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the interface languages available for the product.
    /// </summary>
    public string InterfaceLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the available audio languages for the product.
    /// </summary>
    public string AudioLanguages { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the languages available for subtitles in the product.
    /// </summary>
    public string SubtitlesLanguages { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the collection of product genres associated with the game.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<ProductGenres> ProductGenres { get; set; } = [];
    
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
    
    /// <summary>
    /// Gets or sets the collection of user white lists associated with the product.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<UserWishList> UserWishLists { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of mature content associated with the product.
    /// </summary>
    public virtual ICollection<ProductMatureContent> ProductMatureContents { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of images associated with the product.
    /// </summary>
    public virtual ICollection<ProductImages> ProductImages { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the collection of reviews associated with the product.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<Review> Reviews { get; set; } = [];
}