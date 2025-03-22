namespace Forja.Application.Requests.Storage;

public class ImageFileUploadRequest
{
    [Required]
    public string FilePath { get; set; } = string.Empty;
}