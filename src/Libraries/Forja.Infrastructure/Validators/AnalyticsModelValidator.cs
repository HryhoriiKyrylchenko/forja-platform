namespace Forja.Infrastructure.Validators;

public static class AnalyticsModelValidator
{
    public static bool ValidateAnalyticsAggregateModel(AnalyticsAggregate model, out string? error)
    {
        error = null;

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (string.IsNullOrWhiteSpace(model.MetricName))
        {
            error = "MetricName is required.";
            return false;
        }

        if (model.Value < 0)
        {
            error = "Value cannot be negative.";
            return false;
        }

        return true;
    }
    
    public static bool ValidateAnalyticsEventModel(AnalyticsEvent model, out string? error)
    {
        error = null;

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (model.EventType == default(AnalyticEventType))
        {
            error = "EventType is required.";
            return false;
        }

        if (model.Timestamp == default(DateTime))
        {
            error = "Timestamp is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(model.Metadata))
        {
            error = "Metadata is required.";
            return false;
        }

        return true;
    }
    
    public static bool ValidateAnalyticsSessionModel(AnalyticsSession model, out string? error)
    {
        error = null;

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (model.StartTime == default(DateTime))
        {
            error = "StartTime is required.";
            return false;
        }

        if (model.EndTime != null && model.EndTime < model.StartTime)
        {
            error = "EndTime cannot be earlier than StartTime.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(model.Metadata))
        {
            error = "Metadata is required.";
            return false;
        }

        return true;
    }
    
    public static bool ValidateAuditLogModel(AuditLog model, out string? error)
    {
        error = null;

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (model.EntityType == default(AuditEntityType))
        {
            error = "EntityType is required.";
            return false;
        }

        if (model.EntityId == Guid.Empty)
        {
            error = "EntityId is required.";
            return false;
        }

        if (model.ActionType == default(AuditActionType))
        {
            error = "ActionType is required.";
            return false;
        }

        if (model.ActionDate == default(DateTime))
        {
            error = "ActionDate is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(model.Details))
        {
            error = "Details are required.";
            return false;
        }

        return true;
    }
}