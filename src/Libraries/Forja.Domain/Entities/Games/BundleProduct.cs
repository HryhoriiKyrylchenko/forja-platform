namespace Forja.Domain.Entities.Games;

[Table("BundleProducts", Schema = "games")]
public class BundleProduct
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [ForeignKey("Bundle")]
    public Guid BundleId { get; set; }
    
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }
    
    public virtual Bundle Bundle { get; set; } = null!;
    
    public virtual Product Product { get; set; } = null!;
}