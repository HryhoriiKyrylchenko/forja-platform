namespace Forja.Infrastructure.Minio;

public class ChankedUploadResponse
{
    public HttpStatusCode ResponseStatusCode { get; set; }
    public string ResponseContent { get; set; } = string.Empty;
    public string ObjectPath { get; set; } = string.Empty;
    public string FileHash { get; set; } = string.Empty;
    public long FileSize { get; set; }
}