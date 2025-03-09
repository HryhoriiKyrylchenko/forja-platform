namespace Forja.Infrastructure.Validators;

/// <summary>
/// Provides validation methods for various domain models within the application.
/// </summary>
public class UserProfileModelValidator
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

        if (achievement.Id == Guid.Empty)
            return false; 

        if (achievement.GameId == Guid.Empty)
            return false; 

        if (string.IsNullOrWhiteSpace(achievement.Name))
            return false; 

        if (achievement.Name.Length > 50)
            return false; 

        if (achievement.Description.Length > 500)
            return false; 

        if (achievement.Points < 0)
            return false; 

        if (!string.IsNullOrWhiteSpace(achievement.LogoUrl) && !Uri.IsWellFormedUriString(achievement.LogoUrl, UriKind.Absolute))
            return false; 

        return true;
    }

    /// <summary>
    /// Validates a GameSave instance.
    /// </summary>
    /// <param name="gameSave">The GameSave object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateGameSave(GameSave gameSave)
    {
        if (gameSave == null)
            throw new ArgumentNullException(nameof(gameSave));
        
        if (gameSave.Id == Guid.Empty)
            return false;

        if (string.IsNullOrWhiteSpace(gameSave.Name))
            return false; 

        if (gameSave.Name.Length > 100)
            return false;
    
        if (gameSave.UserId == Guid.Empty)
            return false;
    
        if (gameSave.UserLibraryGameId == Guid.Empty)
            return false;
    
        if (string.IsNullOrWhiteSpace(gameSave.SaveFileUrl))
            return false;
        
        if (!Uri.IsWellFormedUriString(gameSave.SaveFileUrl, UriKind.Absolute))
            return false;
    
        if (gameSave.CreatedAt > DateTime.UtcNow)
            return false;
    
        return true;
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

        if (review.Id == Guid.Empty)
            return false; 

        if (review.UserId == Guid.Empty)
            return false; 

        if (review.ProductId == Guid.Empty)
            return false; 

        if (review.Comment.Length > 1000)
            return false; 

        if (review.CreatedAt > DateTime.UtcNow)
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
        if (!ValidateId(user.Id))
            return false;

        if (string.IsNullOrWhiteSpace(user.KeycloakUserId))
            return false; 

        if (string.IsNullOrWhiteSpace(user.Username))
            return false; 

        if (user.Username.Length > 30)
            return false; 

        if (string.IsNullOrWhiteSpace(user.Email) || !IsValidEmail(user.Email))
            return false;
        
        // Date validations
        if (user.CreatedAt > DateTime.UtcNow)
            return false; 

        if (user.ModifiedAt.HasValue && user.ModifiedAt.Value > DateTime.UtcNow)
            return false; 
        
        if (user.BirthDate.HasValue && user.BirthDate.Value > DateTime.UtcNow)
            return false;

        // Validate optional properties
        if (!string.IsNullOrWhiteSpace(user.PhoneNumber) && !IsValidPhoneNumber(user.PhoneNumber))
            return false;

        if (user.Firstname != null && user.Firstname.Length > 30)
            return false;
        
        if (user.Lastname != null && user.Lastname.Length > 30)
            return false;
        
        if (user.Gender != null && user.Gender.Length > 10)
            return false;
        
        if (user.Country != null && user.Country.Length > 30)
            return false;
        
        if (user.City != null && user.City.Length > 30)
            return false;

        if (user.SelfDescription != null && user.SelfDescription.Length > 500)
            return false;

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
    /// Validates a UserFollower instance.
    /// </summary>
    /// <param name="userFollower">The UserFollower object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserFollower(UserFollower userFollower)
    {
        if (userFollower == null)
            throw new ArgumentNullException(nameof(userFollower));
        
        if (userFollower.Id == Guid.Empty)
            return false;
        
        if (userFollower.FollowerId == Guid.Empty)
            return false;
        
        if (userFollower.FollowedId == Guid.Empty)
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
    /// Validates a UserWishList instance.
    /// </summary>
    /// <param name="userWishList">The UserWishList object to validate.</param>
    /// <returns>True if the UserWishList instance is valid, otherwise false.</returns>
    public static bool ValidateUserWishList(UserWishList userWishList)
    {
        if (userWishList == null) 
            throw new ArgumentNullException(nameof(userWishList));

        if (userWishList.Id == Guid.Empty)
            return false;
        
        if (userWishList.UserId == Guid.Empty)
            return false;
        
        if (userWishList.ProductId == Guid.Empty)
            return false;
        
        return true;
    }

    /// <summary>
    /// Validates if a phone number is in a valid format.
    /// </summary>
    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        var regex = @"^\+?[1-9]\d{1,14}$"; // Basic regex for phone numbers
        return Regex.IsMatch(phoneNumber, regex);
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

    /// <summary>
    /// Determines whether the provided email address is in a valid format.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>True if the email is valid, otherwise false.</returns>
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}