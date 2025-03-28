namespace Forja.Application.DTOs.Analytics;

public class AnalyticsEventDto
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public DateTime Timestamp { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}