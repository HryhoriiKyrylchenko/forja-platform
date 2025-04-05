namespace Forja.Application.Interfaces.Storage;

/// <summary>
/// Defines methods for managing file storage, uploading, deleting, and retrieving files
/// in a chunked or full upload manner for various purposes such as user avatars, product images,
/// product logos, and custom content types.
/// </summary>
public interface IFileManagerService
{
    Task<string> StartChunkedUploadAsync(StartChunkedUploadRequest request);

    Task<HttpStatusCode> UploadChunkAsync(UploadChunkRequest request);

    Task<ChankedUploadResponse> CompleteChunkedUploadAsync(CompleteChunkedUploadRequest request);

    Task DeleteGameFileAsync(DeleteObjectRequest request);

    Task<string> UploadProductLogoAsync(UploadObjectImageRequest request);

    Task DeleteProductLogoAsync(DeleteObjectRequest request);

    Task<string> UploadProductImageAsync(UploadImageRequest request);

    Task DeleteProductImageAsync(DeleteObjectRequest request);

    Task<string> UploadUserAvatarAsync(UploadObjectImageRequest request);

    Task DeleteUserAvatarAsync(DeleteObjectRequest request);
    
    Task<string> UploadAchievementImageAsync(UploadObjectImageRequest request);

    Task DeleteAchievementImageAsync(DeleteObjectRequest request);
    
    Task<string> UploadMatureContentImageAsync(UploadObjectImageRequest request);

    Task DeleteMatureContentImageAsync(DeleteObjectRequest request);
    
    Task<string> UploadMechanicImageAsync(UploadObjectImageRequest request);

    Task DeleteMechanicImageAsync(DeleteObjectRequest request);
    
    Task<string> UploadNewsArticleImageAsync(UploadObjectImageRequest request);

    Task DeleteNewsArticleImageAsync(DeleteObjectRequest request);

    Task<string> GetPresignedUrlAsync(string objectPath, int expiresInSeconds = 3600);

    Task<string> UploadProfileHatVariantFileAsync(ProfileHatVariantFileUploadRequest request);

    Task DeleteProfileHatVariantFileAsync(ProfileHatVariantFileDeleteRequest request);

    Task<string> GetPresignedProfileHatVariantUrlAsync(ProfileHatVariantGetByIdRequest request);
    
    Task<List<string>> GetPresignedProductImagesUrlsAsync(Guid productId, int expiresInSeconds = 3600);
    Task<string> GetPresignedUserAvatarUrlAsync(Guid userId, int expiresInSeconds = 3600);
    Task<string> GetPresignedProductLogoUrlAsync(Guid productId, int expiresInSeconds = 3600);
    Task<string> GetPresignedAchievementImageUrlAsync(Guid achievementId, int expiresInSeconds = 3600);
    Task<string> GetPresignedMatureContentImageUrlAsync(Guid matureContentId, int expiresInSeconds = 3600);
    Task<string> GetPresignedMechanicImageUrlAsync(Guid mechanicId, int expiresInSeconds = 3600);
    Task<string> GetPresignedNewsArticleImageUrlAsync(Guid mechanicId, int expiresInSeconds = 3600);
}