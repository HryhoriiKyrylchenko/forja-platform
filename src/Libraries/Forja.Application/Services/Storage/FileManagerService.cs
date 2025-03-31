using System.Net;

namespace Forja.Application.Services.Storage;

public class FileManagerService : IFileManagerService
{
    private readonly IStorageService _storageService;

    public FileManagerService(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<string> UploadGameFilesAsync(GameFilesUploadRequest request)
    {
        if (!StorageRequestsValidator.ValidateGameFilesUploadRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }
        
        string folderName = Path.GetFileName(request.FolderPath).TrimEnd('/');
        
        string uniqueSuffix = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string destinationPath = $"games/{folderName}-{uniqueSuffix}/";

        var result = await _storageService.UploadFolderAsync(destinationPath, request.FolderPath);
        if (result.Any(r => r.ResponseStatusCode != HttpStatusCode.OK))
        {
            throw new InvalidOperationException($"Failed to upload files, status code: {result.First(r => r.ResponseStatusCode != HttpStatusCode.OK).ResponseStatusCode.ToString()}");
        }
        return destinationPath;
    }

    public async Task DownloadGameFilesAsync(GameFilesDownloadRequest request)
    {
        if (!StorageRequestsValidator.ValidateGameFilesDownloadRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }
        
        await _storageService.DownloadFolderAsync(request.SourcePath, request.DestinationPath);
    }

    public async Task DeleteGameFilesAsync(GameFilesDeleteRequest request)
    {
        if (!StorageRequestsValidator.ValidateGameFilesDeleteRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }
        
        await _storageService.DeleteFolderAsync(request.SourcePath);
    }

    public async Task<string> UploadAddonFilesAsync(AddonFilesUploadRequest request)
    {
        if (!StorageRequestsValidator.ValidateAddonFilesUploadRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }

        string folderName = Path.GetFileName(request.FolderPath).TrimEnd('/');
        
        string uniqueSuffix = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string destinationPath = $"addons/{folderName}-{uniqueSuffix}/";
        
        destinationPath = await EnsureUniqueFolderPathAsync(destinationPath);
        
        var result = await _storageService.UploadFolderAsync(destinationPath, request.FolderPath);
        if (result.Any(r => r.ResponseStatusCode != HttpStatusCode.OK))
        {
            throw new InvalidOperationException($"Failed to upload files, status code: {result.First(r => r.ResponseStatusCode != HttpStatusCode.OK).ResponseStatusCode.ToString()}");
        }
        
        return destinationPath;
    }

    public async Task DownloadAddonFilesAsync(AddonFilesDownloadRequest request)
    {
        if (!StorageRequestsValidator.ValidateAddonFilesDownloadRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }
        
        await _storageService.DownloadFolderAsync(request.SourcePath, request.DestinationPath);
    }

    public async Task DeleteAddonFilesAsync(AddonFilesDeleteRequest request)
    {
        if (!StorageRequestsValidator.ValidateAddonFilesDeleteRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }
        
        await _storageService.DeleteFolderAsync(request.SourcePath);
    }

    public async Task<string> UploadImageFileAsync(ImageFileUploadRequest request)
    {
        if (!StorageRequestsValidator.ValidateImageFileUploadRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }

        string destinationPath = "images/";
        string extension = Path.GetExtension(request.FilePath);
        string uniqueSuffix = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string filenameWithoutExtension = Path.GetFileNameWithoutExtension(request.FilePath);
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}-{uniqueSuffix}{extension}";
        
        destinationFilePath = await EnsureUniqueFilePathAsync(destinationFilePath);
        
        var result = await _storageService.UploadFileAsync(destinationFilePath, request.FilePath);
        if (result.ResponseStatusCode != HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Failed to upload file, status code: {result.ResponseStatusCode.ToString()}");
        }
        
        return destinationFilePath;
    }

    public async Task DownloadImageFileAsync(ImageFileDownloadRequest request)
    {
        if (!StorageRequestsValidator.ValidateImageFileDownloadRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }
        
        string fileName = Path.GetFileName(request.SourcePath);
        string destinationFilePath = Path.Combine(request.DestinationPath, fileName);
        
        await _storageService.DownloadFileAsync(request.SourcePath, destinationFilePath);
    }

    public async Task DeleteImageFileAsync(ImageFileDeleteRequest request)
    {
        if (!StorageRequestsValidator.ValidateImageFileDeleteRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }
        
        await _storageService.DeleteFileAsync(request.SourcePath);
    }

    public async Task<string> GetPresignedUrlAsync(string objectPath)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
        {
            throw new ArgumentException("Object path cannot be null or empty.", nameof(objectPath));
        }
        
        var result = await _storageService.GetPresignedUrlAsync(objectPath, 3600);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for object path: {objectPath}");
        }
        
        return result;
    }

    public async Task<string> UploadUserImageFileAsync(ImageFileUploadRequest request)
    {
        if (!StorageRequestsValidator.ValidateImageFileUploadRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }

        string destinationPath = "user-images/";
        string extension = Path.GetExtension(request.FilePath);
        string uniqueSuffix = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string filenameWithoutExtension = Path.GetFileNameWithoutExtension(request.FilePath);
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}-{uniqueSuffix}{extension}";
        
        destinationFilePath = await EnsureUniqueFilePathAsync(destinationFilePath);
        
        await _storageService.UploadFileAsync(destinationFilePath, request.FilePath);
        return destinationFilePath;
    }

    public async Task DeleteUserImageFileAsync(ImageFileDeleteRequest request)
    {
        if (!StorageRequestsValidator.ValidateImageFileDeleteRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }
        
        string path = request.SourcePath.Split('/').First();
        if (path != "user-images")
        {
            throw new InvalidOperationException("Invalid source path.");
        }
        
        await _storageService.DeleteFileAsync(request.SourcePath);
    }

    public async Task<string> UploadProfileHatVariantFileAsync(ProfileHatVariantFileUploadRequest request)
    {
        if (!StorageRequestsValidator.ValidateProfileHatVariantFileUploadRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }

        string destinationPath = "profile-hat-variants/";
        string extension = Path.GetExtension(request.FilePath);
        string destinationFilePath = $"{destinationPath}{request.ProfileHatVariantId}{extension}";
        
        await _storageService.UploadFileAsync(destinationFilePath, request.FilePath);
        return destinationFilePath;
    }

    public async Task DeleteProfileHatVariantFileAsync(ProfileHatVariantFileDeleteRequest request)
    {
        if (!StorageRequestsValidator.ValidateProfileHatVariantFileDeleteRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }

        string path = $"profile-hat-variants/{request.ProfileHatVariantId}.png"; // Path should be in .png format.
        
        await _storageService.DeleteFileAsync(path);
    }

    public async Task<string> GetPresignedProfileHatVariantUrlAsync(ProfileHatVariantGetByIdRequest request)
    {
        if (!StorageRequestsValidator.ValidateProfileHatVariantGetByIdRequest(request, out string errorMessage))
        {
            throw new ArgumentException($"Invalid request: {errorMessage}", nameof(request));
        }
        
        string objectPath = $"profile-hat-variants/{request.ProfileHatVariantId}.png";
        
        var result = await _storageService.GetPresignedUrlAsync(objectPath, 3600);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for object path: {objectPath}");
        }
        
        return result;
    }

    private async Task<string> EnsureUniqueFilePathAsync(string filePath)
    {
        int count = 0;
        string uniqueFilePath = filePath;

        while (await _storageService.FileExistsAsync(filePath))
        {
            count++;
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            uniqueFilePath = $"{nameWithoutExtension}-{count}{extension}";
        }

        return uniqueFilePath;
    }

    private async Task<string> EnsureUniqueFolderPathAsync(string folderPath)
    {
        int count = 0;
        string uniqueFolderPath = folderPath;

        while (await _storageService.FolderExistsAsync(folderPath))
        {
            count++;
            uniqueFolderPath = $"{folderPath.TrimEnd('/')}-{count}/";
        }

        return uniqueFolderPath;
    }
}