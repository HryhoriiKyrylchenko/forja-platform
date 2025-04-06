namespace Forja.Infrastructure.Validators;

/// <summary>
/// Provides validation methods for various domain models within the application.
/// </summary>
public class UserProfileModelValidator
{

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

        return true;
    }

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
    
        if (gameSave.CreatedAt > DateTime.UtcNow)
            return false;
    
        return true;
    }
    
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
    
    public static bool ValidateUser(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

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
        
        if (user.CreatedAt > DateTime.UtcNow)
            return false; 

        if (user.ModifiedAt.HasValue && user.ModifiedAt.Value > DateTime.UtcNow)
            return false; 
        
        if (user.BirthDate.HasValue && user.BirthDate.Value > DateTime.UtcNow)
            return false;

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
    
    public static bool ValidateUserAchievement(UserAchievement userAchievement)
    {
        if (userAchievement == null) 
            throw new ArgumentNullException(nameof(userAchievement));

        if (userAchievement.Id == Guid.Empty)
            return false; 

        if (userAchievement.UserId == Guid.Empty)
            return false;

        if (userAchievement.AchievementId == Guid.Empty)
            return false; 

        if (userAchievement.AchievedAt > DateTime.UtcNow)
            return false; 

        return true;
    }

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
    
    public static bool ValidateUserLibraryAddon(UserLibraryAddon userLibraryAddon)
    {
        if (userLibraryAddon == null) 
            throw new ArgumentNullException(nameof(userLibraryAddon));

        if (userLibraryAddon.Id == Guid.Empty)
            return false;

        if (userLibraryAddon.UserLibraryGameId == Guid.Empty)
            return false;

        if (userLibraryAddon.AddonId == Guid.Empty)
            return false;

        if (userLibraryAddon.PurchaseDate > DateTime.UtcNow)
            return false;

        return true;
    }

    public static bool ValidateUserLibraryGame(UserLibraryGame userLibraryGame)
    {
        if (userLibraryGame == null) 
            throw new ArgumentNullException(nameof(userLibraryGame));

        if (userLibraryGame.Id == Guid.Empty)
            return false;

        if (userLibraryGame.UserId == Guid.Empty)
            return false;

        if (userLibraryGame.GameId == Guid.Empty)
            return false;

        if (userLibraryGame.PurchaseDate > DateTime.UtcNow)
            return false;

        return true;
    }

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

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        var regex = @"^\+?[1-9]\d{1,14}$";
        return Regex.IsMatch(phoneNumber, regex);
    }
    
    public static bool ValidateId(Guid id)
    {
        return id != Guid.Empty;
    }

    public static bool ValidateDate(DateTime date)
    {
        return date <= DateTime.UtcNow;
    }

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