namespace Forja.Infrastructure.Repositories.UserProfile;

/// <summary>
/// Repository for managing UserFollower entity data.
/// </summary>
public class UserFollowerRepository : IUserFollowerRepository
{
    private readonly ForjaDbContext _context;
    private readonly DbSet<UserFollower> _userFollowers;

    public UserFollowerRepository(ForjaDbContext context)
    {
        _context = context;
        _userFollowers = context.Set<UserFollower>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserFollower>> GetAllAsync()
    {
        return await _userFollowers
            .Include(uf => uf.Follower)
            .Include(uf => uf.Followed)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<UserFollower?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User follower id cannot be empty.", nameof(id));
        }
        
        return await _userFollowers
            .Include(uf => uf.Follower)
            .Include(uf => uf.Followed)
            .FirstOrDefaultAsync(uf => uf.Id == id);
    }

    /// <inheritdoc />
    public async Task<UserFollower?> AddAsync(UserFollower userFollower)
    {
        if (!UserProfileModelValidator.ValidateUserFollower(userFollower))
        {
            throw new ArgumentException("User follower is invalid.", nameof(userFollower));
        }
        await _userFollowers.AddAsync(userFollower);
        await _context.SaveChangesAsync();

        var addedUserFollower = await _userFollowers
                                            .Include(uf => uf.Follower)
                                            .Include(uf => uf.Followed)
                                            .FirstOrDefaultAsync(uf => uf.Id == userFollower.Id);
                                                    

        return addedUserFollower;
    }

    /// <inheritdoc />
    public async Task<UserFollower?> UpdateAsync(UserFollower userFollower)
    {
        if (!UserProfileModelValidator.ValidateUserFollower(userFollower))
        {
            throw new ArgumentException("User follower is invalid.", nameof(userFollower));
        }
        _userFollowers.Update(userFollower);
        await _context.SaveChangesAsync();
        
        var updatedUserFollower = await _userFollowers
                                                .Include(uf => uf.Follower)
                                                .Include(uf => uf.Followed)
                                                .FirstOrDefaultAsync(uf => uf.Id == userFollower.Id);
                                                    

        return updatedUserFollower;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var userFollower = await GetByIdAsync(id);
        if (userFollower != null)
        {
            _userFollowers.Remove(userFollower);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserFollower>> GetFollowersByUserIdAsync(Guid userId) // �������� �� ����
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        var loadedUserFollowers = await _userFollowers
            .Where(uf => uf.FollowedId == userId)
            .Include(uf => uf.Follower)
            .Include(u => u.Followed)
            .ToListAsync();

        return loadedUserFollowers;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserFollower>> GetFollowedByUserIdAsync(Guid userId) // �� ���� � �������
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        var loadedFollowedByUser = await _userFollowers
            .Where(uf => uf.FollowerId == userId)            
            .Include(uf => uf.Followed)
            .Include(u => u.Follower)
            .ToListAsync();

        return loadedFollowedByUser;
    }
}