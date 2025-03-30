namespace Forja.Application.Requests.Analytics;

public class AnalyticsEventUpdateRequest
{
    public Guid Id { get; set;}
    public AnalyticEventType EventType { get; set;}
    public Guid? UserId { get; set;}
    public Dictionary<string, string> Metadata { get; set;} = [];
}