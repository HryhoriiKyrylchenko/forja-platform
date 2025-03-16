namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to associate a mechanic with a game.
/// </summary>
public class GameMechanicCreateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the game.
    /// </summary>
    [Required]
    public Guid GameId { get; set; }

    /// <summary>
    /// Represents the unique identifier of the game mechanic to be created.
    /// </summary>
    [Required]
    public Guid MechanicId { get; set; }
}