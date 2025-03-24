namespace Forja.Application.Validators;

public static class SupportRequestsValidator
{
    public static bool ValidateFAQCreateRequest(FAQCreateRequest request, out string? errorMessage)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            errorMessage = "Question is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.Answer))
        {
            errorMessage = "Answer is required";
            return false;
        }
        
        errorMessage = null;
        return true;
    }
    
    public static bool ValidateFAQUpdateRequest(FAQUpdateRequest request, out string? errorMessage)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Id == Guid.Empty)
        {
            errorMessage = "FAQ ID is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            errorMessage = "Question is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.Answer))
        {
            errorMessage = "Answer is required";
            return false;
        }
        
        errorMessage = null;
        return true;
    }
    
    public static bool ValidateSupportTicketCreateRequest(SupportTicketCreateRequest request, out string? errorMessage)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.UserId == Guid.Empty)
        {
            errorMessage = "User ID is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.Subject))
        {
            errorMessage = "Subject is required";
            return false;
        }

        if (request.Subject.Length > 50)
        {
            errorMessage = "Subject cannot exceed 50 characters";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.Description))
        {
            errorMessage = "Description is required";
            return false;
        }

        if (request.Description.Length > 2000)
        {
            errorMessage = "Description cannot exceed 2000 characters";
            return false;
        }
        
        errorMessage = null;
        return true;
    }
    
    public static bool ValidateSupportTicketUpdateStatusRequest(SupportTicketUpdateStatusRequest statusRequest, out string? errorMessage)
    {
        if (statusRequest == null)
        {
            throw new ArgumentNullException(nameof(statusRequest));
        }
        
        if (statusRequest.Id == Guid.Empty)
        {
            errorMessage = "Ticket ID is required";
            return false;
        }
        
        errorMessage = null;
        return true;
    }
    
    public static bool ValidateTicketMessageCreateRequest(TicketMessageCreateRequest request, out string? errorMessage)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.SupportTicketId == Guid.Empty)
        {
            errorMessage = "Ticket ID is required";
            return false;
        }

        if (request.SenderId == Guid.Empty)
        {
            errorMessage = "Sender ID is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            errorMessage = "Message is required";
            return false;
        }

        if (request.Message.Length > 2000)
        {
            errorMessage = "Message cannot exceed 2000 characters";
            return false;
        }
        
        errorMessage = null;
        return true;
    }
    
    public static bool ValidateTicketMessageUpdateRequest(TicketMessageUpdateRequest request, out string? errorMessage)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Id == Guid.Empty)
        {
            errorMessage = "Message ID is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            errorMessage = "Message is required";
            return false;
        }

        if (request.Message.Length > 2000)
        {
            errorMessage = "Message cannot exceed 2000 characters";
            return false;
        }
        
        errorMessage = null;
        return true;
    }
}