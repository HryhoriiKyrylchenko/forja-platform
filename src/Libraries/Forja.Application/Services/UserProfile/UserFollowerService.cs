namespace Forja.Application.Services.UserProfile;

/// <summary>
/// Service class for UserFollower functionality.
/// Provides logic and orchestration for managing UserFollower entities.
/// </summary>
public class UserFollowerService : IUserFollowerService
{
    private readonly IUserFollowerRepository _userFollowerRepository;

    public UserFollowerService(IUserFollowerRepository userFollowerRepository)
    {
        _userFollowerRepository = userFollowerRepository;
    }

    /// <inheritdoc />
    public async Task<List<UserFollowerDto>> GetAllAsync()
    {
        var userFollowers = await _userFollowerRepository.GetAllAsync();
        
        userFollowers = userFollowers.ToList();
        if (!userFollowers.Any())
        {
            return new List<UserFollowerDto>();
        }
        
        return userFollowers.Select(UserProfileEntityToDtoMapper.MapToUserFollowerDto).ToList();
    }
    
    /// <inheritdoc />
    public async Task<UserFollowerDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var userFollower = await _userFollowerRepository.GetByIdAsync(id);
        
        return userFollower == null ? null : UserProfileEntityToDtoMapper.MapToUserFollowerDto(userFollower);
    }

    /// <inheritdoc />
    public async Task<UserFollowerDto?> AddAsync(UserFollowerCreateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateUserFollowerCreateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var userFollower = new UserFollower
        {
            Id = Guid.NewGuid(),
            FollowerId = request.FollowerId,
            FollowedId = request.FollowedId
        };

        var createdUserFollower = await _userFollowerRepository.AddAsync(userFollower);
        return createdUserFollower == null ? null : UserProfileEntityToDtoMapper.MapToUserFollowerDto(createdUserFollower);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(UserFollowerUpdateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateUserFollowerUpdateRequest(request))
        {
            throw new ArgumentException("Invalid request.", nameof(request));
        }
        
        var userFollower = await _userFollowerRepository.GetByIdAsync(request.Id);
        if (userFollower == null)
        {
            throw new KeyNotFoundException($"UserFollower with ID {request.Id} not found.");
        }
            
        userFollower.FollowerId = request.FollowerId;
        userFollower.FollowedId = request.FollowedId;

        await _userFollowerRepository.UpdateAsync(userFollower);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        await _userFollowerRepository.DeleteAsync(id);
    }

    /// <inheritdoc />
    public async Task<List<UserFollowerDto>> GetFollowersByUserIdAsync(Guid userId) // підписані на мене
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(userId));
        }
        
        var followers = await _userFollowerRepository.GetFollowersByUserIdAsync(userId);
        return followers.Select(UserProfileEntityToDtoMapper.MapToUserFollowerDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserFollowerDto>> GetFollowedByUserIdAsync(Guid userId) // я підписан
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(userId));
        }
        
        var followedUsers = await _userFollowerRepository.GetFollowedByUserIdAsync(userId);
        return followedUsers.Select(UserProfileEntityToDtoMapper.MapToUserFollowerDto).ToList();
    }

    #region User Statistics Methods
    /// <summary>
    ///  Retrieves the count of followers for a specified user and updates the provided statistics object.
    /// </summary>
    /// <param name="userId">Identifies the user for whom the follower count is being retrieved.</param>
    /// <param name="statisticsDto">Holds the statistics data that will be updated with the follower count.</param>
    /// <returns>Returns the updated statistics object containing the follower count.</returns>
    /// <exception cref="ArgumentException">Thrown when the identifier for the user is empty.</exception>
    public async Task<int> GetFollowersCountAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(userId));
        }

        var followers = await _userFollowerRepository.GetFollowersCountByUserIdAsync(userId);

        return followers;
    }

    #endregion
}