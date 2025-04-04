namespace Forja.Application.Requests.Storage;

public class UploadChunkRequest
{
    [Required]
    public string UploadId { get; set; } = string.Empty;
    [Required]
    public int ChunkNumber { get; set; }
    [Required]
    public required IFormFile ChunkFile { get; set; }
    [Required]
    public long ChunkSize { get; set; }
}