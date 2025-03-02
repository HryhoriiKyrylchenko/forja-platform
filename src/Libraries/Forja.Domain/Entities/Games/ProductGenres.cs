namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents the relationship between a game and a genre within the system.
/// This entity defines the many-to-many association between games and genres.
/// </summary>
[Table("ProductGenres", Schema = "games")]
public class ProductGenres
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// This property serves as the primary key, ensuring the uniqueness
    /// of each record within the associated database table.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the unique identifier of the product associated with this entity.
    /// This property establishes a foreign key relationship with the Product entity.
    /// </summary>
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated genre for a game.
    /// This property establishes the relationship between a specific game and a genre,
    /// reflecting the many-to-many relationship in the system.
    /// </summary>
    [ForeignKey("Genre")]
    public Guid GenreId { get; set; }

    /// <summary>
    /// Represents a product entity with several properties defining its characteristics
    /// and collections representing its relationships with other entities.
    /// </summary>
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Represents the genre associated with a game within the system.
    /// This property establishes the relationship between a game and its specific genre.
    /// </summary>
    public virtual Genre Genre { get; set; } = null!;
}