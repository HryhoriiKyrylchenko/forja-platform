namespace Forja.Infrastructure.Services;

/// <summary>
/// Provides methods for interacting with storage services for file management.
/// </summary>
public class StorageService : IStorageService, IDisposable
{
    private readonly IDistributedCache _cache;
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public StorageService(string endpoint, string accessKey, string secretKey, string bucketName, IDistributedCache cache, bool useSSL = true)
    {
        _bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
        
        var sanitizedEndpoint = endpoint.Replace("http://", string.Empty).Replace("https://", string.Empty);
        
        _minioClient = new MinioClient()
            .WithEndpoint(sanitizedEndpoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(useSSL)
            .Build();
        
        _cache = cache;
        
        _ = EnsureBucketExistsAsync().ConfigureAwait(false);
    }
    
    /// <inheritdoc />
    public async Task<PutObjectResponse> UploadStreamAsync(string objectPath, Stream dataStream, long objectSize, string contentType)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(objectPath));
        }

        if (dataStream == null)
        {
            throw new ArgumentNullException(nameof(dataStream));
        }

        if (objectSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(objectSize), "Object size must be greater than 0");
        }

        if (string.IsNullOrWhiteSpace(contentType))
        {
            throw new ArgumentException("Content type cannot be null or empty", nameof(contentType));
        }
        
        try
        {
            return await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectPath)
                .WithStreamData(dataStream)
                .WithObjectSize(objectSize)
                .WithContentType(contentType));
        }
        catch (MinioException e)
        {
            throw new Exception($"Error uploading file to MinIO: {e.Message}");
        }
    }

    /// <inheritdoc />
    public async Task<string> StartChunkedUploadAsync(string fileName, long fileSize, int totalChunks, Guid userId, string contentType)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name cannot be null or empty", nameof(fileName));
        }

        if (fileSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(fileSize), "File size must be greater than 0");
        }

        if (totalChunks < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(totalChunks), "Total chunks must be greater than 0");
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(contentType))
        {
            throw new ArgumentException("Content type cannot be null or empty", nameof(contentType));
        }
        
        var uploadId = Guid.NewGuid().ToString();
    
        var metadata = new UploadMetadata
        {
            UploadId = uploadId,
            UserId = userId,
            FileName = fileName,
            FileSize = fileSize,
            UploadedChunks = 0,
            TotalChunks = totalChunks,
            ContentType = contentType
        };

        await _cache.SetStringAsync($"upload:{uploadId}", JsonSerializer.Serialize(metadata), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        });

        return uploadId;
    }

    /// <inheritdoc />
    public async Task<PutObjectResponse> UploadChunkAsync(string uploadId, int partNumber, Stream chunkStream, long chunkSize)
    {
        if (string.IsNullOrWhiteSpace(uploadId))
        {
            throw new ArgumentException("Upload ID cannot be null or empty", nameof(uploadId));
        }

        if (partNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(partNumber), "Part number must be greater than 0");
        }

        if (chunkStream == null)
        {
            throw new ArgumentNullException(nameof(chunkStream));
        }

        if (chunkSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(chunkSize), "Chunk size must be greater than 0");
        }
        
        var metadataJson = await _cache.GetStringAsync($"upload:{uploadId}");
        if (metadataJson == null)
        {
            throw new InvalidOperationException("Upload session not found or expired.");
        }

        var metadata = JsonSerializer.Deserialize<UploadMetadata>(metadataJson);
        if (metadata == null)
        {
            throw new InvalidOperationException("Upload session not found or expired.");
        }

        if (partNumber > metadata.TotalChunks)
        {
            throw new InvalidOperationException($"Chunk {partNumber} is not part of the upload session.");
        }
        
        var chunkPath = $"uploads/{uploadId}/chunk_{partNumber}";
        
        var uploadedChunksJson = await _cache.GetStringAsync($"upload:{uploadId}:chunks");
        var uploadedChunks = uploadedChunksJson != null 
            ? JsonSerializer.Deserialize<HashSet<int>>(uploadedChunksJson) ?? throw new InvalidOperationException("Invalid uploaded chunks list.")
            : new HashSet<int>();

        if (uploadedChunks.Contains(partNumber))
        {
            throw new InvalidOperationException($"Chunk {partNumber} has already been uploaded.");
        }

        if (await IsObjectExistsAsync(chunkPath))
        {
            throw new InvalidOperationException($"Chunk {partNumber} already exists in storage.");
        }

        var result = await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(chunkPath)
            .WithStreamData(chunkStream)
            .WithObjectSize(chunkSize)
            .WithContentType(metadata.ContentType));
        
        uploadedChunks.Add(partNumber);
        await _cache.SetStringAsync($"upload:{uploadId}:chunks", JsonSerializer.Serialize(uploadedChunks),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
        
        metadata.UploadedChunks++;

        await _cache.SetStringAsync($"upload:{uploadId}", JsonSerializer.Serialize(metadata),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
        
        return result;
    }

    /// <inheritdoc />
    public async Task<ChankedUploadResponse> CompleteChunkedUploadAsync(string uploadId, string finalFilePath)
    {
        if (string.IsNullOrWhiteSpace(uploadId))
        {
            throw new ArgumentException("Upload ID cannot be null or empty", nameof(uploadId));
        }

        if (string.IsNullOrWhiteSpace(finalFilePath))
        {
            throw new ArgumentException("Final file path cannot be null or empty", nameof(finalFilePath));
        }
        
        var metadataJson = await _cache.GetStringAsync($"upload:{uploadId}");
        if (metadataJson == null)
        {
            throw new InvalidOperationException("Upload session not found or expired.");
        }

        var metadata = JsonSerializer.Deserialize<UploadMetadata>(metadataJson);
        if (metadata == null)
        {
            throw new InvalidOperationException("Upload session not found or expired.");
        }
        
        var tempPath = $"uploads/{uploadId}/";
        var chunkPaths = await ListChunksAsync(tempPath);
        
        if (chunkPaths.Count != metadata.TotalChunks)
        {
            throw new InvalidOperationException("Not all chunks uploaded yet.");
        }
        
        await using var finalStream = new MemoryStream();
        using var sha256 = SHA256.Create();

        foreach (var chunk in chunkPaths.OrderBy(c => c))
        {
            await using var chunkStream = await DownloadFileAsync(chunk);
            byte[] buffer = new byte[8192];
            int bytesRead;
        
            while ((bytesRead = await chunkStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await finalStream.WriteAsync(buffer, 0, bytesRead);
                sha256.TransformBlock(buffer, 0, bytesRead, null, 0);
            }
        }
        
        sha256.TransformFinalBlock([], 0, 0);
        string fileHash = BitConverter.ToString(sha256.Hash!).Replace("-", "").ToLower();

        finalStream.Position = 0;
        var result = await UploadStreamAsync(finalFilePath, finalStream, finalStream.Length, metadata.ContentType);

        var objectPath = string.Empty;
        
        if (result.ResponseStatusCode == HttpStatusCode.OK)
        {
            foreach (var chunk in chunkPaths)
            {
                await DeleteFileAsync(chunk);
            }
            
            objectPath = finalFilePath;
        }
        
        await _cache.RemoveAsync($"upload:{uploadId}");

        return new ChankedUploadResponse
        {
            ResponseStatusCode = result.ResponseStatusCode,
            ResponseContent = result.ResponseContent,
            ObjectPath = objectPath,
            FileHash = fileHash
        };
    }
    
    /// <inheritdoc />
    public async Task<string> GetPresignedUrlAsync(string objectPath, int expiryInSeconds = 3600)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
        {
            throw new ArgumentException("Object path cannot be null or empty", nameof(objectPath));
        }

        if (expiryInSeconds < 1 || expiryInSeconds > 604800)
        {
            throw new ArgumentOutOfRangeException(nameof(expiryInSeconds), "Expiry must be between 1 second and 7 days (604800 seconds).");
        }
        
        try
        {
            var presignedUrl = await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectPath)
                .WithExpiry(expiryInSeconds)); 

            return presignedUrl;
        }
        catch (MinioException e)
        {
            throw new Exception($"Error generating presigned URL for '{objectPath}': {e.Message}");
        }
    }
    
    /// <inheritdoc />
    public async Task<bool> IsObjectExistsAsync(string objectPath)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
            throw new ArgumentException("File path cannot be null or empty", nameof(objectPath));

        try
        {
            await _minioClient.StatObjectAsync(new StatObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectPath));

            return true; 
        }
        catch (ObjectNotFoundException)
        {
            return false;
        }
        catch (MinioException ex)
        {
            throw new InvalidOperationException($"Error checking existence of file '{objectPath}' in bucket '{_bucketName}': {ex.Message}", ex);
        }
    }

    /// <inheritdoc />
    public async Task DeleteFileAsync(string objectPath)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(objectPath));
        }

        try
        {
            if (!await IsObjectExistsAsync(objectPath))
            {
                throw new FileNotFoundException($"Object '{objectPath}' not found in bucket '{_bucketName}'");
            }
            
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectPath));
        }
        catch (MinioException e)
        {
            throw new Exception($"Error deleting file '{objectPath}' from bucket '{_bucketName}' at path '{objectPath}': {e.Message}");
        }
    }
    
    /// <inheritdoc />
    public void Dispose()
    {
        _minioClient.Dispose();
    }
    
    private async Task<Stream> DownloadFileAsync(string objectPath)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(objectPath));
        }
        
        try
        { 
            var memoryStream = new MemoryStream();
            
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectPath)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream)));
            
            memoryStream.Position = 0;
            return memoryStream;
        }
        catch (MinioException e)
        {
            throw new Exception($"Error downloading file '{objectPath}' from bucket '{_bucketName}' at path '{objectPath}': {e.Message}");
        }
    }
    
    private async Task<List<string>> ListChunksAsync(string pathPrefix)
    {
        var chunks = new List<string>();
        await foreach (var item in _minioClient.ListObjectsEnumAsync(new ListObjectsArgs()
                           .WithBucket(_bucketName)
                           .WithPrefix(pathPrefix)))
        {
            chunks.Add(item.Key);
        }
        return chunks;
    }
    
    private async Task EnsureBucketExistsAsync()
    {
        if (string.IsNullOrWhiteSpace(_bucketName))
            throw new ArgumentException("Bucket name must not be null or empty");

        try
        {
            bool bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!bucketExists)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
            }
        }
        catch (MinioException ex)
        {
            throw new InvalidOperationException($"Error initializing bucket '{_bucketName}': {ex.Message}", ex);
        }
    }
}