namespace Forja.Domain.Repositories.Common;

/// <summary>
/// Repository interface for managing NewsArticle data.
/// </summary>
public interface INewsArticleRepository
{
    /// <summary>
    /// Retrieves a news article by its unique identifier.
    /// </summary>
    /// <param name="articleId">The unique identifier of the news article.</param>
    /// <returns>A news article if found; otherwise, null.</returns>
    Task<NewsArticle?> GetNewsArticleByIdAsync(Guid articleId);

    /// <summary>
    /// Retrieves all news articles, optionally filtered by publication date.
    /// </summary>
    /// <param name="publicationDate">The publication date to filter by, or null to retrieve all articles.</param>
    /// <returns>A collection of news articles.</returns>
    Task<IEnumerable<NewsArticle>> GetNewsArticlesByPublicationDateAsync(DateTime? publicationDate = null);

    /// <summary>
    /// Retrieves all active news articles.
    /// </summary>
    /// <returns>A collection of active news articles.</returns>
    Task<IEnumerable<NewsArticle>> GetActiveNewsArticlesAsync();

    /// <summary>
    /// Adds a new news article to the repository.
    /// </summary>
    /// <param name="article">The news article to add.</param>
    Task<NewsArticle?> AddNewsArticleAsync(NewsArticle article);

    /// <summary>
    /// Updates an existing news article in the repository.
    /// </summary>
    /// <param name="article">The updated news article object.</param>
    Task<NewsArticle?> UpdateNewsArticleAsync(NewsArticle article);

    /// <summary>
    /// Deletes a news article by its unique identifier.
    /// </summary>
    /// <param name="articleId">The unique identifier of the news article to delete.</param>
    Task DeleteNewsArticleAsync(Guid articleId);
}