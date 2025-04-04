namespace Forja.Application.Requests.Storage;

public class GetPresignedImageUrlRequest
{
    [Required]
    public string ObjectPath { get; set; } = string.Empty;
    public int ExpirationInSeconds { get; set; } = 3600;
}