namespace Forja.Domain.Entities.Support;

/// <summary>
/// Represents a Frequently Asked Question (FAQ) entity.
/// </summary>
[Table("FAQs", Schema = "support")]
public class FAQ : SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the FAQ.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the question of the FAQ.
    /// </summary>
    [Required]
    public string Question { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the answer to the FAQ.
    /// </summary>
    [Required]
    public string Answer { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the order of the FAQ.
    /// </summary>
    public int Order { get; set; } = 0;
}