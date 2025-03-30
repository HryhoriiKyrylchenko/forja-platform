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

        if (model.Metadata == null)
        {
            throw new ArgumentNullException(nameof(model.Metadata));
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

        if (model.EndTime != null && model.EndTime < model.StartTime)
        {
            error = "EndTime cannot be earlier than StartTime.";
            return false;
        }

        if (model.Metadata == null)
        {
            throw new ArgumentNullException(nameof(model.Metadata));
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

        if (model.ActionDate > DateTime.UtcNow)
        {
            error = "ActionDate cannot be in the future.";
            return false;
        }

        if (model.Details == null)
        {
            throw new ArgumentNullException(nameof(model.Details));
        }

        return true;
    }
}