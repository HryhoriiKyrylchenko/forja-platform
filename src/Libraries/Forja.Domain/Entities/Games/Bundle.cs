namespace Forja.Domain.Entities.Games;

[Table("Bundles", Schema = "games")]
public class Bundle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public decimal TotalPrice { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public bool IsActive { get; set; }
    
    public virtual ICollection<BundleProduct> BundleProducts { get; set; } = [];
}