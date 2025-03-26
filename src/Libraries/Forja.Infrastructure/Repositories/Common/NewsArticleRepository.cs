namespace Forja.Infrastructure.Repositories.Common;

/// <summary>
/// Implementation of the NewsArticle repository interface.
/// </summary>
public class NewsArticleRepository : INewsArticleRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<NewsArticle> _newsArticles;

    public NewsArticleRepository(ForjaDbContext context)
    {
        _context = context;
        _newsArticles = context.Set<NewsArticle>();
    }

    /// <inheritdoc />
    public async Task<NewsArticle?> GetNewsArticleByIdAsync(Guid articleId)
    {
        if (articleId == Guid.Empty)
        {
            throw new ArgumentException("Invalid article ID.", nameof(articleId));
        }

        return await _newsArticles
            .Include(na => na.Product)
            .FirstOrDefaultAsync(na => na.Id == articleId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<NewsArticle>> GetNewsArticlesByPublicationDateAsync(DateTime? publicationDate = null)
    {
        var query = _newsArticles.AsQueryable();

        if (publicationDate.HasValue)
        {
            query = query.Where(na => na.PublicationDate.Date == publicationDate.Value.Date);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<NewsArticle>> GetActiveNewsArticlesAsync()
    {
        return await _newsArticles
            .Where(na => na.IsActive)
            .OrderByDescending(na => na.PublicationDate)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<NewsArticle?> AddNewsArticleAsync(NewsArticle article)
    {
        if (!CommonModelValidator.ValidateNewsArticleModel(article, out var errors))
        {
            throw new ArgumentException($"Invalid news article. Error: {errors}", nameof(article));
        }

        await _newsArticles.AddAsync(article);
        await _context.SaveChangesAsync();
        
        return article;
    }

    /// <inheritdoc />
    public async Task<NewsArticle?> UpdateNewsArticleAsync(NewsArticle article)
    {
        if (!CommonModelValidator.ValidateNewsArticleModel(article, out var errors))
        {
            throw new ArgumentException($"Invalid news article. Error: {errors}", nameof(article));
        }

        _newsArticles.Update(article);
        await _context.SaveChangesAsync();
        
        return article;
    }

    /// <inheritdoc />
    public async Task DeleteNewsArticleAsync(Guid articleId)
    {
        if (articleId == Guid.Empty)
        {
            throw new ArgumentException("Invalid article ID.", nameof(articleId));
        }

        var article = await _newsArticles.FirstOrDefaultAsync(na => na.Id == articleId);

        if (article == null)
        {
            throw new KeyNotFoundException($"NewsArticle with ID {articleId} not found.");
        }

        _newsArticles.Remove(article);
        await _context.SaveChangesAsync();
    }
}