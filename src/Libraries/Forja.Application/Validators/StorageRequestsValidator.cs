namespace Forja.Application.Validators;

public static class StorageRequestsValidator
{
    public static bool ValidateStartChunkedUploadRequest(StartChunkedUploadRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            errorMessage = "File name cannot be null or empty.";
            return false;
        }

        if (request.FileSize < 1)
        {
            errorMessage = "File size must be greater than 0.";
            return false;
        }

        if (request.TotalChunks < 1)
        {
            errorMessage = "Total chunks must be greater than 0.";
            return false;
        }

        if (request.UserId == Guid.Empty)
        {
            errorMessage = "User ID cannot be null or empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.ContentType))
        {
            errorMessage = "Content type cannot be null or empty.";
            return false;
        }
        
        return true;
    }
    
    public static bool ValidateUploadChunkRequest(UploadChunkRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.UploadId))
        {
            errorMessage = "Upload ID cannot be null or empty.";
            return false;
        }

        if (request.ChunkSize < 1)
        {
            errorMessage = "Chunk size must be greater than 0.";
            return false;
        }

        if (request.ChunkNumber < 1)
        {
            errorMessage = "Chunk number must be greater than 0.";
            return false;
        }
        
        return true;
    }
    
    public static bool ValidateCompleteChunkedUploadRequest(CompleteChunkedUploadRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.UploadId))
        {
            errorMessage = "Upload ID cannot be null or empty.";
            return false;
        }

        if (request.GameId == Guid.Empty)
        {
            errorMessage = "Game ID cannot be null or empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Version))
        {
            errorMessage = "Version cannot be null or empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.FinalFileName))
        {
            errorMessage = "Final file name cannot be null or empty.";
            return false;
        }

        if (request.FileType == FileType.GameAddon && (request.AddonId == null || request.AddonId == Guid.Empty))
        {
            errorMessage = "Addon ID cannot be null or empty.";
            return false;
        }

        if (request.FileType == FileType.GamePatch 
            && string.IsNullOrWhiteSpace(request.FromVersion) 
            && string.IsNullOrWhiteSpace(request.ToVersion))
        {
            errorMessage = "From version and to version cannot be null or empty.";
            return false;
        }

        if (request.FileType == FileType.GameAddon && (request.AddonId == null || request.AddonId == Guid.Empty))
        {
            errorMessage = "Addon ID cannot be null or empty.";
            return false;
        }
        
        return true;
    }
    
    public static bool ValidateUploadObjectImageRequest(UploadObjectImageRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }

        if (request.ObjectId == Guid.Empty)
        {
            errorMessage = "Object ID cannot be null or empty.";
            return false;
        }
        
        if (request.ObjectSize < 1)
        {
            errorMessage = "Object size must be greater than 0.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.ContentType))
        {
            errorMessage = "Content type cannot be null or empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            errorMessage = "File name cannot be null or empty.";
            return false;
        }

        if (!new HashSet<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp" }.Contains(Path.GetExtension(request.FileName)))
        {
            errorMessage = "File extension must be one of the following: .png, .jpg, .jpeg, .gif, .bmp, .webp";
            return false;
        }
        
        return true;
    }
    
    public static bool ValidateUploadImageRequest(UploadImageRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }

        if (request.ObjectSize < 1)
        {
            errorMessage = "Object size must be greater than 0.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.ContentType))
        {
            errorMessage = "Content type cannot be null or empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            errorMessage = "File name cannot be null or empty.";
            return false;
        }

        if (!new HashSet<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp" }.Contains(Path.GetExtension(request.FileName)))
        {
            errorMessage = "File extension must be one of the following: .png, .jpg, .jpeg, .gif, .bmp, .webp";
            return false;
        }
        
        return true;
    }

    public static bool ValidateUploadProductImageRequest(UploadProductImageRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }

        if (request.ProductId == Guid.Empty)
        {
            errorMessage = "Product ID cannot be null or empty.";
            return false;
        }
        
        if (request.ObjectSize < 1)
        {
            errorMessage = "Object size must be greater than 0.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.ContentType))
        {
            errorMessage = "Content type cannot be null or empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            errorMessage = "File name cannot be null or empty.";
            return false;
        }

        if (!new HashSet<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp" }.Contains(Path.GetExtension(request.FileName)))
        {
            errorMessage = "File extension must be one of the following: .png, .jpg, .jpeg, .gif, .bmp, .webp";
            return false;
        }

        return true;
    }
    
    public static bool ValidateDeleteObjectRequest(DeleteObjectRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.ObjectPath))
        {
            errorMessage = "Object path cannot be null or empty.";
            return false;
        }
        
        return true;
    }
    
    public static bool ValidateGetPresignedFileUrlRequest(GetPresignedFileUrlRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.ObjectPath))
        {
            errorMessage = "Object path cannot be null or empty.";
            return false;
        }

        if (request.ExpirationInSeconds < 10)
        {
            errorMessage = "Expiration time must be greater than 10 seconds.";
            return false;
        }

        if (request.ProductId == Guid.Empty)
        {
            errorMessage = "Product ID cannot be null or empty.";
            return false;
        }

        if (!request.ObjectPath.Contains(request.ProductId.ToString()))
        {
            errorMessage = "Object path must contain the product ID.";
            return false;
        }
        
        return true;
    }
    
    public static bool ValidateGetPresignedImageUrlRequest(GetPresignedImageUrlRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.ObjectPath))
        {
            errorMessage = "Object path cannot be null or empty.";
            return false;
        }

        if (request.ExpirationInSeconds < 10)
        {
            errorMessage = "Expiration time must be greater than 10 seconds.";
            return false;
        }
        
        if (!request.ObjectPath.Contains("images") || request.ObjectPath.Contains("games"))
        {
            errorMessage = "Wrong object path.";
            return false;
        }
        
        return true;
    }
    
    public static bool ValidateProfileHatVariantFileUploadRequest(ProfileHatVariantFileUploadRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }
        
        if (request.ProfileHatVariantId < 1)
        {
            errorMessage = "ProfileHatVariantId must be greater than 0.";
            return false;
        }

        if (request.FileSize < 1)
        {
            errorMessage = "File size must be greater than 0.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.ContentType))
        {
            errorMessage = "Content type cannot be null or empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            errorMessage = "File name cannot be null or empty.";
            return false;
        }
        
        if (!new HashSet<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp" }.Contains(Path.GetExtension(request.FileName)))
        {
            errorMessage = "File extension must be one of the following: .png, .jpg, .jpeg, .gif, .bmp, .webp";
            return false;
        }
        
        return true;
    }
    
    public static bool ValidateProfileHatVariantFileDeleteRequest(ProfileHatVariantFileDeleteRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }
        
        if (request.ProfileHatVariantId < 1)
        {
            errorMessage = "ProfileHatVariantId must be greater than 0.";
            return false;
        }
        
        return true;
    }
    
    public static bool ValidateProfileHatVariantGetByIdRequest(ProfileHatVariantGetByIdRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        
        if (request == null)
        {
            errorMessage = "Request cannot be null.";
            return false;
        }
        
        if (request.ProfileHatVariantId < 1)
        {
            errorMessage = "ProfileHatVariantId must be greater than 0.";
            return false;
        }
        
        return true;
    }
}