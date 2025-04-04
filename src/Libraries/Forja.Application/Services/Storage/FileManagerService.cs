namespace Forja.Application.Services.Storage;

public class FileManagerService : IFileManagerService
{
    private readonly IStorageService _storageService;

    public FileManagerService(IStorageService storageService)
    {
        _storageService = storageService;
    }

    /// <inheritdoc />
    public async Task<string> StartChunkedUploadAsync(StartChunkedUploadRequest request)
    {
        if (!StorageRequestsValidator.ValidateStartChunkedUploadRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        var uploadId = await _storageService.StartChunkedUploadAsync(request.FileName, request.FileSize, request.TotalChunks, request.UserId, request.ContentType);
        return uploadId;
    }

    /// <inheritdoc />
    public async Task<HttpStatusCode> UploadChunkAsync(UploadChunkRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadChunkRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (request.ChunkFile == null)
        {
            throw new ArgumentException("Chunk stream cannot be null.", nameof(request.ChunkFile));
        }
        
        await using var stream = request.ChunkFile.OpenReadStream();
        
        var result = await _storageService.UploadChunkAsync(request.UploadId, request.ChunkNumber, stream, request.ChunkSize);
        return result.ResponseStatusCode;
    }

    /// <inheritdoc />
    public async Task<ChankedUploadResponse> CompleteChunkedUploadAsync(CompleteChunkedUploadRequest request)
    {
        if (!StorageRequestsValidator.ValidateCompleteChunkedUploadRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        string destinationPath = request.FileType switch
        {
            FileType.FullGame => $"games/{request.GameId}/versions/{request.VersionId}/{request.FinalFileName}",
            FileType.GameFile => $"games/{request.GameId}/versions/{request.VersionId}/files/{request.FinalFileName}",
            FileType.GamePatch => $"games/{request.GameId}/patches/{request.FinalFileName}",
            FileType.GameAddon => $"games/{request.GameId}/addons/{request.AddonId}/{request.FinalFileName}",
            _ => throw new ArgumentOutOfRangeException(nameof(request.FileType), request.FileType, "Invalid file type")
        };

        return await _storageService.CompleteChunkedUploadAsync(request.UploadId, destinationPath);
    }

    /// <inheritdoc />
    public async Task DeleteGameFileAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        if (!request.ObjectPath.Contains("games"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }

    /// <inheritdoc />
    public async Task<string> UploadProductLogoAsync(UploadLogoRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadLogoRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (request.File == null)
        {
            throw new ArgumentException("File cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/product/logo/";
        string extension = Path.GetExtension(request.FileName);
        string filenameWithoutExtension = $"product-logo_{request.ProductId}";
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}{extension}";
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.ObjectSize, request.ContentType);
        return destinationFilePath;
    }

    /// <inheritdoc />
    public async Task DeleteProductLogoAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (!request.ObjectPath.Contains("images/product/logo/product-logo"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }
    
    /// <inheritdoc />
    public async Task<string> UploadProductImageAsync(UploadImageRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadImageRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        if (request.File == null)
        {
            throw new ArgumentException("File cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/product/album/";
        string extension = Path.GetExtension(request.FileName);
        string uniqueSuffix = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string filenameWithoutExtension = "product-image";
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}-{uniqueSuffix}{extension}";
        
        destinationFilePath = await EnsureUniqueObjectPathAsync(destinationFilePath);
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.ObjectSize, request.ContentType);
        return destinationFilePath;
    }

    /// <inheritdoc />
    public async Task DeleteProductImageAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (!request.ObjectPath.Contains("images/product/album/product-image"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }

    /// <inheritdoc />
    public async Task<string> UploadUserAvatarAsync(UploadAvatarRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadAvatarRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        if (request.File == null)
        {
            throw new ArgumentException("Stream cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/user/avatars/";
        string extension = Path.GetExtension(request.FileName);
        string filenameWithoutExtension = $"user-avatar_{request.UserId}";
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}{extension}";
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.ObjectSize, request.ContentType);
        return destinationFilePath;
    }

    /// <inheritdoc />
    public async Task DeleteUserAvatarAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (!request.ObjectPath.Contains("images/user/avatars/user-avatar"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }
    
    /// <inheritdoc />
    public async Task<string> GetPresignedUrlAsync(string objectPath, int expiresInSeconds = 3600)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
        {
            throw new ArgumentException("Object path cannot be empty.", nameof(objectPath));
        }

        if (expiresInSeconds <= 10)
        {
            throw new ArgumentException("Expiration time cannot be less than 10 seconds.", nameof(expiresInSeconds));
        }
        
        var result = await _storageService.GetPresignedUrlAsync(objectPath, expiresInSeconds);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for object path: {objectPath}");
        }
        
        return result;
    }
    
    /// <inheritdoc />
    public async Task<string> UploadProfileHatVariantFileAsync(ProfileHatVariantFileUploadRequest request)
    {
        if (!StorageRequestsValidator.ValidateProfileHatVariantFileUploadRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (request.File == null)
        {
            throw new ArgumentException("Stream cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/user/profile-hat-variants/";
        string extension = Path.GetExtension(request.FileName);
        string destinationFilePath = $"{destinationPath}{request.ProfileHatVariantId}{extension}";
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.FileSize, request.ContentType);
        return destinationFilePath;
    }

    /// <inheritdoc />
    public async Task DeleteProfileHatVariantFileAsync(ProfileHatVariantFileDeleteRequest request)
    {
        if (!StorageRequestsValidator.ValidateProfileHatVariantFileDeleteRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        var variantPath = await FindProfileHatVariantPathAsync(request.ProfileHatVariantId);
        if (string.IsNullOrWhiteSpace(variantPath))
        {
            throw new InvalidOperationException($"Profile hat variant with ID {request.ProfileHatVariantId} not found.");
        }
        
        await _storageService.DeleteFileAsync(variantPath);
    }

    /// <inheritdoc />
    public async Task<string> GetPresignedProfileHatVariantUrlAsync(ProfileHatVariantGetByIdRequest request)
    {
        if (!StorageRequestsValidator.ValidateProfileHatVariantGetByIdRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        var variantPath = await FindProfileHatVariantPathAsync(request.ProfileHatVariantId);
        if (string.IsNullOrWhiteSpace(variantPath))
        {
            throw new InvalidOperationException($"Profile hat variant with ID {request.ProfileHatVariantId} not found.");
        }
        
        var result = await _storageService.GetPresignedUrlAsync(variantPath);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for hat variant: {request.ProfileHatVariantId}");
        }
        
        return result;
    }

    private async Task<string?> FindProfileHatVariantPathAsync(short profileHatVariantId)
    {
        string[] possibleExtensions = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp"];
        foreach (var extension in possibleExtensions)
        {
            string path = $"images/user/profile-hat-variants/{profileHatVariantId}{extension}";
            if (await _storageService.IsObjectExistsAsync(path))
            {
                return path;
            }
        }
        return null;
    }

    private async Task<string> EnsureUniqueObjectPathAsync(string objectPath)
    {
        int count = 0;
        string uniqueObjectPath = objectPath;
        
        if (uniqueObjectPath.Contains("games"))
        {
            uniqueObjectPath = uniqueObjectPath.Replace("games", "_");
        }
        
        while (await _storageService.IsObjectExistsAsync(objectPath))
        {
            count++;
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(objectPath);
            string extension = Path.GetExtension(objectPath);
            uniqueObjectPath = $"{nameWithoutExtension}({count}){extension}";
        }

        return uniqueObjectPath;
    }
    
    private async Task UploadStreamAsync(string objectPath, Stream dataStream, long objectSize, string contentType)
    {
        var result = await _storageService.UploadStreamAsync(objectPath, dataStream, objectSize, contentType);
        if (result.ResponseStatusCode != HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Failed to upload file, status code: {result.ResponseStatusCode}");
        }
    }
}