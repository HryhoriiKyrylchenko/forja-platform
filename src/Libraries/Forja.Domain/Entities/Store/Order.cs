namespace Forja.Domain.Entities.Store;

[Table("Orders", Schema = "store")]
public class Order : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey("User")]
    public Guid UserId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    [Required]
    public OrderPaymentStatus PaymentStatus { get; set; }

    public decimal TotalAmount { get; set; }
    
    public virtual Payment? Payment { get; set; }
    
    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
}