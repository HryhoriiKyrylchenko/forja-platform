using Forja.Domain.Entities.Common;

namespace Forja.Application.Mapping;

public static class CommonEntityToDtoMapper
{
    public static LegalDocumentDto MapToLegalDocumentDto(LegalDocument entity)
    {
        return new LegalDocumentDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            EffectiveDate = entity.EffectiveDate,
            FileContent = entity.FileContent
        };
    }

    public static NewsArticleDto MapToNewsArticleDto(NewsArticle entity)
    {
        return new NewsArticleDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            PublicationDate = entity.PublicationDate,
            IsActive = entity.IsActive,
            IsPrioritized = entity.IsPrioritized,
            CreatedAt = entity.CreatedAt,
            FileContent = entity.FileContent,
            ImageUrl = entity.ImageUrl,
            AuthorId = entity.AuthorId,
            ProductId = entity.ProductId
        };
    }
}