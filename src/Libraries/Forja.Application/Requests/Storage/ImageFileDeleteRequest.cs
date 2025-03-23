namespace Forja.Application.Requests.Storage;

public class ImageFileDeleteRequest
{
    [Required]
    public string SourcePath { get; set; } = string.Empty;
}