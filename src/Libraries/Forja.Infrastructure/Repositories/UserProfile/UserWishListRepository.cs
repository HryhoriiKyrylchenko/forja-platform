namespace Forja.Infrastructure.Repositories.UserProfile;

/// <summary>
/// Repository for managing UserWishList entity data.
/// </summary>
public class UserWishListRepository : IUserWishListRepository
{
    private readonly DbContext _context;

    public UserWishListRepository(DbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserWishList>> GetAllAsync()
    {
        return await _context.Set<UserWishList>()
            .Include(uwl => uwl.Product)
            .Include(uwl => uwl.User)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<UserWishList?> GetByIdAsync(Guid id)
    {
        return await _context.Set<UserWishList>()
            .Include(uwl => uwl.Product)
            .Include(uwl => uwl.User)
            .FirstOrDefaultAsync(uwl => uwl.Id == id);
    }

    /// <inheritdoc />
    public async Task<UserWishList> AddAsync(UserWishList userWishList)
    {
        await _context.Set<UserWishList>().AddAsync(userWishList);
        await _context.SaveChangesAsync();
        return userWishList;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(UserWishList userWishList)
    {
        _context.Set<UserWishList>().Update(userWishList);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var userWishList = await GetByIdAsync(id);
        if (userWishList != null)
        {
            _context.Set<UserWishList>().Remove(userWishList);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserWishList>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Set<UserWishList>()
            .Where(uwl => uwl.UserId == userId)
            .Include(uwl => uwl.Product)
            .ToListAsync();
    }
}