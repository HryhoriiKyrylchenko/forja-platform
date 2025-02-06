namespace Forja.Domain.Entities.Games;

[Table("Bundles", Schema = "games")]
public class Bundle : Product
{
    public virtual ICollection<BundleProduct> BundleProducts { get; set; } = [];
    
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = [];
    
    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
    
    public virtual ICollection<CartItem> CartItems { get; set; } = [];
    
    public virtual ICollection<Discount> Discounts { get; set; } = [];
}