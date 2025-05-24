namespace Forja.Application.Requests.UserProfile;

public class UserUpdateRequest
{
    public Guid Id { get; set; }

    [MaxLength(30)]
    public string? Username { get; set; }

    [MaxLength(30)]
    public string? Firstname { get; set; }

    [MaxLength(30)]
    public string? Lastname { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? BirthDate { get; set; }

    [MaxLength(10)]
    public string? Gender { get; set; }

    [MaxLength(30)]
    public string? Country { get; set; }

    [MaxLength(30)]
    public string? City { get; set; }

    [MaxLength(500)]
    public string? SelfDescription { get; set; }

    public bool? ShowPersonalInfo { get; set; }

    public string? CustomUrl { get; set; }
    
    [Range(1, 5)]
    public short? ProfileHatVariant { get; set; }
}