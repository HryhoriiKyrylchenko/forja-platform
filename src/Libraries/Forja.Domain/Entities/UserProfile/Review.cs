namespace Forja.Domain.Entities.UserProfile;

[Table("Reviews", Schema = "user-profile")]
public class Review : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey("User")]
    public Guid UserId { get; set; }
        
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
        
    [Range(1, 5)]
    public int Rating { get; set; }
        
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    public bool IsApproved { get; set; } = false;
    
    public virtual User User { get; set; } = null!;
    
    public virtual Game Game { get; set; } = null!;
}