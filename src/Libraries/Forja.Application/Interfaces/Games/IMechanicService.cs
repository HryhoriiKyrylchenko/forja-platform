namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Provides operations for managing game mechanics within the system.
/// </summary>
public interface IMechanicService
{
    /// <summary>
    /// Asynchronously retrieves all available game mechanics.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing an enumerable collection
    /// of <c>MechanicDto</c> objects.
    /// </returns>
    Task<IEnumerable<MechanicDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a collection of all deleted game mechanics as data transfer objects (DTOs).
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a collection of <see cref="MechanicDto"/>
    /// objects representing deleted game mechanics.
    /// </returns>
    Task<IEnumerable<MechanicDto>> GetAllDeletedAsync();

    /// <summary>
    /// Retrieves a mechanic entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the mechanic to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the <see cref="MechanicDto"/> if found; otherwise, null.
    /// </returns>
    Task<MechanicDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Creates a new game mechanic based on the provided request.
    /// </summary>
    /// <param name="request">The details of the mechanic to be created. Must include name and other optional information.</param>
    /// <returns>A data transfer object containing the details of the created mechanic, or null if creation fails.</returns>
    Task<MechanicDto?> CreateAsync(MechanicCreateRequest request);

    /// <summary>
    /// Updates an existing mechanic entity in the system with the provided data.
    /// </summary>
    /// <param name="request">An instance of <see cref="MechanicUpdateRequest"/> containing the details to update the mechanic.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result contains an instance of <see cref="MechanicDto"/> representing the updated mechanic, or null if the update fails.</returns>
    Task<MechanicDto?> UpdateAsync(MechanicUpdateRequest request);

    /// <summary>
    /// Deletes a mechanic by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the mechanic to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided id is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no mechanic is found with the specified id.</exception>
    Task DeleteAsync(Guid id);
}