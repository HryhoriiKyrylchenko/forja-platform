namespace Forja.Application.Requests.Common;

public class NewsArticleCreateRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublicationDate { get; set; }
    public bool IsPrioritized { get; set; }
    public byte[]? FileContent { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? AuthorId { get; set; }
    public Guid? ProductId { get; set; }
}