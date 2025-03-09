namespace Forja.Infrastructure.Repositories.UserProfile;

/// <summary>
/// Repository for managing UserWishList entity data.
/// </summary>
public class UserWishListRepository : IUserWishListRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<UserWishList> _userWishLists;

    public UserWishListRepository(ForjaDbContext context)
    {
        _context = context;
        _userWishLists = context.Set<UserWishList>();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<UserWishList>> GetAllAsync()
    {
        return await _userWishLists
            .Include(uwl => uwl.Product)
            .Include(uwl => uwl.User)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<UserWishList?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User wish list id cannot be empty.", nameof(id));
        }
        return await _userWishLists
            .Include(uwl => uwl.Product)
            .Include(uwl => uwl.User)
            .FirstOrDefaultAsync(uwl => uwl.Id == id);
    }

    /// <inheritdoc />
    public async Task<UserWishList?> AddAsync(UserWishList userWishList)
    {
        if (!UserProfileModelValidator.ValidateUserWishList(userWishList))
        {
            throw new ArgumentException("User wish list is invalid.", nameof(userWishList));
        }
        await _userWishLists.AddAsync(userWishList);
        await _context.SaveChangesAsync();
        return userWishList;
    }

    /// <inheritdoc />
    public async Task<UserWishList?> UpdateAsync(UserWishList userWishList)
    {
        if (!UserProfileModelValidator.ValidateUserWishList(userWishList))
        {
            throw new ArgumentException("User wish list is invalid.", nameof(userWishList));
        }
        _userWishLists.Update(userWishList);
        await _context.SaveChangesAsync();
        return userWishList;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User wish list id cannot be empty.", nameof(id));
        }
        var userWishList = await GetByIdAsync(id);
        if (userWishList != null)
        {
            _userWishLists.Remove(userWishList);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserWishList>> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }
        return await _userWishLists
            .Where(uwl => uwl.UserId == userId)
            .Include(uwl => uwl.Product)
            .ToListAsync();
    }
}