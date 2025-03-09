namespace Forja.Application.Mapping;

/// <summary>
/// Provides functionality to map user profile entity objects to their corresponding Data Transfer Objects (DTOs).
/// </summary>
/// <remarks>
/// This class contains static methods designed to transform user profile entity data into
/// a format suitable for external communication through DTOs, facilitating the separation
/// of business logic and presentation layers.
/// </remarks>
public static class UserProfileEntityToDtoMapper
{
    /// <summary>
    /// Maps an <see cref="Achievement"/> entity to an <see cref="AchievementDto"/>.
    /// </summary>
    /// <param name="achievement">The <see cref="Achievement"/> entity to be mapped.</param>
    /// <returns>An <see cref="AchievementDto"/> representing the mapped data.</returns>
    public static AchievementDto MapToAchievementDto(Achievement achievement)
    {
        return new AchievementDto
        {
            Id = achievement.Id,
            Name = achievement.Name,
            Description = achievement.Description,
            Points = achievement.Points,
            LogoUrl = achievement.LogoUrl,
            Game = GamesEntityToDtoMapper.MapToGameDto(achievement.Game)
        };
    }

    /// <summary>
    /// Maps a <see cref="UserAchievement"/> entity to a <see cref="UserAchievementDto"/>.
    /// </summary>
    /// <param name="userAchievement">The <see cref="UserAchievement"/> entity to be mapped.</param>
    /// <returns>A <see cref="UserAchievementDto"/> representing the mapped data.</returns>
    public static UserAchievementDto MapToUserAchievementDto(UserAchievement userAchievement)
    {
        return new UserAchievementDto
        {
            Id = userAchievement.Id,
            User = MapToUserProfileDto(userAchievement.User),
            Achievement = MapToAchievementDto(userAchievement.Achievement),
            AchievedAt = userAchievement.AchievedAt
        };
    }
    
    /// <summary>
    /// Maps a <see cref="GameSave"/> entity to a <see cref="GameSaveDto"/>.
    /// </summary>
    /// <param name="gameSave">The <see cref="GameSave"/> entity to be mapped.</param>
    /// <returns>A <see cref="GameSaveDto"/> representing the mapped data.</returns>
    public static GameSaveDto MapToGameSaveDto(GameSave gameSave)
    {
        return new GameSaveDto
        {
            Id = gameSave.Id,
            Name = gameSave.Name,
            SaveFileUrl = gameSave.SaveFileUrl,
            CreatedAt = gameSave.CreatedAt,
            LastUpdatedAt = gameSave.LastUpdatedAt,
            UserId = gameSave.UserId,
            UserLibraryGameId = gameSave.UserLibraryGameId
        };
    }
    
    /// <summary>
    /// Converts a <see cref="Review"/> entity to a <see cref="ReviewDto"/> object.
    /// </summary>
    /// <param name="review">The review entity to be mapped.</param>
    /// <returns>A <see cref="ReviewDto"/> object containing the mapped data from the provided review entity.</returns>
    public static ReviewDto MapToReviewDto(Review review)
    {
        return new ReviewDto
        {
            Id = review.Id,
            UserId = review.UserId,
            ProductId = review.ProductId,
            PositiveRating = review.PositiveRating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };
    }
    
    /// <summary>
    /// Maps a UserFollower entity to a UserFollowerDTO.
    /// </summary>
    /// <param name="userFollower">The UserFollower entity to map.</param>
    /// <returns>The mapped UserFollowerDTO.</returns>
    public static UserFollowerDto MapToUserFollowerDto(UserFollower userFollower)
    {
        return new UserFollowerDto
        {
            Id = userFollower.Id,
            FollowerId = userFollower.FollowerId,
            FollowerUsername = userFollower.Follower.Username,
            FollowerTag = userFollower.Follower.CustomUrl,
            FollowedId = userFollower.FollowedId,
            FollowedUsername = userFollower.Followed.Username,
            FollowedTag = userFollower.Followed.CustomUrl
        };
    }
    
    /// <summary>
    /// Maps a <see cref="UserLibraryGame"/> entity to a <see cref="UserLibraryGameDto"/>.
    /// </summary>
    /// <param name="userLibraryGame">The user library game entity to be mapped.</param>
    /// <returns>A <see cref="UserLibraryGameDto"/> representation of the provided <see cref="UserLibraryGame"/> entity.</returns>
    public static UserLibraryGameDto MapToUserLibraryGameDto(UserLibraryGame userLibraryGame)
    {
        return new UserLibraryGameDto
        {
            Id = userLibraryGame.Id,
            UserId = userLibraryGame.UserId,
            Game = GamesEntityToDtoMapper.MapToGameDto(userLibraryGame.Game),
            TimePlayed = userLibraryGame.TimePlayed,
            PurchaseDate = userLibraryGame.PurchaseDate
        };
    }

    /// <summary>
    /// Maps a <see cref="UserLibraryAddon"/> entity to a <see cref="UserLibraryAddonDto"/> DTO.
    /// </summary>
    /// <param name="userLibraryAddon">The <see cref="UserLibraryAddon"/> entity to map.</param>
    /// <returns>A <see cref="UserLibraryAddonDto"/> that represents the mapped data.</returns>
    public static UserLibraryAddonDto MapToUserLibraryAddonDto(UserLibraryAddon userLibraryAddon)
    {
        return new UserLibraryAddonDto
        {
            Id = userLibraryAddon.Id,
            UserLibraryGameId = userLibraryAddon.UserLibraryGameId,
            GameAddon = GamesEntityToDtoMapper.MapGameAddonDto(userLibraryAddon.GameAddon),
            PurchaseDate = userLibraryAddon.PurchaseDate
        };
    }
    
    /// <summary>
    /// Maps a <see cref="User"/> domain entity to a <see cref="UserProfileDto"/> data transfer object.
    /// </summary>
    /// <param name="user">The <see cref="User"/> instance to map.</param>
    /// <returns>A <see cref="UserProfileDto"/> that represents the mapped user information.</returns>
    public static UserProfileDto MapToUserProfileDto(User user)
    {
        return new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = user.AvatarUrl,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            Country = user.Country,
            City = user.City,
            SelfDescription = user.SelfDescription,
            ShowPersonalInfo = user.ShowPersonalInfo,
            CreatedAt = user.CreatedAt,
            CustomUrl = user.CustomUrl
        };
    }
    
    /// <summary>
    /// Maps a UserWishList entity to a UserWishListDTO.
    /// </summary>
    /// <param name="userWishList">The UserWishList entity to map.</param>
    /// <returns>The mapped UserWishListDTO.</returns>
    public static UserWishListDto MapToUserWishListDto(UserWishList userWishList)
    {
        return new UserWishListDto
        {
            Id = userWishList.Id,
            UserId = userWishList.UserId,
            UserName = userWishList.User.Username,
            ProductId = userWishList.ProductId,
            ProductName = userWishList.Product.Title
        };
    }
}