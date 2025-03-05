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
    public async Task<IEnumerable<UserFollowerDto>> GetAllAsync()
    {
        var userFollowers = await _userFollowerRepository.GetAllAsync();
        return userFollowers.Select(MapToUserFollowerDto);
    }
    
    /// <inheritdoc />
    public async Task<UserFollowerDto?> GetByIdAsync(Guid id)
    {
        var userFollower = await _userFollowerRepository.GetByIdAsync(id);
        return userFollower == null ? null : MapToUserFollowerDto(userFollower);
    }

    /// <inheritdoc />
    public async Task<UserFollowerDto> AddAsync(Guid followerId, Guid followedId)
    {
        var userFollower = new UserFollower
        {
            Id = Guid.NewGuid(),
            FollowerId = followerId,
            FollowedId = followedId
        };

        var createdUserFollower = await _userFollowerRepository.AddAsync(userFollower);
        return MapToUserFollowerDto(createdUserFollower);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Guid id, Guid followerId, Guid followedId)
    {
        var userFollower = await _userFollowerRepository.GetByIdAsync(id);
        if (userFollower == null)
            throw new KeyNotFoundException($"UserFollower with ID {id} not found.");

        userFollower.FollowerId = followerId;
        userFollower.FollowedId = followedId;

        await _userFollowerRepository.UpdateAsync(userFollower);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        await _userFollowerRepository.DeleteAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserFollowerDto>> GetFollowersByUserIdAsync(Guid userId)
    {
        var followers = await _userFollowerRepository.GetFollowersByUserIdAsync(userId);
        return followers.Select(MapToUserFollowerDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserFollowerDto>> GetFollowedByUserIdAsync(Guid userId)
    {
        var followedUsers = await _userFollowerRepository.GetFollowedByUserIdAsync(userId);
        return followedUsers.Select(MapToUserFollowerDto);
    }

    /// <summary>
    /// Maps a UserFollower entity to a UserFollowerDTO.
    /// </summary>
    /// <param name="userFollower">The UserFollower entity to map.</param>
    /// <returns>The mapped UserFollowerDTO.</returns>
    private static UserFollowerDto MapToUserFollowerDto(UserFollower userFollower)
    {
        return new UserFollowerDto
        {
            Id = userFollower.Id,
            FollowerId = userFollower.FollowerId,
            FollowerName = userFollower.Follower.Username,
            FollowedId = userFollower.FollowedId,
            FollowedName = userFollower.Followed.Username
        };
    }
}