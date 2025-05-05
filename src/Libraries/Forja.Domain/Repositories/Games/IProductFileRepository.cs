namespace Forja.Domain.Repositories.Games;

/// <summary>
/// Interface for GameFile repository, providing methods for managing game file data.
/// </summary>
public interface IProductFileRepository
{
    /// <summary>
    /// Gets all game files.
    /// </summary>
    /// <returns>A collection of all <see cref="ProductFile"/> entities.</returns>
    Task<IEnumerable<ProductFile>> GetAllAsync();

    /// <summary>
    /// Gets a game file by its ID.
    /// </summary>
    /// <param name="id">The ID of the game file.</param>
    /// <returns>The <see cref="ProductFile"/> entity if found; otherwise, null.</returns>
    Task<ProductFile?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a game file by its associated game version ID and file name.
    /// </summary>
    /// <param name="productVersionId">The ID of the game version associated with the game file.</param>
    /// <param name="fileName">The name of the game file.</param>
    /// <returns>The <see cref="ProductFile"/> entity if found; otherwise, null.</returns>
    Task<ProductFile?> GetGameFileByGameVersionIdAndFileName(Guid productVersionId, string fileName);

    /// <summary>
    /// Gets all files for a specific game version.
    /// </summary>
    /// <param name="gameVersionId">The ID of the game version.</param>
    /// <returns>A collection of <see cref="ProductFile"/> entities associated with the specified game version.</returns>
    Task<IEnumerable<ProductFile>> GetByGameVersionIdAsync(Guid gameVersionId);

    /// <summary>
    /// Adds a new game file.
    /// </summary>
    /// <param name="productFile">The game file to add.</param>
    /// <returns>The added <see cref="ProductFile"/> entity.</returns>
    Task<ProductFile?> AddAsync(ProductFile productFile);

    /// <summary>
    /// Updates an existing game file.
    /// </summary>
    /// <param name="productFile">The game file to update.</param>
    /// <returns>The updated <see cref="ProductFile"/> entity.</returns>
    Task<ProductFile?> UpdateAsync(ProductFile productFile);

    /// <summary>
    /// Deletes a game file by its ID.
    /// </summary>
    /// <param name="id">The ID of the game file to delete.</param>
    Task DeleteAsync(Guid id);


    /// <summary>
    /// Finds a product file by platform type, version, product ID, and file name.
    /// </summary>
    /// <param name="platform">The platform for which the product file is associated (e.g., Windows, Mac, Linux).</param>
    /// <param name="version">The version of the product.</param>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="fileName">The name of the file to search for.</param>
    /// <returns>A <see cref="ProductFile"/> entity if found, or null if no matching file exists.</returns>
    Task<ProductFile?> FindByPlatformVersionProductIdAndNameAsync(PlatformType platform, string version, Guid productId, string fileName);
}