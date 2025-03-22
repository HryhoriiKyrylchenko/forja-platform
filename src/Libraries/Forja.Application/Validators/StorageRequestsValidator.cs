namespace Forja.Application.Validators;

public static class StorageRequestsValidator
{
    public static bool ValidateGameFilesUploadRequest(GameFilesUploadRequest request, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(request.FolderPath))
        {
            errorMessage = "FolderPath cannot be null or empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public static bool ValidateGameFilesDownloadRequest(GameFilesDownloadRequest request, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(request.SourcePath))
        {
            errorMessage = "SourcePath cannot be null or empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.DestinationPath))
        {
            errorMessage = "DestinationPath cannot be null or empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public static bool ValidateGameFilesDeleteRequest(GameFilesDeleteRequest request, out string errorMessage)
    {

        if (string.IsNullOrWhiteSpace(request.SourcePath))
        {
            errorMessage = "SourcePath cannot be null or empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public static bool ValidateAddonFilesUploadRequest(AddonFilesUploadRequest request, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(request.FolderPath))
        {
            errorMessage = "FolderPath cannot be null or empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public static bool ValidateAddonFilesDownloadRequest(AddonFilesDownloadRequest request, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(request.SourcePath))
        {
            errorMessage = "SourcePath cannot be null or empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.DestinationPath))
        {
            errorMessage = "DestinationPath cannot be null or empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public static bool ValidateAddonFilesDeleteRequest(AddonFilesDeleteRequest request, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(request.SourcePath))
        {
            errorMessage = "SourcePath cannot be null or empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public static bool ValidateImageFileUploadRequest(ImageFileUploadRequest request, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(request.FilePath))
        {
            errorMessage = "FilePath cannot be null or empty.";
            return false;
        }
        
        if (Path.GetExtension(request.FilePath) != ".png"
            || Path.GetExtension(request.FilePath) != ".jpg"
            || Path.GetExtension(request.FilePath) != ".jpeg"
            || Path.GetExtension(request.FilePath) != ".gif"
            || Path.GetExtension(request.FilePath) != ".bmp"
            || Path.GetExtension(request.FilePath) != ".webp")
        {
            errorMessage = "File extension must be .png, .jpg, .jpeg, .gif, .bmp or .webp.";
            return false;
        }
        
        errorMessage = string.Empty;
        return true;
    }

    public static bool ValidateImageFileDownloadRequest(ImageFileDownloadRequest request, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(request.SourcePath))
        {
            errorMessage = "SourcePath cannot be null or empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.DestinationPath))
        {
            errorMessage = "DestinationPath cannot be null or empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public static bool ValidateImageFileDeleteRequest(ImageFileDeleteRequest request, out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(request.SourcePath))
        {
            errorMessage = "SourcePath cannot be null or empty.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public static bool ValidateProfileHatVariantFileUploadRequest(ProfileHatVariantFileUploadRequest request,
        out string errorMessage)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        if (string.IsNullOrWhiteSpace(request.FilePath))
        {
            errorMessage = "FilePath cannot be null or empty.";
            return false;
        }

        if (Path.GetExtension(request.FilePath) != ".png")
        {
            errorMessage = "File extension must be .png.";
            return false;
        }

        if (request.ProfileHatVariantId < 1 || request.ProfileHatVariantId > 5)
        {
            errorMessage = "ProfileHatVariantId must be between 1 and 5.";
            return false;
        }
        
        errorMessage = string.Empty;
        return true;
    }

    public static bool ValidateProfileHatVariantFileDeleteRequest(ProfileHatVariantFileDeleteRequest request, 
        out string errorMessage)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        if (request.ProfileHatVariantId < 1 || request.ProfileHatVariantId > 5)
        {
            errorMessage = "ProfileHatVariantId must be between 1 and 5.";
            return false;
        }
        
        errorMessage = string.Empty;
        return true;
    }

    public static bool ValidateProfileHatVariantGetByIdRequest(ProfileHatVariantGetByIdRequest request,
        out string errorMessage)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        if (request.ProfileHatVariantId < 1 || request.ProfileHatVariantId > 5)
        {
            errorMessage = "ProfileHatVariantId must be between 1 and 5.";
            return false;
        }
        
        errorMessage = string.Empty;
        return true;
    }
}