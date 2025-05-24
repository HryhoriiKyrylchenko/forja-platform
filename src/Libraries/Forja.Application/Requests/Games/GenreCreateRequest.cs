namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a request to create a new genre.
/// </summary>
public class GenreCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the genre.
    /// This property is required and has a maximum length of 100 characters.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}