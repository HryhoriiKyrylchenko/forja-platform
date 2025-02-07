namespace Forja.Domain.Entities.UserProfile;

[Table("UserAchievements", Schema = "user-profile")]
public class UserAchievement
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [ForeignKey("Achievement")]
    public Guid AchievementId { get; set; }
    
    public DateTime AchievedAt { get; set; } = DateTime.Now;

    public virtual User User { get; set; } = null!;

    public virtual Achievement Achievement { get; set; } = null!;
}