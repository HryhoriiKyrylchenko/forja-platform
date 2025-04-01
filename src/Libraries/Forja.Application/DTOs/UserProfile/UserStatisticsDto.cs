namespace Forja.Application.DTOs.UserProfile
{
    /// <summary>
    /// Represents statistics for a user's profile.
    /// </summary>
    /// <remarks>
    /// This DTO contains various statistics related to the user's game library, wishlist, and followers.
    /// It is typically used to display an overview of the user's activity in the system.
    /// </remarks>
    public class UserStatisticsDto
    {
        /// <summary>
        /// Gets or sets the number of games owned by the user.
        /// </summary>
        /// <remarks>
        /// This property indicates the total number of games the user has purchased or added to their library.
        /// It includes both free and paid games.
        /// </remarks>
        public int GamesOwned { get; set; } = -1;

        /// <summary>
        /// Gets or sets the number of DLCs owned by the user.
        /// </summary>
        /// <remarks>
        /// This property represents the total number of downloadable content (DLCs) the user has in their library.
        /// DLCs are additional content packages that enhance or extend the base game.
        /// </remarks>
        public int DlcOwned { get; set; } = -1;

        /// <summary>
        /// Gets or sets the number of items in the user's wishlist.
        /// </summary>
        /// <remarks>
        /// This property shows the total count of games, add-ons, or other content
        /// that the user has added to their wishlist for future purchase or consideration.
        /// </remarks>
        public int Whishlisted { get; set; } = -1;

        /// <summary>
        /// Gets or sets the number of users the current user follows.
        /// </summary>
        /// <remarks>
        /// This property reflects the total number of other users that the current user is following.
        /// Following other users may provide updates on their activity or content they share.
        /// </remarks>
        public int Follows { get; set; } = -1;
    }
}
