namespace Forja.Application.Requests.Games;

/// <summary>
/// Represents a data transfer object used for updating a genre in the system.
/// </summary>
/// <remarks>
/// This request ensures that the required fields such as Id and Name are properly validated
/// in accordance with the provided constraints.
/// </remarks>
public class GenreUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the genre update request.
    /// </summary>
    /// <remarks>
    /// This property is required and represents the unique identifier (GUID) associated with the genre being updated.
    /// </remarks>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the genre.
    /// </summary>
    /// <remarks>
    /// This property is required and has a maximum length of 100 characters.
    /// </remarks>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the genre is marked as deleted.
    /// </summary>
    /// <remarks>
    /// This property is used to determine the deletion status of a genre, typically for soft delete functionality.
    /// </remarks>
    public bool IsDeleted { get; set; }
}