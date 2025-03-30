namespace Forja.Application.Mapping;

public static class AnalyticsEntityToDtoMapper
{
    public static AnalyticsAggregateDto MapToAnalyticsAggregateDto(AnalyticsAggregate analyticsAggregate)
    {
        return new AnalyticsAggregateDto
        {
            Id = analyticsAggregate.Id,
            Date = analyticsAggregate.Date,
            MetricName = analyticsAggregate.MetricName,
            Value = analyticsAggregate.Value
        };
    }

    public static AnalyticsEventDto MapToAnalyticsEventDto(AnalyticsEvent analyticsEvent)
    {
        return new AnalyticsEventDto
        {
            Id = analyticsEvent.Id,
            EventType = analyticsEvent.EventType.ToString(),
            UserId = analyticsEvent.UserId,
            Timestamp = analyticsEvent.Timestamp,
            Metadata = analyticsEvent.Metadata
        };
    }

    public static AnalyticsSessionDto MapToAnalyticsSessionDto(AnalyticsSession analyticsSession)
    {
        return new AnalyticsSessionDto
        {
            SessionId = analyticsSession.SessionId,
            UserId = analyticsSession.UserId,
            StartTime = analyticsSession.StartTime,
            EndTime = analyticsSession.EndTime,
            Metadata = analyticsSession.Metadata
        };
    }

    public static AuditLogDto MapToAuditLogDto(AuditLog auditLog)
    {
        return new AuditLogDto
        {
            Id = auditLog.Id,
            EntityType = auditLog.EntityType.ToString(),
            EntityId = auditLog.EntityId,
            ActionType = auditLog.ActionType.ToString(),
            UserId = auditLog.UserId,
            ActionDate = auditLog.ActionDate,
            Details = auditLog.Details
        };
    }
}