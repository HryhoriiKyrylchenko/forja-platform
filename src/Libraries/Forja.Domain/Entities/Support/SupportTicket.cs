namespace Forja.Domain.Entities.Support;

[Table("SupportTickets", Schema = "support")]
public class SupportTicket : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Subject { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = String.Empty;

    [Required]
    public SupportTicketStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<TicketMessage> Messages { get; set; } = [];
}