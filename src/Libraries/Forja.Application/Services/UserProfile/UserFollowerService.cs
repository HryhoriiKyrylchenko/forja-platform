namespace Forja.Application.Services.UserProfile;

/// <summary>
/// Service class for UserFollower functionality.
/// Provides logic and orchestration for managing UserFollower entities.
/// </summary>
public class UserFollowerService : IUserFollowerService
{
    private readonly IUserFollowerRepository _userFollowerRepository;
    private readonly IFileManagerService _fileManagerService;

    public UserFollowerService(IUserFollowerRepository userFollowerRepository,
        IFileManagerService fileManagerService)
    {
        _userFollowerRepository = userFollowerRepository;
        _fileManagerService = fileManagerService;
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
        
        var result = new List<UserFollowerDto>();

        foreach (var userFollower in userFollowers)
        {
            string followerAvatarUrl = userFollower.Follower.AvatarUrl != null
                ? await _fileManagerService.GetPresignedUrlAsync(userFollower.Follower.AvatarUrl, 1900)
                : string.Empty;
            string followedAvatarUrl = userFollower.Followed.AvatarUrl != null
                ? await _fileManagerService.GetPresignedUrlAsync(userFollower.Followed.AvatarUrl, 1900)
                : string.Empty;
            
            result.Add(UserProfileEntityToDtoMapper.MapToUserFollowerDto(userFollower, followerAvatarUrl, followedAvatarUrl));
        }

        return result;
    }
    
    /// <inheritdoc />
    public async Task<UserFollowerDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }
        
        var userFollower = await _userFollowerRepository.GetByIdAsync(id);
        
        return userFollower == null ? null : UserProfileEntityToDtoMapper.MapToUserFollowerDto(
            userFollower,
            userFollower.Follower.AvatarUrl != null
                ? await _fileManagerService.GetPresignedUrlAsync(userFollower.Follower.AvatarUrl, 1900)
                : string.Empty,
            userFollower.Followed.AvatarUrl != null
                ? await _fileManagerService.GetPresignedUrlAsync(userFollower.Followed.AvatarUrl, 1900)
                : string.Empty
            );
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
        return createdUserFollower == null ? null : UserProfileEntityToDtoMapper.MapToUserFollowerDto(
            createdUserFollower,
            createdUserFollower.Follower.AvatarUrl != null
                ? await _fileManagerService.GetPresignedUrlAsync(createdUserFollower.Follower.AvatarUrl, 1900)
                : string.Empty,
            createdUserFollower.Followed.AvatarUrl != null
                ? await _fileManagerService.GetPresignedUrlAsync(createdUserFollower.Followed.AvatarUrl, 1900)
                : string.Empty
        );
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
    public async Task<List<UserFollowerDto>> GetFollowersByUserIdAsync(Guid userId) 
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(userId));
        }
        
        var followers = await _userFollowerRepository.GetFollowersByUserIdAsync(userId);
        
        var result = new List<UserFollowerDto>();

        foreach (var userFollower in followers)
        {
            string followerAvatarUrl = userFollower.Follower.AvatarUrl != null
                ? await _fileManagerService.GetPresignedUrlAsync(userFollower.Follower.AvatarUrl, 1900)
                : string.Empty;
            string followedAvatarUrl = userFollower.Followed.AvatarUrl != null
                ? await _fileManagerService.GetPresignedUrlAsync(userFollower.Followed.AvatarUrl, 1900)
                : string.Empty;
            
            result.Add(UserProfileEntityToDtoMapper.MapToUserFollowerDto(userFollower, followerAvatarUrl, followedAvatarUrl));
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<List<UserFollowerDto>> GetFollowedByUserIdAsync(Guid userId) 
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(userId));
        }
        
        var followedUsers = await _userFollowerRepository.GetFollowedByUserIdAsync(userId);
        var result = new List<UserFollowerDto>();

        foreach (var userFollower in followedUsers)
        {
            string followerAvatarUrl = userFollower.Follower.AvatarUrl != null
                ? await _fileManagerService.GetPresignedUrlAsync(userFollower.Follower.AvatarUrl, 1900)
                : string.Empty;
            string followedAvatarUrl = userFollower.Followed.AvatarUrl != null
                ? await _fileManagerService.GetPresignedUrlAsync(userFollower.Followed.AvatarUrl, 1900)
                : string.Empty;
            
            result.Add(UserProfileEntityToDtoMapper.MapToUserFollowerDto(userFollower, followerAvatarUrl, followedAvatarUrl));
        }

        return result;
    }

    #region User Statistics Methods
    /// <summary>
    ///  Retrieves the count of followers for a specified user and updates the provided statistics object.
    /// </summary>
    /// <param name="userId">Identifies the user for whom the follower count is being retrieved.</param>
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