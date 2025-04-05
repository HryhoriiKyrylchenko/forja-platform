namespace Forja.Application.Interfaces.Common;

/// <summary>
/// Service interface for handling operations related to News Articles.
/// </summary>
public interface INewsArticleService
{
    /// <summary>
    /// Retrieves a news article identified by its unique ID asynchronously.
    /// </summary>
    /// <param name="articleId">The unique identifier of the news article to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="NewsArticleDto"/> if found, otherwise null.</returns>
    Task<NewsArticleDto?> GetNewsArticleByIdAsync(Guid articleId);

    /// <summary>
    /// Retrieves a list of news articles based on the specified publication date.
    /// If no date is provided, returns all available articles.
    /// </summary>
    /// <param name="publicationDate">The publication date to filter the articles. If null, retrieves all articles.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an IEnumerable of NewsArticleDto, representing the filtered news articles.</returns>
    Task<List<NewsArticleDto>> GetNewsArticlesByPublicationDateAsync(DateTime? publicationDate = null);

    /// <summary>
    /// Retrieves a collection of active news articles.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of active news articles.</returns>
    Task<List<NewsArticleDto>> GetActiveNewsArticlesAsync();

    /// <summary>
    /// Creates a new News Article with the specified details and returns the created article.
    /// </summary>
    /// <param name="request">The request object containing details required to create the news article.</param>
    /// <returns>A <see cref="NewsArticleDto"/> representing the newly created news article.</returns>
    Task<NewsArticleDto?> CreateNewsArticleAsync(NewsArticleCreateRequest request);

    /// <summary>
    /// Updates an existing news article with the provided data.
    /// </summary>
    /// <param name="request">The request containing updated data for the news article.</param>
    /// <returns>Returns the updated news article as a <see cref="NewsArticleDto"/> object.</returns>
    Task<NewsArticleDto?> UpdateNewsArticleAsync(NewsArticleUpdateRequest request);

    /// <summary>
    /// Deletes a news article identified by the provided article ID.
    /// </summary>
    /// <param name="articleId">The unique identifier of the news article to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteNewsArticleAsync(Guid articleId);
}