namespace Forja.Domain.Entities.Games;

[Table("Categories", Schema = "games")]
public class Category : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<GameCategory> GameCategories { get; set; } = [];
}