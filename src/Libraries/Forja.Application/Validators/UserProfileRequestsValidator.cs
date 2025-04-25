namespace Forja.Application.Validators;

public static class UserProfileRequestsValidator
{
    /// <summary>
    /// Validates a UserProfileDto instance.
    /// </summary>
    /// <param name="request">The UserCreateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserCreateRequest(UserCreateRequest request)
    {
        if (request == null) 
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.KeycloakUserId))
            return false; 

        if (!IsValidEmail(request.Email))
            return false; 
        
        if (request.CreatedAt > DateTime.UtcNow)
            return false;

        // Optional fields validation
        if (!string.IsNullOrWhiteSpace(request.Username) && request.Username.Length > 30)
            return false; 
        
        if (!string.IsNullOrWhiteSpace(request.Firstname) && request.Firstname.Length > 30)
            return false; 

        if (!string.IsNullOrWhiteSpace(request.Lastname) && request.Lastname.Length > 30)
            return false;

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber) && !IsValidPhoneNumber(request.PhoneNumber))
            return false; 

        if (!string.IsNullOrWhiteSpace(request.AvatarUrl) && !Uri.IsWellFormedUriString(request.AvatarUrl, UriKind.Absolute))
            return false; 
        
        if (request.BirthDate != null && request.BirthDate > DateTime.UtcNow)
            return false;
        
        if (!string.IsNullOrWhiteSpace(request.Gender) && request.Gender.Length > 10)
            return false;
        
        if (!string.IsNullOrWhiteSpace(request.Country) && request.Country.Length > 30)
            return false;
        
        if (!string.IsNullOrWhiteSpace(request.City) && request.City.Length > 30)
            return false;
        
        if (!string.IsNullOrWhiteSpace(request.SelfDescription) && request.SelfDescription.Length > 500)
            return false;

        return true;
    }

    /// <summary>
    /// Validates a UserUpdateRequest instance.
    /// </summary>
    /// <param name="request">The UserUpdateRequest object to validate.</param>
    /// <returns>True if the request is valid, otherwise false.</returns>
    public static bool ValidateUserUpdateRequest(UserUpdateRequest request)
    {
        if (request == null) 
            throw new ArgumentNullException(nameof(request));
        
        if (request.Id == Guid.Empty)
            return false;

        if (string.IsNullOrWhiteSpace(request.KeycloakUserId))
            return false; 

        if (!IsValidEmail(request.Email))
            return false; 
        
        if (request.ModifiedAt > DateTime.UtcNow)
            return false;

        if (request.ProfileHatVariant is < 1 or > 5)
        {
            return false;
        }

        // Optional fields validation
        if (!string.IsNullOrWhiteSpace(request.Username) && request.Username.Length > 30)
            return false; 
        
        if (!string.IsNullOrWhiteSpace(request.Firstname) && request.Firstname.Length > 30)
            return false; 

        if (!string.IsNullOrWhiteSpace(request.Lastname) && request.Lastname.Length > 30)
            return false;

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber) && !IsValidPhoneNumber(request.PhoneNumber))
            return false; 
        
        if (request.BirthDate != null && request.BirthDate > DateTime.UtcNow)
            return false;
        
        if (!string.IsNullOrWhiteSpace(request.Gender) && request.Gender.Length > 10)
            return false;
        
        if (!string.IsNullOrWhiteSpace(request.Country) && request.Country.Length > 30)
            return false;
        
        if (!string.IsNullOrWhiteSpace(request.City) && request.City.Length > 30)
            return false;
        
        if (!string.IsNullOrWhiteSpace(request.SelfDescription) && request.SelfDescription.Length > 500)
            return false;

        return true;
    }

    /// <summary>
    /// Validates a UserUpdateProfileHatVariantRequest instance.
    /// </summary>
    /// <param name="request">The UserUpdateProfileHatVariantRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserUpdateProfileHatVariantRequest(UserUpdateProfileHatVariantRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        
        if (request.UserId == Guid.Empty)
            return false;
        
        if (request.Variant is < 1 or > 5)
            return false;
        
        return true;
    }

    /// <summary>
    /// Validates a UserUpdateAvatarRequest instance.
    /// </summary>
    /// <param name="request">The UserUpdateAvatarRequest object to validate.</param>
    /// <returns>True if the request is valid, otherwise false.</returns>
    public static bool ValidateUserUpdateAvatarRequest(UserUpdateAvatarRequest request)
    {
        if (request == null) 
            throw new ArgumentNullException(nameof(request));
        
        if (request.Id == Guid.Empty)
            return false;
        
        if (string.IsNullOrWhiteSpace(request.AvatarUrl))
            return false;

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
        var regex = @"^\+?[1-9]\d{1,14}$"; 
        return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, regex);
    }

    /// <summary>
    /// Validates a GameSaveCreateRequest instance.
    /// </summary>
    /// <param name="request">The GameSaveCreateRequest object containing the game save details to validate.</param>
    /// <returns>True if the request is valid, otherwise false.</returns>
    public static bool ValidateGameSaveCreateRequest(GameSaveCreateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.Name))
            return false;

        if (request.UserId == Guid.Empty)
            return false;
        
        if (request.UserLibraryGameId == Guid.Empty)
            return false;
        
        if (string.IsNullOrWhiteSpace(request.SaveFileUrl))
            return false;
        
        if (request.CreatedAt > DateTime.UtcNow)
            return false;
        
        return true;
    }

    /// <summary>
    /// Validates a GameSaveUpdateRequest instance.
    /// </summary>
    /// <param name="request">The GameSaveUpdateRequest object to validate.</param>
    /// <returns>True if the request is valid, otherwise false.</returns>
    public static bool ValidateGameSaveUpdateRequest(GameSaveUpdateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        
        if (request.Id == Guid.Empty)
            return false;

        if (string.IsNullOrWhiteSpace(request.Name))
            return false;

        if (request.UserId == Guid.Empty)
            return false;
        
        if (request.UserLibraryGameId == Guid.Empty)
            return false;
        
        if (string.IsNullOrWhiteSpace(request.SaveFileUrl))
            return false;
        
        if (request.LastUpdatedAt > DateTime.UtcNow)
            return false;
        
        return true;
    }

    /// <summary>
    /// Validates a ReviewCreateRequest instance.
    /// </summary>
    /// <param name="request">The ReviewCreateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateReviewCreateRequest(ReviewCreateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.UserId == Guid.Empty)
            return false;
        
        if (request.ProductId == Guid.Empty)
            return false;
        
        if (request.CreatedAt > DateTime.UtcNow)
            return false;
        
        return true;
    }

    /// <summary>
    /// Validates a ReviewUpdateRequest instance.
    /// </summary>
    /// <param name="request">The ReviewUpdateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateReviewUpdateRequest(ReviewUpdateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Id == Guid.Empty)
            return false;
        
        if (request.UserId == Guid.Empty)
            return false;
        
        if (request.ProductId == Guid.Empty)
            return false;
        
        return true;
    }

    /// <summary>
    /// Validates a ReviewApproveRequest instance.
    /// </summary>
    /// <param name="request">The ReviewApproveRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateReviewApproveRequest(ReviewApproveRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Id == Guid.Empty)
            return false;
        
        return true;
    }

    /// <summary>
    /// Validates an AchievementCreateRequest instance.
    /// </summary>
    /// <param name="request">The AchievementCreateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateAchievementCreateRequest(AchievementCreateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.Name))
            return false; 

        if (request.Name.Length > 50)
            return false; 

        if (request.Description.Length > 500)
            return false; 

        if (request.Points < 0)
            return false; 
        
        return true;
    }
    
    /// <summary>
    /// Validates an AchievementUpdateRequest instance.
    /// </summary>
    /// <param name="request">The AchievementUpdateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateAchievementUpdateRequest(AchievementUpdateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Id == Guid.Empty)
            return false;
        
        if (string.IsNullOrWhiteSpace(request.Name))
            return false; 

        if (request.Name.Length > 50)
            return false; 

        if (request.Description.Length > 500)
            return false; 

        if (request.Points < 0)
            return false; 
        
        return true;
    }
    
    /// <summary>
    /// Validates a UserAchievementCreateRequest instance.
    /// </summary>
    /// <param name="request">The UserAchievementCreateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserAchievementCreateRequest(UserAchievementCreateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.UserId == Guid.Empty)
            return false;

        if (request.AchievementId == Guid.Empty)
            return false;

        if (request.AchievedAt > DateTime.UtcNow)
            return false;

        return true;
    }
    
    /// <summary>
    /// Validates a UserAchievementUpdateRequest instance.
    /// </summary>
    /// <param name="request">The UserAchievementUpdateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserAchievementUpdateRequest(UserAchievementUpdateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Id == Guid.Empty)
            return false;

        if (request.UserId == Guid.Empty)
            return false;

        if (request.AchievementId == Guid.Empty)
            return false;

        if (request.AchievedAt > DateTime.UtcNow)
            return false;

        return true;
    }

    /// <summary>
    /// Validates a UserFollowerCreateRequest instance.
    /// </summary>
    /// <param name="request">The UserFollowerCreateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserFollowerCreateRequest(UserFollowerCreateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.FollowerId == Guid.Empty)
            return false;

        if (request.FollowedId == Guid.Empty)
            return false;
        
        return true;
    }

    /// <summary>
    /// Validates a UserFollowerUpdateRequest instance.
    /// </summary>
    /// <param name="request">The UserFollowerUpdateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserFollowerUpdateRequest(UserFollowerUpdateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        
        if (request.Id == Guid.Empty)
            return false;

        if (request.FollowerId == Guid.Empty)
            return false;

        if (request.FollowedId == Guid.Empty)
            return false;

        return true;
    }
    
    /// <summary>
    /// Validates a UserLibraryGameDto instance.
    /// </summary>
    /// <param name="request">The UserLibraryGameCreateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserLibraryGameCreateRequest(UserLibraryGameCreateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.UserId == Guid.Empty)
            return false;

        if (request.GameId == Guid.Empty)
            return false; 

        return true;
    }

    /// <summary>
    /// Validates a UserLibraryGameUpdateRequest instance.
    /// </summary>
    /// <param name="request">The UserLibraryGameUpdateRequest object to validate.</param>
    /// <returns>True if the request is valid, otherwise false.</returns>
    public static bool ValidateUserLibraryGameUpdateRequest(UserLibraryGameUpdateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Id == Guid.Empty)
            return false;

        return true;
    }

    /// <summary>
    /// Validates a UserLibraryAddonCreateRequest instance.
    /// </summary>
    /// <param name="request">The UserLibraryAddonCreateRequest object to validate.</param>
    /// <returns>True if the request is valid, otherwise false.</returns>
    public static bool ValidateUserLibraryAddonCreateRequest(UserLibraryAddonCreateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.UserLibraryGameId == Guid.Empty)
            return false;

        if (request.AddonId == Guid.Empty)
            return false; 

        return true;
    }

    /// <summary>
    /// Validates a UserLibraryAddonUpdateRequest instance.
    /// </summary>
    /// <param name="request">The UserLibraryAddonUpdateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserLibraryAddonUpdateRequest(UserLibraryAddonUpdateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Id == Guid.Empty)
            return false;
        
        if (request.UserLibraryGameId == Guid.Empty)
            return false;

        if (request.AddonId == Guid.Empty)
            return false; 
        
        if (request.PurchaseDate > DateTime.UtcNow)
            return false;

        return true;
    }

    /// <summary>
    /// Validates a UserWishListCreateRequest instance.
    /// </summary>
    /// <param name="request">The UserWishListCreateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserWishListCreateRequest(UserWishListCreateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        
        if (request.UserId == Guid.Empty)
            return false;
        
        if (request.ProductId == Guid.Empty)
            return false;
        
        return true;
    }

    /// <summary>
    /// Validates a UserWishListUpdateRequest instance.
    /// </summary>
    /// <param name="request">The UserWishListUpdateRequest object to validate.</param>
    /// <returns>True if valid, otherwise false.</returns>
    public static bool ValidateUserWishListUpdateRequest(UserWishListUpdateRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        
        if (request.Id == Guid.Empty)
            return false;
        
        if (request.UserId == Guid.Empty)
            return false;
        
        if (request.ProductId == Guid.Empty)
            return false;
        
        return true;
    }
}