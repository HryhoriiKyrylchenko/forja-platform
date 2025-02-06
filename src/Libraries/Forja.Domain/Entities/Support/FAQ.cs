namespace Forja.Domain.Entities.Support;

[Table("FAQs", Schema = "support")]
public class FAQ : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Question { get; set; } = String.Empty;

    [Required]
    public string Answer { get; set; } = String.Empty;

    public int Order { get; set; } = 0;
}