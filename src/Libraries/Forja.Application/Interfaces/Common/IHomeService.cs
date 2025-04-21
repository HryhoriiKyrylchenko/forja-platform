namespace Forja.Application.Interfaces.Common;

/// <summary>
/// Defines a service interface for handling operations related to the homepage.
/// </summary>
public interface IHomeService
{
    /// Asynchronously retrieves data needed for the homepage, including a list of top games, popular games by genre,
    /// available bundles, and recent news articles.
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an object of type HomepageDto
    /// including the top games, popular games by genre, bundles, and news articles.
    /// </returns>
    Task<HomepageDto> GetHomepageDataAsync();
}