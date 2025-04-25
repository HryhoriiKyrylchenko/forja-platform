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
    /// Asynchronously retrieves a deleted user by their Keycloak unique identifier.
    /// </summary>
    /// <param name="userKeycloakId">The unique identifier associated with the user's Keycloak account.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the deleted user entity if found,
    /// or null if no matching deleted user exists.
    /// </returns>
    Task<User?> GetDeletedByKeycloakIdAsync(string userKeycloakId);

    /// <summary>
    /// Asynchronously retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>A <see cref="User"/> object if a user with the specified email exists; otherwise, null.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Asynchronously retrieves a deleted user by their email address.
    /// </summary>
    /// <param name="email">The email address of the deleted user to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the deleted user entity if found,
    /// or null if no matching deleted user exists.
    /// </returns>
    Task<User?> GetDeletedByEmailAsync(string email);

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
    /// Asynchronously adds a new user entity to the repository.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the added user entity with populated data from the repository.
    /// </returns>
    Task<User?> AddAsync(User user);

    /// <summary>
    /// Updates an existing user entity asynchronously.
    /// </summary>
    /// <param name="user">The user entity containing the updated information.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the updated user entity.
    /// </returns>
    Task<User?> UpdateAsync(User user);

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
    /// <param name="userId">The unique identifier (GUID) of the user to restore.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the restored user entity.
    /// </returns>
    Task<User?> RestoreAsync(Guid userId);

    /// <summary>
    /// Checks if a user with the specified username exists in the system.
    /// </summary>
    /// <param name="username">The username to check for existence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether a user with the specified username exists.</returns>
    Task<bool> ExistsByUsernameAsync(string username);

    /// <summary>
    /// Asynchronously generates a unique username based on a given base username.
    /// </summary>
    /// <param name="baseUsername">The base username to use for generating a unique username.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the generated unique username as a string.
    /// </returns>
    Task<string> GenerateUniqueUsernameAsync(string baseUsername);

    /// <summary>
    /// Asynchronously retrieves a user entity by a unique identifier.
    /// </summary>
    /// <param name="identifier">The unique identifier (e.g., username, email, or custom identifier) of the user to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the user entity if found,
    /// or null if no matching user exists.
    /// </returns>
    Task<User?> GetByIdentifierAsync(string identifier);
}