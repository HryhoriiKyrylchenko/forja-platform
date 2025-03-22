namespace Forja.Application.Requests.Storage;

public class ProfileHatVariantFileUploadRequest
{
    [Required]
    [Range(1, 5)]
    public short ProfileHatVariantId { get; set; }
    [Required]
    public string FilePath { get; set; } = string.Empty;
}