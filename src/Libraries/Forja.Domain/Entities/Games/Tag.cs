namespace Forja.Domain.Entities.Games;

/// <summary>
/// Represents a tag entity in the games schema.
/// </summary>
[Table("Tags", Schema = "games")]
public class Tag
{
    /// <summary>
    /// Gets or sets the unique identifier for the tag.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the title of the tag.
    /// </summary>
    [Required]
    [MaxLength(30)]
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the collection of game tags associated with this tag.
    /// Virtual property for Entity Framework to handle related data.
    /// </summary>
    public virtual ICollection<GameTag> GameTags { get; set; } = [];
}