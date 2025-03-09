namespace Forja.Infrastructure.Repositories.UserProfile;

public class ReviewRepository : IReviewRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<Review> _reviews;
    
    public ReviewRepository(ForjaDbContext context)
    {
        _context = context;
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
    public async Task<(int positive, int negative)> GetProductApprovedReviewsCountAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product id cannot be empty.", nameof(productId));
        }
        
        var positive = await _reviews
            .Where(r => r.ProductId == productId && !r.IsDeleted && r.IsApproved && r.PositiveRating)
            .CountAsync();
        
        var negative = await _reviews
            .Where(r => r.ProductId == productId && !r.IsDeleted && r.IsApproved && !r.PositiveRating)
            .CountAsync();
        
        return (positive, negative);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(productId));
        }
        
        return await _reviews
            .Where(r => r.ProductId == productId)
            .Where(r => !r.IsDeleted)
            .ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetAllDeletedByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Game id cannot be empty.", nameof(productId));
        }
        
        return await _reviews
            .Where(r => r.ProductId == productId)
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
    public async Task<Review?> AddAsync(Review review)
    {
        if (!UserProfileModelValidator.ValidateReview(review))
        {
            throw new ArgumentException("Review is not valid.", nameof(review));
        }
        
        await _reviews.AddAsync(review);
        await _context.SaveChangesAsync();
        
        return review;
    }

    /// <inheritdoc />
    public async Task<Review?> UpdateAsync(Review review)
    {
        if (!UserProfileModelValidator.ValidateReview(review))
        {
            throw new ArgumentException("Review is not valid.", nameof(review));
        }
        
        _reviews.Update(review);
        await _context.SaveChangesAsync();
        
        return review;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review id cannot be empty.", nameof(reviewId));
        }
        
        var review = await GetByIdAsync(reviewId) 
                     ?? throw new ArgumentException("Review not found.", nameof(reviewId));
        
        review.IsDeleted = true;
        _reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<Review?> RestoreAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty)
        {
            throw new ArgumentException("Review id cannot be empty.", nameof(reviewId));
        }
        
        var review = await GetByIdAsync(reviewId) 
                     ?? throw new ArgumentException("Review not found.", nameof(reviewId));

        if (!review.IsDeleted)
        {
            return review;
        }
        
        review.IsDeleted = false;
        _reviews.Update(review);
        await _context.SaveChangesAsync();
        
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