namespace Forja.Application.Interfaces.Storage;

/// <summary>
/// Defines methods for managing file storage, uploading, deleting, and retrieving files
/// in a chunked or full upload manner for various purposes such as user avatars, product images,
/// product logos, and custom content types.
/// </summary>
public interface IFileManagerService
{
    /// <summary>
    /// Initializes a chunked upload process by creating an upload session and providing an identifier
    /// for the file being uploaded, along with necessary metadata such as the file name, size,
    /// total number of chunks, and the user initiating the upload.
    /// </summary>
    /// <param name="request">An object containing details about the file to upload, including its name, size, total number of chunks, user's ID, and content type.</param>
    /// <returns>A task that represents the asynchronous operation and returns a string identifier for the initiated upload session.</returns>
    Task<string> StartChunkedUploadAsync(StartChunkedUploadRequest request);

    /// <summary>
    /// Handles the upload of a specific chunk during a chunked upload process.
    /// </summary>
    /// <param name="request">An object containing the details of the chunk being uploaded,
    /// such as its upload ID, chunk number, the chunk file, and its size.</param>
    /// <returns>A task representing the asynchronous operation that completes with an HTTP status code
    /// indicating the result of the upload.</returns>
    Task<HttpStatusCode> UploadChunkAsync(UploadChunkRequest request);

    /// <summary>
    /// Completes a chunked file upload by finalizing the uploaded chunks
    /// and returning details about the uploaded file, such as its path and hash.
    /// </summary>
    /// <param name="request">The request containing information needed to complete the chunked upload.</param>
    /// <returns>A response object containing the status code, content, file path, and hash of the completed upload.</returns>
    Task<ChankedUploadResponse> CompleteChunkedUploadAsync(CompleteChunkedUploadRequest request);

    /// <summary>
    /// Asynchronously deletes a game file from storage based on the specified request.
    /// </summary>
    /// <param name="request">An object containing details about the file to delete, such as its path or identifier.</param>
    /// <returns>A task representing the asynchronous operation of deleting the specified file.</returns>
    Task DeleteGameFileAsync(DeleteObjectRequest request);

    /// <summary>
    /// Uploads a product logo to the file storage and associates the uploaded file
    /// with the specified product object.
    /// </summary>
    /// <param name="request">The request containing the product object identifier and file data to be uploaded.</param>
    /// <returns>A string containing the path or identifier of the uploaded product logo in the storage.</returns>
    Task<string> UploadProductLogoAsync(UploadObjectImageRequest request);

    /// <summary>
    /// Deletes a product logo asynchronously based on the details provided in the request object.
    /// </summary>
    /// <param name="request">The request object containing the details necessary to locate and delete the product logo.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates the success or failure of the deletion process.</returns>
    Task DeleteProductLogoAsync(DeleteObjectRequest request);

    /// <summary>
    /// Asynchronously uploads an image associated with a product, saving it to the appropriate storage
    /// and handling necessary metadata.
    /// </summary>
    /// <param name="request">An object containing details about the image upload, including metadata such as file name and product association.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a URL to the uploaded image's storage location.</returns>
    Task<string> UploadProductImageAsync(UploadImageRequest request);

    /// <summary>
    /// Deletes the specified product image from the storage.
    /// </summary>
    /// <param name="request">
    /// An object containing the path of the product image to delete.
    /// The <see cref="DeleteObjectRequest.ObjectPath"/> property must specify the file path of the product image within the storage.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task DeleteProductImageAsync(DeleteObjectRequest request);

    /// <summary>
    /// Asynchronously uploads a user's avatar image to the storage system.
    /// </summary>
    /// <param name="request">An object representing the details of the avatar image to upload, including file stream, file name, and associated metadata.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is a string containing the URL or identifier of the uploaded avatar image.</returns>
    Task<string> UploadUserAvatarAsync(UploadObjectImageRequest request);

    /// <summary>
    /// Deletes a user avatar based on the provided request.
    /// </summary>
    /// <param name="request">The request object containing details about the avatar to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteUserAvatarAsync(DeleteObjectRequest request);

    /// <summary>
    /// Uploads an achievement image to the storage system.
    /// </summary>
    /// <param name="request">The request containing the image file, object ID, size, and content type.</param>
    /// <returns>Returns the URL of the uploaded achievement image as a string.</returns>
    Task<string> UploadAchievementImageAsync(UploadObjectImageRequest request);

    /// <summary>
    /// Deletes an achievement image from the storage service based on the specified object path.
    /// </summary>
    /// <param name="request">The request object containing the path of the achievement image to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAchievementImageAsync(DeleteObjectRequest request);

    /// <summary>
    /// Uploads an image containing mature content to the storage service asynchronously.
    /// This method ensures that the image is stored with proper handling suitable for explicit content.
    /// </summary>
    /// <param name="request">An object containing the image data, file name, and associated metadata for the upload process.</param>
    /// <returns>A task that represents the asynchronous operation and returns a string indicating the file location or identifier of the uploaded image.</returns>
    Task<string> UploadMatureContentImageAsync(UploadObjectImageRequest request);

    /// <summary>
    /// Deletes a mature content image from the storage based on the specified request.
    /// </summary>
    /// <param name="request">The details of the image to be deleted, including its object path.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteMatureContentImageAsync(DeleteObjectRequest request);

    /// <summary>
    /// Uploads an image for a mechanic entity to the storage and returns the URL or identifier
    /// of the uploaded image.
    /// </summary>
    /// <param name="request">
    /// The request containing information about the image to be uploaded,
    /// such as the object ID, file data, size, content type, and file name.
    /// </param>
    /// <returns>
    /// A string representing the URL or identifier of the uploaded mechanic image.
    /// </returns>
    Task<string> UploadMechanicImageAsync(UploadObjectImageRequest request);

    /// <summary>
    /// Deletes a mechanic image file from the storage based on the specified request.
    /// </summary>
    /// <param name="request">
    /// An instance of <see cref="DeleteObjectRequest"/> that contains the path
    /// of the mechanic image to be deleted.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous delete operation.
    /// </returns>
    Task DeleteMechanicImageAsync(DeleteObjectRequest request);

    /// <summary>
    /// Asynchronously uploads an image related to a news article to the designated storage or media server.
    /// </summary>
    /// <param name="request">An object containing details necessary to perform the upload, such as the image data, file name, and associated metadata.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a string indicating the URL or identifier of the uploaded image.</returns>
    Task<string> UploadNewsArticleImageAsync(UploadObjectImageRequest request);

    /// <summary>
    /// Deletes a specified news article image from storage based on the provided object path.
    /// </summary>
    /// <param name="request">An instance of <see cref="DeleteObjectRequest"/> containing the object path of the news article image to be deleted.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteNewsArticleImageAsync(DeleteObjectRequest request);

    /// <summary>
    /// Generates a presigned URL for accessing a file in storage.
    /// </summary>
    /// <param name="objectPath">
    /// The path to the object in the storage for which the presigned URL should be generated.
    /// </param>
    /// <param name="expiresInSeconds">
    /// The duration (in seconds) for which the presigned URL will remain valid. Defaults to 3600 seconds.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the presigned URL as a string.
    /// </returns>
    Task<string> GetPresignedUrlAsync(string objectPath, int expiresInSeconds = 3600);

    /// <summary>
    /// Uploads a file for a specific profile hat variant, associating it with the provided variant identifier.
    /// </summary>
    /// <param name="request">
    /// An object containing the necessary information for uploading the file, such as the variant identifier,
    /// file data, and metadata including file name and content type.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a string indicating
    /// the unique identifier for the successfully uploaded file.
    /// </returns>
    Task<string> UploadProfileHatVariantFileAsync(ProfileHatVariantFileUploadRequest request);

    /// <summary>
    /// Deletes the profile hat variant file associated with the specified profile hat variant.
    /// </summary>
    /// <param name="request">The request containing the profile hat variant ID to identify which file to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task does not return a value.</returns>
    Task DeleteProfileHatVariantFileAsync(ProfileHatVariantFileDeleteRequest request);

    /// <summary>
    /// Generates a presigned URL for accessing a specific profile hat variant file, allowing temporary access
    /// to the file for a given request.
    /// </summary>
    /// <param name="request">An object that contains the identifier of the profile hat variant to retrieve the presigned URL for.</param>
    /// <returns>A task that represents the asynchronous operation and returns a string containing the presigned URL.</returns>
    Task<string> GetPresignedProfileHatVariantUrlAsync(ProfileHatVariantGetByIdRequest request);

    /// <summary>
    /// Asynchronously generates presigned URLs for accessing product images stored in a secured storage system.
    /// The presigned URLs allow temporary access to the specified images without requiring direct authentication.
    /// </summary>
    /// <param name="productId">The unique identifier of the product whose image URLs are being requested.</param>
    /// <param name="expiresInSeconds">The duration of time for which the presigned URLs will remain valid.</param>
    /// <returns>A dictionary mapping each image key to its corresponding presigned URL.</returns>
    Task<List<string>> GetPresignedProductImagesUrlsAsync(Guid productId, int expiresInSeconds = 3600);

    /// <summary>
    /// Asynchronously retrieves a pre-signed URL for the user's avatar image.
    /// This can be used to access the avatar image directly from storage with temporary access permissions.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the pre-signed URL is being requested.</param>
    /// <param name="expiresInSeconds">The number of seconds until the presigned URL expires. Default value is 3600 seconds.</param>
    /// <returns>A task representing the asynchronous operation, with a result containing the pre-signed URL as a string.</returns>
    Task<string> GetPresignedUserAvatarUrlAsync(Guid userId, int expiresInSeconds = 3600);

    /// <summary>
    /// Generates a presigned URL for accessing a product logo in storage.
    /// </summary>
    /// <param name="productId">The unique identifier of the product whose logo URL is being generated.</param>
    /// <param name="expiresInSeconds">The number of seconds until the presigned URL expires. Default value is 3600 seconds.</param>
    /// <returns>A presigned URL string allowing secure and temporary access to the product logo.</returns>
    Task<string> GetPresignedProductLogoUrlAsync(Guid productId, int expiresInSeconds = 3600);

    /// <summary>
    /// Asynchronously generates a pre-signed URL for accessing the achievement image.
    /// </summary>
    /// <param name="achievementId">The unique identifier of the achievement.</param>
    /// <param name="expiresInSeconds">The duration after which the URL will expire.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the pre-signed URL as a string.</returns>
    Task<string> GetPresignedAchievementImageUrlAsync(Guid achievementId, int expiresInSeconds = 3600);

    /// <summary>
    /// Generates a presigned URL for accessing the mature content image.
    /// </summary>
    /// <param name="matureContentId">The unique identifier of the mature content.</param>
    /// <param name="expiresInSeconds">The duration in seconds for which the presigned URL will be valid. Defaults to 3600 seconds.</param>
    /// <returns>A presigned URL as a string, allowing temporary access to the mature content image.</returns>
    Task<string> GetPresignedMatureContentImageUrlAsync(Guid matureContentId, int expiresInSeconds = 3600);

    /// <summary>
    /// Retrieves a presigned URL for accessing the specified mechanic image.
    /// </summary>
    /// <param name="mechanicId">The unique identifier of the mechanic whose image URL is to be retrieved.</param>
    /// <param name="expiresInSeconds">The duration, in seconds, for which the presigned URL remains valid. Defaults to 3600 seconds.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the presigned URL as a string.</returns>
    Task<string> GetPresignedMechanicImageUrlAsync(Guid mechanicId, int expiresInSeconds = 3600);

    /// <summary>
    /// Retrieves a presigned URL for the image associated with a news article, allowing
    /// temporary access for file download or operations without direct access to the storage.
    /// </summary>
    /// <param name="newsArticleId">The unique identifier of the news article whose image URL is requested.</param>
    /// <param name="expiresInSeconds">The expiration time for the presigned URL in seconds. Defaults to 3600 seconds.</param>
    /// <returns>A presigned URL for the news article's image, valid for the specified duration.</returns>
    Task<string> GetPresignedNewsArticleImageUrlAsync(Guid newsArticleId, int expiresInSeconds = 3600);
}