namespace Forja.Application.DTOs.UserProfile;

public class UserLibraryGameExtendedDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required GameLibraryDto Game { get; set; }
    public TimeSpan? TimePlayed { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int TotalGameAchievements { get; set; }
    public List<AchievementShortDto> CompletedAchievements { get; set; } = [];
    public List<UserLibraryAddonDto> Addons { get; set; } = [];
}