namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for GameVersion repository, providing methods for managing game version data.
/// </summary>
public interface IProductVersionRepository
{
    /// <summary>
    /// Gets all game versions.
    /// </summary>
    /// <returns>A collection of all <see cref="ProductVersion"/> entities.</returns>
    Task<IEnumerable<ProductVersion>> GetAllAsync();

    /// <summary>
    /// Gets a game version by its ID.
    /// </summary>
    /// <param name="id">The ID of the game version.</param>
    /// <returns>The <see cref="ProductVersion"/> entity if found; otherwise, null.</returns>
    Task<ProductVersion?> GetByIdAsync(Guid id);


    /// <summary>
    /// Retrieves a product version based on the specified product ID, platform, and version.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="platform">The platform type (e.g., Windows, Mac, Linux).</param>
    /// <param name="version">The version string of the product.</param>
    /// <returns>The matching <see cref="ProductVersion"/>, or null if not found.</returns>
    Task<ProductVersion?> GetByProductIdPlatformAndVersionAsync(Guid productId, PlatformType platform, string version);

    /// <summary>
    /// Gets all versions of a specific game.
    /// </summary>
    /// <param name="productId">The ID of the game.</param>
    /// <returns>A collection of <see cref="ProductVersion"/> entities associated with the specified game.</returns>
    Task<IEnumerable<ProductVersion>> GetByProductIdAsync(Guid productId);

    /// <summary>
    /// Adds a new game version.
    /// </summary>
    /// <param name="productVersion">The game version to add.</param>
    /// <returns>The added <see cref="ProductVersion"/> entity.</returns>
    Task<ProductVersion?> AddAsync(ProductVersion productVersion);

    /// <summary>
    /// Updates an existing game version.
    /// </summary>
    /// <param name="productVersion">The game version to update.</param>
    /// <returns>The updated <see cref="ProductVersion"/> entity.</returns>
    Task<ProductVersion?> UpdateAsync(ProductVersion productVersion);

    /// <summary>
    /// Deletes a game version by its ID.
    /// </summary>
    /// <param name="id">The ID of the game version to delete.</param>
    Task DeleteAsync(Guid id);
}