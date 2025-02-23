namespace Forja.Application.DTOs.UserProfile;

public class UserLibraryGameDto
{
    public Guid Id { get; set; }
    public Game Game { get; set; } = null!;
    public User User { get; set; } = null!;
    public DateTime PurchaseDate { get; set; }
}