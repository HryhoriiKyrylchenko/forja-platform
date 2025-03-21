namespace Forja.Application.Interfaces.Storage;

public interface IFileManagerService
{
    Task<string> UploadGameFilesAsync(GameFilesUploadRequest request);
    Task DownloadGameFilesAsync(GameFilesDownloadRequest request);
    Task DeleteGameFilesAsync(GameFilesDeleteRequest request);

    Task<string> UploadAddonFilesAsync(AddonFilesUploadRequest request);
    Task DownloadAddonFilesAsync(AddonFilesDownloadRequest request);
    Task DeleteAddonFilesAsync(AddonFilesDeleteRequest request);

    Task<string> UploadImageFileAsync(ImageFileUploadRequest request);
    Task DownloadImageFileAsync(ImageFileDownloadRequest request);
    Task DeleteImageFileAsync(ImageFileDeleteRequest request);
    Task<string> GetPresignedUrlAsync(string objectPath);
    
    Task<string> UploadUserImageFileAsync(ImageFileUploadRequest request);
    Task DeleteUserImageFileAsync(ImageFileDeleteRequest request);
}