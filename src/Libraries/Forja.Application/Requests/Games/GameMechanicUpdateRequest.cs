namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to update a game mechanic within a specific game.
/// </summary>
public class GameMechanicUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the game mechanic update request.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the game associated with the mechanic update request.
    /// </summary>
    [Required]
    public Guid GameId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the mechanic associated with the game.
    /// </summary>
    [Required]
    public Guid MechanicId { get; set; }
}