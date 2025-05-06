namespace Forja.Application.DTOs.Common
{
    public class ProdShortDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string LogoUrl { get; set; }
        public DiscountDto? ActiveDiscount { get; set; } 
    }
}