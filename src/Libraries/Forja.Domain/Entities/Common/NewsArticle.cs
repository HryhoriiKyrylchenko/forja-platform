namespace Forja.Domain.Entities.Common;

/// <summary>
/// Represents a news article within the system.
/// </summary>
/// <remarks>
/// This entity contains information about news articles, including title, content, publication details,
/// associated author, and product. It supports optional file attachments and tracks creation time.
/// </remarks>
[Table("NewsArticles", Schema = "common")]
public class NewsArticle
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the news article.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the main content of the news article.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Represents the date and time when the news article was published.
    /// </summary>
    public DateTime PublicationDate { get; set; }

    /// <summary>
    /// Indicates whether the entity is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Indicates whether the entity's identifier holds priority or is given precedence.
    /// </summary>
    public bool IsPrioritized { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    /// <remarks>
    /// This property is automatically initialized to the current UTC date and time when a new instance is created.
    /// </remarks>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Represents the binary content of a file associated with the news article.
    /// </summary>
    /// <remarks>
    /// This property is optional and can store data for files such as images, documents, or other related content
    /// associated with a news article.
    /// </remarks>
    public byte[]? FileContent { get; set; }

    /// <summary>
    /// URL of the image associated with the news article.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Represents the identifier of the author associated with the news article.
    /// This is a foreign key linking to the <see cref="User"/> entity.
    /// </summary>
    [ForeignKey("Author")]
    public Guid? AuthorId { get; set; }

    /// <summary>
    /// Represents an optional foreign key to associate a news article with a specific product.
    /// </summary>
    [ForeignKey("Product")]
    public Guid? ProductId { get; set; }

    /// <summary>
    /// Represents the author of the news article.
    /// </summary>
    /// <remarks>
    /// This property establishes a relationship between the news article and the user
    /// who created or is associated with the article.
    /// The author is optional and may not always be assigned.
    /// </remarks>
    public virtual User? Author { get; set; }

    /// <summary>
    /// Represents an abstract base class for products within the games domain.
    /// </summary>
    /// <remarks>
    /// A product includes detailed information such as titles, descriptions, pricing, release dates, and more.
    /// It also supports relationships with various entities including genres, discounts, orders, and user-related data.
    /// </remarks>
    public virtual Product? Product { get; set; }
}