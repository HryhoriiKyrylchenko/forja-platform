namespace Forja.Application.DTOs.Games;

public class GameLibraryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public List<GenreDto> Genres { get; set; } = [];
    public List<TagDto> Tags { get; set; } = [];
    public List<MechanicDto> Mechanics { get; set; } = [];
    public List<MatureContentDto> MatureContent { get; set; } = [];
}