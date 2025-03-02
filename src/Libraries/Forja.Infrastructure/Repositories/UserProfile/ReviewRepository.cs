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
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Review id cannot be empty.", nameof(id));    
        }
        
        return await _reviews
            .Where(r => !r.IsDeleted)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }
        
        return await _reviews
            .Where(r => r.UserId == userId)
            .Where(r => !r.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllDeletedByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }
        
        return await _reviews
            .Where(r => r.UserId == userId)
            .Where(r => r.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(gameId));
        }
        
        return await _reviews
            .Where(r => r.GameId == gameId)
            .Where(r => !r.IsDeleted)
            .ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllDeletedByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(gameId));
        }
        
        return await _reviews
            .Where(r => r.GameId == gameId)
            .Where(r => r.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        return await _reviews
            .Where(r => !r.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task AddAsync(Review review)
    {
        if (!ProjectModelValidator.ValidateReview(review))
        {
            throw new ArgumentException("Review is not valid.", nameof(review));
        }
        
        await _reviews.AddAsync(review);
    }

    /// <inheritdoc />
    public Task UpdateAsync(Review review)
    {
        if (!ProjectModelValidator.ValidateReview(review))
        {
            throw new ArgumentException("Review is not valid.", nameof(review));
        }
        
        _reviews.Update(review);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review id cannot be empty.", nameof(reviewId));
        }
        
        var review = await GetByIdAsync(reviewId);
        if (review != null)
        {
            review.IsDeleted = true;
            _reviews.Update(review);
        }
    }

    /// <inheritdoc />
    public async Task<Review> RestoreAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review id cannot be empty.", nameof(reviewId));
        }
        
        var review = await GetByIdAsync(reviewId);
        if (review == null)
        {
            throw new ArgumentException("Review not found.", nameof(reviewId));
        }

        if (!review.IsDeleted)
        {
            return review;
        }
        
        review.IsDeleted = false;
        _reviews.Update(review);
        
        return review;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllNotApprovedAsync()
    {
        return await _reviews
            .Where(r => !r.IsApproved)
            .Where(r => !r.IsDeleted)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllDeletedAsync()
    {
        return await _reviews
            .Where(r => !r.IsDeleted)
            .ToListAsync();
    }
}