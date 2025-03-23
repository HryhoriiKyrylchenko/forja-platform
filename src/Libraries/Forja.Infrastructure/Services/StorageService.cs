using Minio.ApiEndpoints;

namespace Forja.Infrastructure.Services;

public class StorageService : IStorageService, IDisposable
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public StorageService(string endpoint, string accessKey, string secretKey, string bucketName, bool useSSL = true)
    {
        _bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
        
        var sanitizedEndpoint = endpoint.Replace("http://", string.Empty).Replace("https://", string.Empty);
        
        _minioClient = new MinioClient()
            .WithEndpoint(sanitizedEndpoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(useSSL)
            .Build();
        
        _ = EnsureBucketExistsAsync().ConfigureAwait(false);
    }

    private async Task EnsureBucketExistsAsync()
    {
        if (string.IsNullOrWhiteSpace(_bucketName))
            throw new ArgumentException("Bucket name must not be null or empty");

        try
        {
            bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!found)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
            }
        }
        catch (MinioException e)
        {
            throw new Exception($"Error initializing bucket '{_bucketName}': {e.Message}");
        }
    }
    
    public async Task<bool> FileExistsAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        try
        {
            await _minioClient.StatObjectAsync(new StatObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(filePath));

            return true; 
        }
        catch (ObjectNotFoundException)
        {
            return false;
        }
        catch (MinioException ex)
        {
            throw new InvalidOperationException($"Error checking existence of file: {filePath}", ex);
        }
    }

    public async Task<bool> FolderExistsAsync(string folderPath)
    {
        try
        {
            var sanitizedFolderPath = folderPath.TrimEnd('/') + "/";
        
            var objects = _minioClient.ListObjectsEnumAsync(new ListObjectsArgs()
                .WithBucket(_bucketName)
                .WithPrefix(sanitizedFolderPath)
                .WithRecursive(false));

            await foreach (var obj in objects)
            {
                if (!string.IsNullOrEmpty(obj.Key))
                {
                    return true;
                }
            }

            return false; 
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error checking existence of folder: {folderPath}", ex);
        }
    }

    public async Task UploadFileAsync(string destinationObjectPath, string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File '{filePath}' not found");
        }

        if (string.IsNullOrWhiteSpace(destinationObjectPath))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(destinationObjectPath));
        }
        
        try
        {
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(destinationObjectPath)
                .WithFileName(filePath)
                .WithContentType("application/octet-stream"));
        }
        catch (MinioException e)
        {
            throw new Exception($"Error uploading file '{filePath}' to bucket '{_bucketName}' at path '{destinationObjectPath}': {e.Message}");
        }
    }

    public async Task DownloadFileAsync(string objectPath, string downloadFilePath)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(objectPath));
        }
        
        try
        {
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectPath)
                .WithFile(downloadFilePath));
        }
        catch (MinioException e)
        {
            throw new Exception($"Error downloading file '{objectPath}' from bucket '{_bucketName}' at path '{objectPath}': {e.Message}");
        }
    }

    public async Task DeleteFileAsync(string objectPath)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(objectPath));
        }

        try
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectPath));
        }
        catch (MinioException e)
        {
            throw new Exception($"Error deleting file '{objectPath}' from bucket '{_bucketName}' at path '{objectPath}': {e.Message}");
        }
    }
    
    public async Task UploadFolderAsync(string destinationPath, string folderPath)
    {
        if (!Directory.Exists(folderPath)) throw new DirectoryNotFoundException($"Folder '{folderPath}' not found");
        
        var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            string relativePath = Path.GetRelativePath(folderPath, file).Replace("\\", "/");
            string objectName = $"{destinationPath}{relativePath}";
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithFileName(file)
                .WithContentType("application/octet-stream"));
        }
    }

    public async Task DownloadFolderAsync(string sourcePath, string destinationFolderPath)
    {
        if (!Directory.Exists(destinationFolderPath)) Directory.CreateDirectory(destinationFolderPath);

        var objects = _minioClient.ListObjectsEnumAsync(new ListObjectsArgs()
            .WithBucket(_bucketName)
            .WithPrefix(sourcePath)
            .WithRecursive(true));

        await foreach (var obj in objects)
        {
            string objectName = obj.Key;
            string relativePath = objectName.Substring(sourcePath.Length);
            string localFilePath = Path.Combine(destinationFolderPath, relativePath) ?? throw new Exception("Invalid path");
            string localDirectory = Path.GetDirectoryName(localFilePath) ?? throw new Exception("Invalid path");
            if (!Directory.Exists(localDirectory)) Directory.CreateDirectory(localDirectory);
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithFile(localFilePath));
        }
    }
    
    public async Task DeleteFolderAsync(string folderPath)
    {
        if (string.IsNullOrWhiteSpace(folderPath))
        {
            throw new ArgumentException("Folder path cannot be null or empty", nameof(folderPath));
        }

        try
        {
            var objects = _minioClient.ListObjectsEnumAsync(new ListObjectsArgs()
                .WithBucket(_bucketName)
                .WithPrefix(folderPath)
                .WithRecursive(true));

            var objectNames = new List<string>();
            await foreach (var obj in objects)
            {
                objectNames.Add(obj.Key);
            }
            
            if (objectNames.Count > 0)
            {
                await _minioClient.RemoveObjectsAsync(new RemoveObjectsArgs()
                    .WithBucket(_bucketName)
                    .WithObjects(objectNames));
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Error deleting folder '{folderPath}' from bucket '{_bucketName}': {e.Message}");
        }
    }
    
    public async Task<string> GetPresignedUrlAsync(string objectPath, int expiryInSeconds = 3600)
    {
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
    
    public void Dispose()
    {
        _minioClient.Dispose();
    }
}