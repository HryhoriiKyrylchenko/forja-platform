namespace Forja.Domain.Entities.Games;

[Table("GameTags", Schema = "games")]
public class GameTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [MaxLength(30)]
    public string Tag { get; set; } = string.Empty;
    
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
    
    public virtual Game Game { get; set; } = null!;
}