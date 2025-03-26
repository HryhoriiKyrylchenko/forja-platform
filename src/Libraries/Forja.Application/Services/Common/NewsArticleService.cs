namespace Forja.Application.Services.Common;

/// <summary>
/// Service implementation for handling operations related to News Articles.
/// </summary>
public class NewsArticleService : INewsArticleService
{
    private readonly INewsArticleRepository _newsArticleRepository;

    public NewsArticleService(INewsArticleRepository newsArticleRepository)
    {
        _newsArticleRepository = newsArticleRepository;
    }

    /// <inheritdoc />
    public async Task<NewsArticleDto?> GetNewsArticleByIdAsync(Guid articleId)
    {
        if (articleId == Guid.Empty)
        {
            throw new ArgumentException("Article ID cannot be empty.", nameof(articleId));
        }

        var article = await _newsArticleRepository.GetNewsArticleByIdAsync(articleId);

        return article == null ? null : CommonEntityToDtoMapper.MapToNewsArticleDto(article);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<NewsArticleDto>> GetNewsArticlesByPublicationDateAsync(DateTime? publicationDate = null)
    {
        var articles = await _newsArticleRepository.GetNewsArticlesByPublicationDateAsync(publicationDate);

        return articles.Select(CommonEntityToDtoMapper.MapToNewsArticleDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<NewsArticleDto>> GetActiveNewsArticlesAsync()
    {
        var activeArticles = await _newsArticleRepository.GetActiveNewsArticlesAsync();

        return activeArticles.Select(CommonEntityToDtoMapper.MapToNewsArticleDto);
    }

    /// <inheritdoc />
    public async Task<NewsArticleDto?> CreateNewsArticleAsync(NewsArticleCreateRequest request)
    {
        if (!CommonRequestsValidator.ValidateNewsArticleCreateRequest(request, out var error))
        {
            throw new ArgumentException($"Invalid request. Errors: {error}", nameof(request));
        }

        var article = new NewsArticle
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            PublicationDate = request.PublicationDate,
            IsPrioritized = request.IsPrioritized,
            FileContent = request.FileContent,
            ImageUrl = request.ImageUrl,
            AuthorId = request.AuthorId,
            ProductId = request.ProductId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var createdArticle = await _newsArticleRepository.AddNewsArticleAsync(article);

        return createdArticle == null ? null : CommonEntityToDtoMapper.MapToNewsArticleDto(createdArticle);
    }

    /// <inheritdoc />
    public async Task<NewsArticleDto?> UpdateNewsArticleAsync(NewsArticleUpdateRequest request)
    {
        if (!CommonRequestsValidator.ValidateNewsArticleUpdateRequest(request, out var error))
        {
            throw new ArgumentException($"Invalid request. Errors: {error}", nameof(request));
        }

        var article = await _newsArticleRepository.GetNewsArticleByIdAsync(request.Id);

        if (article == null)
        {
            throw new KeyNotFoundException($"News article with ID {request.Id} not found.");
        }

        article.Title = request.Title;
        article.Content = request.Content;
        article.PublicationDate = request.PublicationDate;
        article.IsActive = request.IsActive;
        article.IsPrioritized = request.IsPrioritized;
        article.ImageUrl = request.ImageUrl;
        article.ProductId = request.ProductId;
        article.FileContent = request.FileContent;

        var updatedArticle = await _newsArticleRepository.UpdateNewsArticleAsync(article);

        return updatedArticle == null ? null : CommonEntityToDtoMapper.MapToNewsArticleDto(updatedArticle);
    }

    /// <inheritdoc />
    public async Task DeleteNewsArticleAsync(Guid articleId)
    {
        if (articleId == Guid.Empty)
        {
            throw new ArgumentException("Article ID cannot be empty.", nameof(articleId));
        }

        await _newsArticleRepository.DeleteNewsArticleAsync(articleId);
    }
}