namespace Forja.Domain.Entities.Analytics;

[Table("AuditLogs", Schema = "analytics")]
public class AuditLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public AuditEntityType EntityType { get; set; }

    [Required]
    public Guid EntityId { get; set; }

    [Required]
    public AuditActionType ActionType { get; set; }

    [ForeignKey("User")]
    public Guid? UserId { get; set; }

    public DateTime ActionDate { get; set; } = DateTime.Now;
    
    public string Details { get; set; } = string.Empty;
    
    public virtual User User { get; set; } = null!;    
}