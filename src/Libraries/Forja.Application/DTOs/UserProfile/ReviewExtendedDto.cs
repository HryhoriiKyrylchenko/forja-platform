namespace Forja.Application.DTOs.UserProfile;

public class ReviewExtendedDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public bool PositiveRating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UserForReviewDto User { get; set; } = null!;
}