namespace Forja.Application.Requests.Storage;

public class ImageFileDownloadRequest
{
    [Required]
    public string SourcePath { get; set; } = string.Empty;
    [Required]
    public string DestinationPath { get; set; } = string.Empty;
}