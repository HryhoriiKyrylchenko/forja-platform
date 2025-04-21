namespace Forja.Domain.Entities.Store;

/// <summary>
/// Represents a shopping cart entity in the store domain.
/// </summary>
/// <remarks>
/// A `Cart` can belong to a specific user and contain multiple cart items.
/// It holds information such as the total amount, status, creation, and modification timestamps.
/// </remarks>
[Table("Carts", Schema = "store")]
public class Cart
{
    /// <summary>
    /// Gets or sets the unique identifier of the cart.
    /// </summary>
    /// <remarks>
    /// This property serves as the primary key for the Cart entity, ensuring each cart has a unique identity.
    /// </remarks>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with the cart.
    /// </summary>
    /// <remarks>
    /// This property is used to establish a relationship between a cart
    /// and a specific user. It acts as a foreign key pointing to the user's entry
    /// in the application, enabling the linkage of user and cart data.
    /// </remarks>
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Represents the total monetary value of the cart, calculated based on the prices of all cart items.
    /// </summary>
    /// <remarks>
    /// The value is expressed as a decimal and should not be negative.
    /// It is updated whenever the cart's contents or pricing change, ensuring the amount accurately reflects the cart's state.
    /// </remarks>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Represents the current status of the cart.
    /// Determines if the cart is active, archived, or abandoned.
    /// </summary>
    /// <remarks>
    /// Possible values:
    /// - Active: The cart is currently in use and can be modified.
    /// - Archived: The cart has been finalized or closed and cannot be modified further.
    /// - Abandoned: The cart was not completed and has been marked as abandoned.
    /// </remarks>
    public CartStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the cart was created.
    /// </summary>
    /// <remarks>
    /// This property is automatically initialized with the current UTC timestamp when a new cart is instantiated.
    /// It is used to track the creation time of the cart for auditing and operational purposes.
    /// </remarks>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Represents the date and time when the cart was last modified.
    /// </summary>
    /// <remarks>
    /// This property is nullable and will be set to <c>null</c> if the cart has not been modified
    /// after its creation. It is updated whenever any modifications are made to the cart.
    /// </remarks>
    public DateTime? LastModifiedAt { get; set; }

    /// <summary>
    /// Represents a user entity in the system with various personal and relational attributes.
    /// </summary>
    /// <remarks>
    /// The User entity contains fields for personal details such as name, email, and phone number,
    /// as well as various collections representing the user's relationships with other entities,
    /// such as carts, followers, library games, reviews, payments, orders, achievements, and more.
    /// This class extends <see cref="SoftDeletableEntity"/>, implying it supports soft deletion behavior.
    /// </remarks>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Represents the collection of items associated with a shopping cart.
    /// This property establishes a relationship between the <see cref="Cart"/> entity
    /// and its corresponding <see cref="CartItem"/> entities, where each cart item
    /// contains details like the product, price, and optionally a bundle.
    /// </summary>
    public virtual ICollection<CartItem> CartItems { get; set; } = [];
}