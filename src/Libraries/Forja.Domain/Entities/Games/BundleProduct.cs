namespace Forja.Domain.Entities.Games;

[Table("BundleProducts", Schema = "games")]
public class BundleProduct
{
    [Key]
    [ForeignKey("Bundle")]
    public Guid BundleId { get; set; }
    
    [Key]
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }
    
    public virtual Bundle Bundle { get; set; } = null!;
    
    public virtual Product Product { get; set; } = null!;
}