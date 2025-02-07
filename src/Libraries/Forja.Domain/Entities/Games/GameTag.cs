namespace Forja.Domain.Entities.Games;

[Table("GameTags", Schema = "games")]
public class GameTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
    
    [ForeignKey("Tag")]
    public Guid TagId { get; set; }
    
    public virtual Game Game { get; set; } = null!;
    
    public virtual Tag Tag { get; set; } = null!;
}