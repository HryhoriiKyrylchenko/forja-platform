namespace Forja.Application.Interfaces.Games;

public interface IGameFileService
{
    Task<GameFileDto?> AddGameFile(GameFileCreateRequest request);
    Task<GameFileDto?> UpdateGameFile(GameFileUpdateRequest request);
    Task<GameFileDto?> GetGameFileByGameVersionIdAndFileName(Guid gameVersionId, string fileName);
}