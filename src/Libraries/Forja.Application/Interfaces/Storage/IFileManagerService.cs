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

    Task<string> UploadProductLogoAsync(UploadLogoRequest request);

    Task DeleteProductLogoAsync(DeleteObjectRequest request);

    Task<string> UploadProductImageAsync(UploadImageRequest request);

    Task DeleteProductImageAsync(DeleteObjectRequest request);

    Task<string> UploadUserAvatarAsync(UploadAvatarRequest request);

    Task DeleteUserAvatarAsync(DeleteObjectRequest request);

    Task<string> GetPresignedUrlAsync(string objectPath, int expiresInSeconds = 3600);

    Task<string> UploadProfileHatVariantFileAsync(ProfileHatVariantFileUploadRequest request);

    Task DeleteProfileHatVariantFileAsync(ProfileHatVariantFileDeleteRequest request);

    Task<string> GetPresignedProfileHatVariantUrlAsync(ProfileHatVariantGetByIdRequest request);
}