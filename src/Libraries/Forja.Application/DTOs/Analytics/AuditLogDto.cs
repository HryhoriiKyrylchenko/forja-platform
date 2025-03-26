namespace Forja.Application.DTOs.Analytics;

public class AuditLogDto
{
    public Guid Id { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public DateTime ActionDate { get; set; }
    public string Details { get; set; } = string.Empty;
}