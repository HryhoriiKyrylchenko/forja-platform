namespace Forja.Application.Requests.Common;

public class LegalDocumentCreateRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public byte[]? FileContent { get; set; }
}