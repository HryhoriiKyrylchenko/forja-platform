namespace Forja.Application.DTOs.Games;

/// <summary>
/// Represents the data transfer object for mature content information associated with a product.
/// </summary>
public class ProductMatureContentDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the ProductMatureContent entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product associated with mature content.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the mature content associated with the product.
    /// </summary>
    public Guid MatureContentId { get; set; }
}