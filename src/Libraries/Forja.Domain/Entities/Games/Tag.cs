namespace Forja.Domain.Entities.Games;

[Table("Tags", Schema = "games")]
public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Title { get; set; } = string.Empty;
    
    public virtual ICollection<GameTag> GameTags { get; set; } = [];
}