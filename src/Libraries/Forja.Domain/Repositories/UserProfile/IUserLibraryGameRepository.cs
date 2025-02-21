namespace Forja.Domain.Repositories.UserProfile;

/// <summary>
/// Interface for managing UserLibraryGame entities in the repository.
/// </summary>
public interface IUserLibraryGameRepository
{
    /// <summary>
    /// Retrieves a user library game by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user library game to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// <see cref="UserLibraryGame"/> object if found; otherwise, null.
    /// </returns>
    Task<UserLibraryGame?> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves all user library games.
    /// </summary>
    /// <returns>A collection of <see cref="UserLibraryGame"/> entities.</returns>
    Task<IEnumerable<UserLibraryGame>> GetAllAsync();

    /// <summary>
    /// Asynchronously adds a new UserLibraryGame entity to the repository.
    /// </summary>
    /// <param name="userLibraryGame">The UserLibraryGame instance to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(UserLibraryGame userLibraryGame);

    /// <summary>
    /// Updates an existing UserLibraryGame entity in the data store asynchronously.
    /// </summary>
    /// <param name="userLibraryGame">
    /// The UserLibraryGame entity to be updated, containing any modified data to be persisted.
    /// </param>
    /// <returns>
    /// A Task representing the asynchronous operation.
    /// </returns>
    Task UpdateAsync(UserLibraryGame userLibraryGame);

    /// <summary>
    /// Deletes a UserLibraryGame entity from the repository by its identifier.
    /// </summary>
    /// <param name="userLibraryGameId">The unique identifier of the UserLibraryGame to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid userLibraryGameId);
}