namespace Forja.Infrastructure.Minio;

public class MinioConfiguration
{
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string DefaultBucketName { get; set; } = string.Empty;
    public bool UseSSL { get; set; } 
}
