namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents a data transfer object for a game genre.
/// </summary>
public class GenreDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the genre.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the genre.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}