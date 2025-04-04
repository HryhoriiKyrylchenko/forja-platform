namespace Forja.Application.Interfaces.Games;

public interface IGameVersionService
{
    Task<GameVersionDto?> AddGameVersionAsync(GameVersionCreateRequest request);
    Task<GameVersionDto?> UpdateGameVersionAsync(GameVersionUpdateRequest request);
    Task<GameVersionDto?> GetGameVersionByGameIdAndVersionAsync(Guid gameId, string version);
}