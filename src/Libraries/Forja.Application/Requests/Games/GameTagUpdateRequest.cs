namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update the tag associated with a game.
/// </summary>
public class GameTagUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the game tag update request.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the game associated with the tag update request.
    /// </summary>
    [Required]
    public Guid GameId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the tag associated with the game.
    /// </summary>
    [Required]
    public Guid TagId { get; set; }
}