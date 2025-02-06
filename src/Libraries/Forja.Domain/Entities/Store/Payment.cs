namespace Forja.Domain.Entities.Store;

[Table("Payments", Schema = "store")]
public class Payment : SoftDeletableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey("Order")]
    public Guid OrderId { get; set; }
    
    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.Now;

    [Required]
    public PaymentStatus PaymentStatus { get; set; }
    
    public virtual Order Order { get; set; } = null!;
}