namespace Forja.Application.DTOs.UserProfile;

public class UserLibraryGameDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public string GameLogoUrl { get; set; } = string.Empty;
    public TimeSpan? TimePlayed { get; set; }
    public DateTime PurchaseDate { get; set; }
}