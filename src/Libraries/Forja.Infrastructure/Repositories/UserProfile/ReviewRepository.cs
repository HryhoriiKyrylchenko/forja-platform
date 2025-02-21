namespace Forja.Infrastructure.Repositories.UserProfile;

public class ReviewRepository : IReviewRepository
{
    private readonly DbSet<Review> _reviews;
    
    public ReviewRepository(ForjaDbContext context)
    {
        _reviews = context.Set<Review>();
    }
    
    /// <inheritdoc />
    public async Task<Review?> GetByIdAsync(Guid id)
    {
        return await _reviews
            .Include(r => r.Game)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllByUserIdAsync(Guid userId)
    {
        return await _reviews
            .Include(r => r.Game)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllByGameIdAsync(Guid gameId)
    {
        return await _reviews
            .Include(r => r.Game)
            .Where(r => r.GameId == gameId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        return await _reviews
            .Include(r => r.Game)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(Review review)
    {
        await _reviews.AddAsync(review);
    }

    /// <inheritdoc />
    public Task UpdateAsync(Review review)
    {
        _reviews.Update(review);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid reviewId)
    {
        var review = await GetByIdAsync(reviewId);
        if (review != null)
        {
            review.IsDeleted = true;
            _reviews.Update(review);
        }
    }
}