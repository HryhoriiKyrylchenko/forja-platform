using Forja.Application.DTOs.Support;

namespace Forja.Application.Mapping;

public static class SupportEntityToDtoMapper
{
    public static FAQDto MapToFAQDto(FAQ faq)
    {
        return new FAQDto
        {
            Id = faq.Id,
            Question = faq.Question,
            Answer = faq.Answer,
            Order = faq.Order
        };
    }

    public static SupportTicketDto MapToSupportTicketDto(SupportTicket supportTicket)
    {
        return new SupportTicketDto
        {
            Id = supportTicket.Id,
            UserId = supportTicket.UserId,
            Subject = supportTicket.Subject,
            Description = supportTicket.Description,
            Status = supportTicket.Status.ToString(),
            CreatedAt = supportTicket.CreatedAt,
            UpdatedAt = supportTicket.UpdatedAt
        };
    }

    public static TicketMessageDto MapToTicketMessageDto(TicketMessage ticketMessage)
    {
        return new TicketMessageDto
        {
            Id = ticketMessage.Id,
            SupportTicketId = ticketMessage.SupportTicketId,
            SenderId = ticketMessage.SenderId,
            IsSupportAgent = ticketMessage.IsSupportAgent,
            Message = ticketMessage.Message,
            SentAt = ticketMessage.SentAt
        };
    }
}