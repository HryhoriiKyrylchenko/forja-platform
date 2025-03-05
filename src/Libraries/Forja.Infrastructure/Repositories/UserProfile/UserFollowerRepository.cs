namespace Forja.Infrastructure.Repositories.UserProfile;

/// <summary>
/// Repository for managing UserFollower entity data.
/// </summary>
public class UserFollowerRepository : IUserFollowerRepository
{
    private readonly DbContext _context;

    public UserFollowerRepository(DbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserFollower>> GetAllAsync()
    {
        return await _context.Set<UserFollower>()
            .Include(uf => uf.Follower)
            .Include(uf => uf.Followed)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<UserFollower?> GetByIdAsync(Guid id)
    {
        return await _context.Set<UserFollower>()
            .Include(uf => uf.Follower)
            .Include(uf => uf.Followed)
            .FirstOrDefaultAsync(uf => uf.Id == id);
    }

    /// <inheritdoc />
    public async Task<UserFollower> AddAsync(UserFollower userFollower)
    {
        await _context.Set<UserFollower>().AddAsync(userFollower);
        await _context.SaveChangesAsync();
        return userFollower;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(UserFollower userFollower)
    {
        _context.Set<UserFollower>().Update(userFollower);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var userFollower = await GetByIdAsync(id);
        if (userFollower != null)
        {
            _context.Set<UserFollower>().Remove(userFollower);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserFollower>> GetFollowersByUserIdAsync(Guid userId)
    {
        return await _context.Set<UserFollower>()
            .Where(uf => uf.FollowedId == userId)
            .Include(uf => uf.Follower)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserFollower>> GetFollowedByUserIdAsync(Guid userId)
    {
        return await _context.Set<UserFollower>()
            .Where(uf => uf.FollowerId == userId)
            .Include(uf => uf.Followed)
            .ToListAsync();
    }
}