namespace Forja.Infrastructure.Interfaces;

/// <summary>
/// Defines the contract for storage service interactions, providing methods
/// to manage files and folders, such as checking existence, uploading,
/// downloading, deleting, and generating pre-signed URLs for access.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Uploads a stream of data to the specified storage location.
    /// </summary>
    /// <param name="objectPath">The path in the storage bucket where the object will be stored.</param>
    /// <param name="dataStream">The stream containing the data to upload.</param>
    /// <param name="objectSize">The size of the object being uploaded, in bytes.</param>
    /// <param name="contentType">The MIME type of the object being uploaded.</param>
    /// <returns>A <see cref="PutObjectResponse"/> object representing the result of the upload operation.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="objectPath"/> or <paramref name="contentType"/> is null, empty, or consists only of whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataStream"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="objectSize"/> is less than or equal to zero.</exception>
    /// <exception cref="Exception">Thrown if an error occurs during the upload process.</exception>
    Task<PutObjectResponse> UploadStreamAsync(string objectPath, Stream dataStream, long objectSize, string contentType);

    /// <summary>
    /// Initiates a chunked upload for a specified file by generating a unique upload ID
    /// and storing the metadata in the distributed cache.
    /// </summary>
    /// <param name="fileName">The name of the file to be uploaded.</param>
    /// <param name="fileSize">The total size of the file in bytes.</param>
    /// <param name="totalChunks">The total number of chunks the file will be divided into.</param>
    /// <param name="userId">The unique identifier of the user initiating the upload.</param>
    /// <param name="contentType">The MIME type of the file being uploaded.</param>
    /// <returns>A unique identifier for the chunked upload session.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="fileName"/> or <paramref name="contentType"/> is null or empty, or when <paramref name="userId"/> is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="fileSize"/> or <paramref name="totalChunks"/> is less than 1.</exception>
    Task<string> StartChunkedUploadAsync(string fileName, long fileSize, int totalChunks, Guid userId, string contentType);

    /// <summary>
    /// Uploads a chunk of a file as part of a multi-part upload process.
    /// </summary>
    /// <param name="uploadId">The unique identifier for the upload session.</param>
    /// <param name="partNumber">The part number of the chunk being uploaded, must be greater than 0.</param>
    /// <param name="chunkStream">The stream containing the chunk data to be uploaded.</param>
    /// <param name="chunkSize">The size of the chunk in bytes, must be greater than 0.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a <see cref="PutObjectResponse"/>
    /// which provides the result of the upload operation.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="uploadId"/> is null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="partNumber"/> is less than 1 or when the part number exceeds the total chunks.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the chunk data cannot be processed or if the uploaded chunks list is invalid.</exception>
    Task<PutObjectResponse> UploadChunkAsync(string uploadId, int partNumber, Stream chunkStream, long chunkSize);
    
    /// <summary>
    /// Completes the chunked upload process by combining uploaded chunks into a single file
    /// and storing it at the specified final destination path.
    /// </summary>
    /// <param name="uploadId">The unique identifier of the chunked upload session.</param>
    /// <param name="finalObjectPath">The path where the final combined object will be stored.</param>
    /// <returns>A <see cref="PutObjectResponse"/> object representing the result of the upload completion.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="uploadId"/> or <paramref name="finalObjectPath"/> is null, empty, or consists only of whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the upload metadata or chunk information is invalid or incomplete.</exception>
    /// <exception cref="Exception">Thrown if an error occurs during the finalization of the chunked upload.</exception>
    Task<ChankedUploadResponse> CompleteChunkedUploadAsync(string uploadId, string finalObjectPath);

    /// <summary>
    /// Generates a pre-signed URL for accessing a storage object, allowing
    /// temporary access to the object without requiring authentication.
    /// </summary>
    /// <param name="objectPath">The path of the object in storage for which the pre-signed URL is requested. Cannot be null or empty.</param>
    /// <param name="expiryInSeconds">
    /// The expiration time of the pre-signed URL in seconds. Defaults to 3600 seconds (1 hour).
    /// Must be between 1 and 604800 seconds (7 days).
    /// </param>
    /// <returns>A task representing the asynchronous operation. The task result contains the generated pre-signed URL as a string.</returns>
    /// <exception cref="ArgumentException">Thrown if the object path is null, empty, or the expiry duration is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the pre-signed URL cannot be generated for the specified object path.</exception>
    /// <exception cref="MinioException">Thrown if there is an error related to the storage backend.</exception>
    Task<string> GetPresignedUrlAsync(string objectPath, int expiryInSeconds = 3600);

    /// <summary>
    /// Checks whether an object exists at the specified path in the storage system.
    /// </summary>
    /// <param name="objectPath">The path of the object to check in the storage system.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the object exists.</returns>
    Task<bool> IsObjectExistsAsync(string objectPath);

    /// <summary>
    /// Deletes a file from the storage system.
    /// </summary>
    /// <param name="objectPath">The path of the file to be deleted in the storage system.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteFileAsync(string objectPath);

    /// <summary>
    /// Downloads a specific chunk of data from an object stored in the storage service via HTTP.
    /// </summary>
    /// <param name="objectPath">The path of the object in the storage from which the chunk will be downloaded.</param>
    /// <param name="offset">The starting byte position in the object from which to begin downloading.</param>
    /// <param name="length">The number of bytes to download starting from the specified offset.</param>
    /// <returns>A <see cref="Stream"/> containing the requested chunk of data.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="objectPath"/> is null, empty, or consists only of whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> is less than 0 or if <paramref name="length"/> is less than or equal to 0.</exception>
    /// <exception cref="HttpRequestException">Thrown if the HTTP request to the storage service fails.</exception>
    /// <exception cref="Exception">Thrown for any other errors encountered during the retrieval process.</exception>
    Task<Stream> DownloadChunkViaHttpAsync(string objectPath, long offset, long length);

    /// <summary>
    /// Retrieves metadata of the specified file from the storage service.
    /// </summary>
    /// <param name="objectPath">The path in the storage bucket of the file whose metadata is to be retrieved.</param>
    /// <returns>A <see cref="FileMetadata"/> object containing the file's metadata, or null if the file does not exist.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="objectPath"/> is null, empty, or consists only of whitespace.</exception>
    /// <exception cref="Exception">Thrown if an error occurs while retrieving the file metadata.</exception>
    Task<FileMetadata?> GetFileMetadataAsync(string objectPath);
}