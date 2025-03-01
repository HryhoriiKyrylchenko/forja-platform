namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a genre for games (e.g., Action, RPG, etc.).
/// </summary>
[Table("Genres", Schema = "games")]
public class Genre : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the genre.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the genre.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of game genres associated with this genre.
    /// </summary>
    public virtual ICollection<ProductGenres> ProductGenres { get; set; } = [];
}
