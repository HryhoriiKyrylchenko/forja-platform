namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for associating a tag with a game.
/// </summary>
public class GameTagDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the game tag.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the game associated with this tag.
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated tag.
    /// </summary>
    public Guid TagId { get; set; }
}