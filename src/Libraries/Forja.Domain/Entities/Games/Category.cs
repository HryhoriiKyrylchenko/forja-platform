namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a category entity in the games domain.
/// </summary>
[Table("Categories", Schema = "games")]
public class Category : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the category.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of game categories associated with this category.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<GameCategory> GameCategories { get; set; } = [];
}