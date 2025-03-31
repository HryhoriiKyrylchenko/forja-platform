namespace Forja.Infrastructure.Interfaces;

public interface IStorageService
{
    Task<bool> FileExistsAsync(string filePath);
    Task<bool> FolderExistsAsync(string folderPath);
    Task<PutObjectResponse> UploadFileAsync(string destinationObjectPath, string filePath);
    Task DownloadFileAsync(string objectPath, string downloadFilePath);
    Task DeleteFileAsync(string objectPath);
    Task<List<PutObjectResponse>> UploadFolderAsync(string destinationPath, string folderPath);
    Task DownloadFolderAsync(string sourcePath, string destinationFolderPath);
    Task DeleteFolderAsync(string folderPath);
    Task<string> GetPresignedUrlAsync(string objectPath, int expiryInSeconds);
}