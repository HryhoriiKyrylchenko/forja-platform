namespace Forja.Application.Interfaces.Common;

/// <summary>
/// Provides methods to retrieve data for filtering products in the application.
/// </summary>
public interface IFilterDataService
{
    /// Retrieves game-related filter data, including genres, mechanics, tags, and mature content.
    /// This method aggregates data from multiple repositories and returns a DTO containing the relevant filter lists.
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a
    /// ProductsFilterDataDto object with lists of genres, mechanics, tags, and mature contents.
    /// </returns>
    Task<ProductsFilterDataDto> GetGameFilterDataAsync();
}