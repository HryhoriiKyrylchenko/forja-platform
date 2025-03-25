

namespace Forja.Application.Validators;

public static class CommonRequestsValidator
{
    public static bool ValidateLegalDocumentCreateRequest(LegalDocumentCreateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            error = "The Title field is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            error = "The Content field is required.";
            return false;
        }

        if (request.EffectiveDate == default)
        {
            error = "The EffectiveDate must be a valid date.";
            return false;
        }

        if (request.FileContent != null && request.FileContent.Length == 0)
        {
            error = "FileContent cannot be an empty binary.";
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool ValidateLegalDocumentUpdateRequest(LegalDocumentUpdateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Id == Guid.Empty)
        {
            error = "The Id field is required and must be valid.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            error = "The Title field is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            error = "The Content field is required.";
            return false;
        }

        if (request.EffectiveDate == default)
        {
            error = "The EffectiveDate must be a valid date.";
            return false;
        }

        if (request.FileContent != null && request.FileContent.Length == 0)
        {
            error = "FileContent cannot be an empty binary.";
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool ValidateNewsArticleCreateRequest(NewsArticleCreateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            error = "The Title field is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            error = "The Content field is required.";
            return false;
        }

        if (request.PublicationDate == default)
        {
            error = "The PublicationDate must be a valid date.";
            return false;
        }

        if (request.FileContent != null && request.FileContent.Length == 0)
        {
            error = "FileContent cannot be an empty binary.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.ImageUrl))
        {
            error = "The ImageUrl cannot be empty if provided.";
            return false;
        }

        if (request.AuthorId == null && request.ProductId == null)
        {
            error = "At least one of AuthorId or ProductId must be provided.";
            return false;
        }

        error = null;
        return true;
    }
    
    public static bool ValidateNewsArticleUpdateRequest(NewsArticleUpdateRequest request, out string? error)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Id == Guid.Empty)
        {
            error = "The Id field is required and must be valid.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            error = "The Title field is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            error = "The Content field is required.";
            return false;
        }

        if (request.PublicationDate == default)
        {
            error = "The PublicationDate must be a valid date.";
            return false;
        }

        if (request.FileContent != null && request.FileContent.Length == 0)
        {
            error = "FileContent cannot be an empty binary.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.ImageUrl))
        {
            error = "The ImageUrl cannot be empty if provided.";
            return false;
        }

        if (request.ProductId == Guid.Empty)
        {
            error = "If ProductId is provided, it must be a valid GUID.";
            return false;
        }

        if (request.IsActive && request.PublicationDate > DateTime.UtcNow)
        {
            error = "An active NewsArticle cannot have a PublicationDate in the future.";
            return false;
        }

        error = null;
        return true;
    }
}