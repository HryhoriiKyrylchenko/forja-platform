namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to create an association between a game and a tag.
/// </summary>
public class GameTagCreateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the game associated with the tag.
    /// </summary>
    [Required]
    public Guid GameId { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier of the tag associated with the game.
    /// </summary>
    [Required]
    public Guid TagId { get; set; }
}