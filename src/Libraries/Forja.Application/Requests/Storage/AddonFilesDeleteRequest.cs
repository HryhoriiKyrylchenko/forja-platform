namespace Forja.Application.Requests.Storage;

public class AddonFilesDeleteRequest
{
    [Required]
    public string SourcePath { get; set; } = string.Empty;
}