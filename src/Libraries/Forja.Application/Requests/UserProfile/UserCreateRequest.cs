namespace Forja.Application.Requests.UserProfile;

public class UserCreateRequest
{
    [Required]
    public string KeycloakUserId { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Username { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Firstname { get; set; }

    [MaxLength(30)]
    public string? Lastname { get; set; }

    [Required]
    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string? AvatarUrl { get; set; }

    public DateTime? BirthDate { get; set; }

    [MaxLength(10)]
    public string? Gender { get; set; }

    [MaxLength(30)]
    public string? Country { get; set; }

    [MaxLength(30)]
    public string? City { get; set; }

    [MaxLength(500)]
    public string? SelfDescription { get; set; }

    public bool ShowPersonalInfo { get; set; } = false;

    [Required]
    public DateTime CreatedAt { get; set; }

    public string? CustomUrl { get; set; }
}