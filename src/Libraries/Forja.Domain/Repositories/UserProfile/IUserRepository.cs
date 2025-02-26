namespace Forja.Domain.Repositories.UserProfile;

/// <summary>
/// Represents the repository interface for managing User entities.
/// Provides methods for retrieving, adding, updating, and deleting users,
/// as well as checking for user existence by specific criteria.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Asynchronously retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier (GUID) of the user to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the user entity if found,
    /// or null if no matching user exists.
    /// </returns>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves a deleted user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier (GUID) of the deleted user to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the deleted user entity if found,
    /// or null if no matching deleted user exists.
    /// </returns>
    Task<User?> GetDeletedByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves a user by their Keycloak unique identifier.
    /// </summary>
    /// <param name="userKeycloakId">The Keycloak unique identifier of the user to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the user entity if found,
    /// or null if no matching user exists.
    /// </returns>
    Task<User?> GetByKeycloakIdAsync(string userKeycloakId);

    /// <summary>
    /// Asynchronously retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>A <see cref="User"/> object if a user with the specified email exists; otherwise, null.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Retrieves all users asynchronously from the data source.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains an enumerable of <see cref="User"/> objects.</returns>
    Task<IEnumerable<User>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all users that have been soft deleted.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing a collection of soft-deleted user entities.
    /// </returns>
    Task<IEnumerable<User>> GetAllDeletedAsync();

    /// <summary>
    /// Asynchronously adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user entity to be added.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(User user);

    /// <summary>
    /// Updates the given user entity in the repository.
    /// </summary>
    /// <param name="user">The user entity to be updated.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateAsync(User user);

    /// <summary>
    /// Deletes a user asynchronously by their unique identifier.
    /// If the user exists, it marks the user as deleted and updates the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(Guid userId);
    
    /// <summary>
    /// Restores a soft-deleted user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to restore.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RestoreAsync(Guid userId);


    /// <summary>
    /// Checks if a user with the specified username exists in the system.
    /// </summary>
    /// <param name="username">The username to check for existence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether a user with the specified username exists.</returns>
    Task<bool> ExistsByUsernameAsync(string username);
}