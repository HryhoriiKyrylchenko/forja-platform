namespace Forja.Application.DTOs.UserProfile;

public class ReviewDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}