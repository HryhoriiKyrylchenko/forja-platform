namespace Forja.Application.DTOs.Common;

public class ProductsFilterDataDto
{
    public List<string> Genres { get; set; } = [];
    public List<string> Mechanics { get; set; } = [];
    public List<string> Tags { get; set; } = [];
    public List<string> MatureContents { get; set; } = [];
}