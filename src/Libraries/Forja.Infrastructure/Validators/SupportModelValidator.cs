namespace Forja.Infrastructure.Validators;

public static class SupportModelValidator
{
    public static bool ValidateFAQModel(FAQ faq, out string? errorMessage)
    {
        if (faq == null)
        {
            throw new ArgumentNullException(nameof(faq));
        }

        if (faq.Id == Guid.Empty)
        {
            errorMessage = "FAQ ID is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(faq.Question))
        {
            errorMessage = "FAQ question is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(faq.Answer))
        {
            errorMessage = "FAQ answer is required.";
            return false;
        }
        
        errorMessage = null;
        return true;
    }

    public static bool ValidateSupportTicketModel(SupportTicket supportTicket, out string? errorMessage)
    {
        if (supportTicket == null)
        {
            throw new ArgumentNullException(nameof(supportTicket));
        }

        if (supportTicket.Id == Guid.Empty)
        {
            errorMessage = "Support ticket ID is required.";
            return false;
        }

        if (supportTicket.UserId == Guid.Empty)
        {
            errorMessage = "Support ticket user ID is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(supportTicket.Subject))
        {
            errorMessage = "Support ticket subject is required.";
            return false;
        }

        if (supportTicket.Subject.Length > 50)
        {
            errorMessage = "Support ticket subject must be less than 50 characters.";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(supportTicket.Description))
        {
            errorMessage = "Support ticket description is required.";
            return false;
        }

        if (supportTicket.Description.Length > 1000)
        {
            errorMessage = "Support ticket description must be less than 1000 characters.";
            return false;
        }

        if (supportTicket.CreatedAt > DateTime.UtcNow)
        {
            errorMessage = "Support ticket created at cannot be in the future.";
            return false;
        }

        if (supportTicket.UpdatedAt != null && supportTicket.UpdatedAt > DateTime.UtcNow)
        {
            errorMessage = "Support ticket updated at cannot be in the future.";
            return false;
        }
        
        errorMessage = null;
        return true;
    }

    public static bool ValidateTicketMessageModel(TicketMessage ticketMessage, out string? errorMessage)
    {
        if (ticketMessage == null)
        {
            throw new ArgumentNullException(nameof(ticketMessage));
        }

        if (ticketMessage.Id == Guid.Empty)
        {
            errorMessage = "Ticket message ID is required.";
            return false;
        }

        if (ticketMessage.SupportTicketId == Guid.Empty)
        {
            errorMessage = "Ticket message support ticket ID is required.";
            return false;
        }

        if (ticketMessage.SenderId == Guid.Empty)
        {
            errorMessage = "Ticket message sender ID is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(ticketMessage.Message))
        {
            errorMessage = "Ticket message message is required.";
            return false;
        }

        if (ticketMessage.Message.Length > 2000)
        {
            errorMessage = "Ticket message message must be less than 2000 characters.";
            return false;
        }

        if (ticketMessage.SentAt > DateTime.UtcNow)
        {
            errorMessage = "Ticket message sent at cannot be in the future.";
            return false;
        }
        
        errorMessage = null;
        return true;
    }
}