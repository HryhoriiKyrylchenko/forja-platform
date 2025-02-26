namespace Forja.Application.Validators;

public class ApplicationDtoValidator
{
    /// <summary>
    /// Validates a UserProfileDto instance.
    /// </summary>
    /// <param name="userProfileDto">The UserProfileDto object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserProfileDto(UserProfileDto userProfileDto)
    {
        if (userProfileDto == null) 
            throw new ArgumentNullException(nameof(userProfileDto));

        // Check mandatory fields for UserProfileDto
        if (userProfileDto.Id == Guid.Empty)
            return false; // Id cannot be empty

        if (string.IsNullOrWhiteSpace(userProfileDto.Username))
            return false; // Username cannot be empty or whitespace

        if (userProfileDto.Username.Length > 30)
            return false; // Username cannot exceed 30 characters

        if (string.IsNullOrWhiteSpace(userProfileDto.Email))
            return false; // Email cannot be empty or whitespace

        if (!IsValidEmail(userProfileDto.Email))
            return false; // Email must be in a valid format

        // Optional fields validation
        if (!string.IsNullOrWhiteSpace(userProfileDto.Firstname) && userProfileDto.Firstname.Length > 30)
            return false; // Firstname cannot exceed 30 characters

        if (!string.IsNullOrWhiteSpace(userProfileDto.Lastname) && userProfileDto.Lastname.Length > 30)
            return false; // Lastname cannot exceed 30 characters

        if (!string.IsNullOrWhiteSpace(userProfileDto.PhoneNumber) && !IsValidPhoneNumber(userProfileDto.PhoneNumber))
            return false; // If PhoneNumber is provided, it must be valid

        if (!string.IsNullOrWhiteSpace(userProfileDto.AvatarUrl) && !Uri.IsWellFormedUriString(userProfileDto.AvatarUrl, UriKind.Absolute))
            return false; // If AvatarUrl is provided, it must be a well-formed URL

        return true;
    }

    /// <summary>
    /// Validates if an email address is in a valid format.
    /// </summary>
    private static bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
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
    /// Validates an AchievementDto instance.
    /// </summary>
    /// <param name="achievementDto">The AchievementDto object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateAchievementDto(AchievementDto achievementDto)
    {
        if (achievementDto == null)
            throw new ArgumentNullException(nameof(achievementDto));

        // Check mandatory fields for AchievementDto
        if (achievementDto.Id == Guid.Empty)
            return false; // Id cannot be empty

        if (string.IsNullOrWhiteSpace(achievementDto.Name))
            return false; // Name cannot be empty or whitespace

        if (achievementDto.Name.Length > 50)
            return false; // Name cannot exceed 50 characters

        if (achievementDto.Description.Length > 500)
            return false; // Description cannot exceed 500 characters

        if (achievementDto.Points < 0)
            return false; // Points cannot be negative
        
        return true;
    }
    
    /// <summary>
    /// Validates a UserAchievementDto instance.
    /// </summary>
    /// <param name="userAchievementDto">The UserAchievementDto object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserAchievementDto(UserAchievementDto userAchievementDto)
    {
        if (userAchievementDto == null)
            throw new ArgumentNullException(nameof(userAchievementDto));

        // Check mandatory fields for UserAchievementDto
        if (userAchievementDto.Id == Guid.Empty)
            return false; // Id cannot be empty

        if (userAchievementDto.User.Id == Guid.Empty)
            return false; // User must be provided

        if (userAchievementDto.Achievement.Id == Guid.Empty)
            return false; // Achievement must be provided

        // Date validation for AchievedAt
        if (userAchievementDto.AchievedAt > DateTime.UtcNow)
            return false; // AchievedAt cannot be in the future

        return true;
    }
    
    /// <summary>
    /// Validates a UserLibraryGameDto instance.
    /// </summary>
    /// <param name="userLibraryGameDto">The UserLibraryGameDto object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserLibraryGameDto(UserLibraryGameDto userLibraryGameDto)
    {
        if (userLibraryGameDto == null)
            throw new ArgumentNullException(nameof(userLibraryGameDto));

        // Check mandatory fields for UserLibraryGameDto
        if (userLibraryGameDto.Id == Guid.Empty)
            return false; // Id cannot be empty

        if (userLibraryGameDto.User.Id == Guid.Empty)
            return false; // User must be provided and cannot be null

        if (userLibraryGameDto.Game.Id == Guid.Empty)
            return false; // Game must be provided and cannot be null

        // Date validation for PurchaseDate
        if (userLibraryGameDto.PurchaseDate > DateTime.UtcNow)
            return false; // PurchaseDate cannot be in the future

        return true;
    }
    
    /// <summary>
    /// Validates a UserLibraryAddonDto instance.
    /// </summary>
    /// <param name="userLibraryAddonDto">The UserLibraryAddonDto object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserLibraryAddonDto(UserLibraryAddonDto userLibraryAddonDto)
    {
        if (userLibraryAddonDto == null)
            throw new ArgumentNullException(nameof(userLibraryAddonDto));

        // Check mandatory fields for UserLibraryAddonDto
        if (userLibraryAddonDto.Id == Guid.Empty)
            return false; // Id cannot be empty

        if (userLibraryAddonDto.UserLibraryGame.Id == Guid.Empty)
            return false; // UserLibraryGame must be provided and cannot be null

        if (userLibraryAddonDto.GameAddon.Id == Guid.Empty)
            return false; // GameAddon must be provided and cannot be null

        // Date validation for PurchaseDate
        if (userLibraryAddonDto.PurchaseDate > DateTime.UtcNow)
            return false; // PurchaseDate cannot be in the future

        return true;
    }
}