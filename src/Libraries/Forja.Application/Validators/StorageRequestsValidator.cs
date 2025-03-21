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
}