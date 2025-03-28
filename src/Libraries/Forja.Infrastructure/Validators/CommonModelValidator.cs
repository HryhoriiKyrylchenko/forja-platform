namespace Forja.Infrastructure.Validators;

public static class CommonModelValidator
{
    public static bool ValidateLegalDocumentModel(object model, out string? errorMessage)
    {
        if (model is not LegalDocument legalDocument)
        {
            errorMessage = "The provided model is not a valid LegalDocument.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(legalDocument.Title))
        {
            errorMessage = "The Title field is required.";
            return false;
        }

        if (legalDocument.EffectiveDate == default)
        {
            errorMessage = "The EffectiveDate must be a valid date.";
            return false;
        }

        if (legalDocument.FileContent is { Length: 0 })
        {
            errorMessage = "FileContent cannot be an empty binary.";
            return false;
        }

        errorMessage = null;
        return true;
    }
    
    public static bool ValidateNewsArticleModel(object model, out string? errorMessage)
    {
        if (model is not NewsArticle newsArticle)
        {
            errorMessage = "The provided model is not a valid NewsArticle.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(newsArticle.Title))
        {
            errorMessage = "The Title field is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(newsArticle.Content))
        {
            errorMessage = "The Content field is required.";
            return false;
        }

        if (newsArticle.PublicationDate == default)
        {
            errorMessage = "The PublicationDate must be a valid date.";
            return false;
        }

        if ((newsArticle.IsActive || newsArticle.IsPrioritized) && newsArticle.Id == Guid.Empty)
        {
            errorMessage = "The Id must be a valid identifier when the article is active or prioritized.";
            return false;
        }

        if (newsArticle.FileContent is { Length: 0 })
        {
            errorMessage = "FileContent cannot be an empty binary.";
            return false;
        }

        
        if (newsArticle.AuthorId == null && newsArticle.ProductId == null)
        {
            errorMessage = "At least one of AuthorId or ProductId must be provided.";
            return false;
        }

        errorMessage = null;
        return true;
    }
}