namespace Forja.Domain.Entities.Games;

[Table("GameCategories", Schema = "games")]
public class GameCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [ForeignKey("Game")]
    public Guid GameId { get; set; }
    
    [ForeignKey("Category")]
    public Guid CategoryId { get; set; }
    
    public virtual Game Game { get; set; } = null!;
    
    public virtual Category Category { get; set; } = null!;
}