namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Interface for managing mature content operations in the application.
/// Provides methods for retrieving, creating, updating, and deleting mature content records.
/// </summary>
public interface IMatureContentService
{
    /// <summary>
    /// Retrieves all mature content entities asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// a collection of <see cref="MatureContentDto"/> objects representing the mature content entities.
    /// </returns>
    Task<IEnumerable<MatureContentDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves a mature content record by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the mature content to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="MatureContentDto"/>
    /// if found, otherwise null.</returns>
    Task<MatureContentDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously creates a new mature content entity based on the provided request data.
    /// </summary>
    /// <param name="request">The request object containing the data required to create a new mature content item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created mature content as a <see cref="MatureContentDto"/>, or null if the creation fails.</returns>
    Task<MatureContentDto?> CreateAsync(MatureContentCreateRequest request);

    /// <summary>
    /// Updates the details of mature content based on the provided update request.
    /// </summary>
    /// <param name="request">The request containing the details to update, including the identifier of the mature content and updated properties.</param>
    /// <returns>The updated mature content as a Data Transfer Object (DTO), or null if the update operation fails or the content does not exist.</returns>
    Task<MatureContentDto?> UpdateAsync(MatureContentUpdateRequest request);

    /// <summary>
    /// Deletes a mature content entry based on the provided identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the mature content to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided id is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no mature content is found with the specified id.</exception>
    Task DeleteAsync(Guid id);
}