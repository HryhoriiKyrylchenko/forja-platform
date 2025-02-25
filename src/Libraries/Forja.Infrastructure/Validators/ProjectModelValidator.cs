namespace Forja.Infrastructure.Validators;

/// <summary>
/// Provides validation methods for various domain models within the application.
/// </summary>
public class ProjectModelValidator
{
    /// <summary>
    /// Validates an Achievement instance.
    /// </summary>
    /// <param name="achievement">The Achievement object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateAchievement(Achievement achievement)
    {
        if (achievement == null) 
            throw new ArgumentNullException(nameof(achievement));

        // Check mandatory fields
        if (achievement.Id == Guid.Empty)
            return false; // Id cannot be empty

        if (achievement.GameId == Guid.Empty)
            return false; // GameId cannot be empty

        if (string.IsNullOrWhiteSpace(achievement.Name))
            return false; // Name is required and cannot be empty or whitespace

        if (achievement.Name.Length > 50)
            return false; // Name cannot exceed max length of 50

        if (achievement.Description.Length > 500)
            return false; // Description cannot exceed max length of 500

        if (achievement.Points < 0)
            return false; // Points should not be negative

        if (!string.IsNullOrWhiteSpace(achievement.LogoUrl) && !Uri.IsWellFormedUriString(achievement.LogoUrl, UriKind.Absolute))
            return false; // If LogoUrl is provided, it must be a valid URL

        return true;
    }

    /// <summary>
    /// Validates a UserLibraryGame instance.
    /// </summary>
    /// <param name="userLibraryGame">The instance of UserLibraryGame to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserLibraryGame(UserLibraryGame userLibraryGame)
    {
        if (userLibraryGame == null) 
            throw new ArgumentNullException(nameof(userLibraryGame));

        // Check mandatory fields
        if (userLibraryGame.Id == Guid.Empty)
            return false;

        if (userLibraryGame.UserId == Guid.Empty)
            return false;

        if (userLibraryGame.GameId == Guid.Empty)
            return false;

        // Ensure the purchase date is not in the future
        if (userLibraryGame.PurchaseDate > DateTime.UtcNow)
            return false;

        return true;
    }

    /// <summary>
    /// Validates a UserLibraryAddon instance.
    /// </summary>
    /// <param name="userLibraryAddon">The instance of UserLibraryAddon to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserLibraryAddon(UserLibraryAddon userLibraryAddon)
    {
        if (userLibraryAddon == null) 
            throw new ArgumentNullException(nameof(userLibraryAddon));

        // Check mandatory fields for UserLibraryAddon
        if (userLibraryAddon.Id == Guid.Empty)
            return false;

        if (userLibraryAddon.UserLibraryGameId == Guid.Empty)
            return false;

        if (userLibraryAddon.AddonId == Guid.Empty)
            return false;

        // Ensure other rules specific to the addon are met
        if (userLibraryAddon.PurchaseDate > DateTime.UtcNow)
            return false;

        return true;
    }
    
    /// <summary>
    /// Validates a User instance.
    /// </summary>
    /// <param name="user">The User object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUser(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        // Check mandatory fields for User
        if (user.Id == Guid.Empty)
            return false; // Id cannot be empty

        if (string.IsNullOrWhiteSpace(user.KeycloakUserId))
            return false; // KeycloakUserId cannot be empty or whitespace

        if (string.IsNullOrWhiteSpace(user.Username))
            return false; // Username cannot be empty or whitespace

        if (user.Username.Length > 30)
            return false; // Username cannot exceed 30 characters

        if (string.IsNullOrWhiteSpace(user.Email))
            return false; // Email cannot be empty or whitespace

        // Date validations
        if (user.CreatedAt > DateTime.UtcNow)
            return false; // CreatedAt cannot be in the future

        if (user.ModifiedAt.HasValue && user.ModifiedAt.Value > DateTime.UtcNow)
            return false; // ModifiedAt cannot be in the future

        return true;
    }

    /// <summary>
    /// Validates if a phone number is in a valid format.
    /// </summary>
    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        var regex = @"^\+?[1-9]\d{1,14}$"; // Basic regex for phone numbers
        return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, regex);
    }
    
    /// <summary>
    /// Validates a Review instance.
    /// </summary>
    /// <param name="review">The Review object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateReview(Review review)
    {
        if (review == null) 
            throw new ArgumentNullException(nameof(review));

        // Check mandatory fields for Review
        if (review.Id == Guid.Empty)
            return false; // Id cannot be empty

        if (review.UserId == Guid.Empty)
            return false; // UserId cannot be empty

        if (review.GameId == Guid.Empty)
            return false; // GameId cannot be empty

        if (review.Comment.Length > 1000)
            return false; // Text cannot exceed 1000 characters

        if (review.Rating < 1 || review.Rating > 5)
            return false; // Rating must be between 1 and 5

        // Date validations
        if (review.CreatedAt > DateTime.UtcNow)
            return false; // CreatedAt cannot be in the future

        return true;
    }
    
    /// <summary>
    /// Validates a UserAchievement instance.
    /// </summary>
    /// <param name="userAchievement">The UserAchievement object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserAchievement(UserAchievement userAchievement)
    {
        if (userAchievement == null) 
            throw new ArgumentNullException(nameof(userAchievement));

        // Check mandatory fields for UserAchievement
        if (userAchievement.Id == Guid.Empty)
            return false; // Id cannot be empty

        if (userAchievement.UserId == Guid.Empty)
            return false; // UserId cannot be empty

        if (userAchievement.AchievementId == Guid.Empty)
            return false; // AchievementId cannot be empty

        // Date validation for AchievedAt
        if (userAchievement.AchievedAt > DateTime.UtcNow)
            return false; // AchievedAt cannot be in the future

        return true;
    }

    /// <summary>
    /// Generic validation method to check for null or invalid IDs.
    /// </summary>
    /// <param name="id">The ID to validate.</param>
    /// <returns>True if the ID is valid, otherwise false.</returns>
    public static bool ValidateId(Guid id)
    {
        return id != Guid.Empty;
    }

    /// <summary>
    /// Validates that a given date is not in the future.
    /// </summary>
    /// <param name="date">The date to validate.</param>
    /// <returns>True if the date is valid, otherwise false.</returns>
    public static bool ValidateDate(DateTime date)
    {
        return date <= DateTime.UtcNow;
    }
}