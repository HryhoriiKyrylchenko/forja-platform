namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for a tag associated with a game.
/// </summary>
public class TagDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the tag.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the tag.
    /// </summary>
    public string Title { get; set; } = string.Empty;
}