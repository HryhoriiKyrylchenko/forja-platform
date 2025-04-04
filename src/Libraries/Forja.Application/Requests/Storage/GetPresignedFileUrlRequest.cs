namespace Forja.Application.Requests.Storage;

public class GetPresignedFileUrlRequest
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public string ObjectPath { get; set; } = string.Empty;
    public int ExpirationInSeconds { get; set; } = 10800;
}