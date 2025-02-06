namespace Forja.Domain.Entities.UserProfile;

[Table("Achievements", Schema = "user-profile")]
public class Achievement : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey("Game")]
    public Guid GameId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public int Points { get; set; }
    
    public string? LogoUrl { get; set; } = string.Empty;
    
    public virtual Game Game { get; set; } = null!;
    
    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = [];
}