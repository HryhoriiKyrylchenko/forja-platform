namespace Forja.Application.Interfaces.Games;

/// <summary>
/// Service interface for managing game versions.
/// </summary>
public interface IProductVersionService
{
    /// <summary>
    /// Adds a new game version to the repository.
    /// </summary>
    /// <param name="request">The request object containing details of the game version to be added.</param>
    /// <returns>
    /// A <see cref="ProductVersionDto"/> object representing the added game version, or null if the addition failed.
    /// </returns>
    Task<ProductVersionDto?> AddProductVersionAsync(ProductVersionCreateRequest request);

    /// <summary>
    /// Updates an existing game version with new details.
    /// </summary>
    /// <param name="request">The update request containing the game version ID and updated details such as changelog, release date, etc.</param>
    /// <returns>A <see cref="ProductVersionDto"/> representing the updated game version, or null if the operation was unsuccessful.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided request is invalid.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a game version with the specified ID is not found.</exception>
    Task<ProductVersionDto?> UpdateProductVersionAsync(ProductVersionUpdateRequest request);


    /// <summary>
    /// Retrieves the product version details for a specific product ID, platform, and version.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="platform">The platform type of the product version (e.g., Windows, Mac, Linux).</param>
    /// <param name="version">The version string of the product to be retrieved.</param>
    /// <returns>
    /// A <see cref="ProductVersionDto"/> object containing the details of the specified product version, or null if no matching version is found.
    /// </returns>
    Task<ProductVersionDto?> GetProductVersionByProductIdPlatformAndVersionAsync(Guid productId, PlatformType platform, string version);
}