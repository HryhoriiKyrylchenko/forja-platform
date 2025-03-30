namespace Forja.Application.Validators;

public static class AnalyticsRequestsValidator
{
    public static bool ValidateAnalyticsEventUpdateRequest(AnalyticsEventUpdateRequest request, out string? errors)
    {
        errors = null;
        
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Id == Guid.Empty)
        {
            errors = "The Id field is required and must be valid.";
            return false;
        }

        if (request.UserId != null && request.UserId == Guid.Empty)
        {
            errors = "The UserId field is invalid.";
            return false;
        }

        if (request.Metadata == null)
        {
            errors = "The Metadata field is required.";
            throw new ArgumentNullException(nameof(request.Metadata));
        }

        return true;
    }
}