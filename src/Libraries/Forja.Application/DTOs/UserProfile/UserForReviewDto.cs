namespace Forja.Application.DTOs.UserProfile;

public class UserForReviewDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string UserTag { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public int ProductsInLibrary { get; set; }
    public string HatVariantUrl { get; set; } = string.Empty;
    public List<AchievementShortDto> Achievements { get; set; } = [];
}