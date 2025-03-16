namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for the relationship between a game and its associated mechanic.
/// </summary>
public class GameMechanicDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the game mechanic.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the game associated with this instance.
    /// This property links the game mechanic to a specific game.
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Represents the unique identifier of a game mechanic associated with a game.
    /// </summary>
    public Guid MechanicId { get; set; }
}