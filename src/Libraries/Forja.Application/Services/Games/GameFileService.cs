namespace Forja.Application.Services.Games;

public class GameFileService : IGameFileService
{
    private readonly IGameFileRepository _gameFileRepository;

    public GameFileService(IGameFileRepository gameFileRepository)
    {
        _gameFileRepository = gameFileRepository;
    }
    
    public async Task<GameFileDto?> AddGameFile(GameFileCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameFileCreateRequest(request, out var errors))
        {
            throw new ArgumentException(errors);
        }
        
        var isArchive = request.FileName.EndsWith(".zip");
            
        var gameFile = new GameFile
        {
            Id = Guid.NewGuid(),
            GameVersionId = request.GameVersionId,
            FileName = request.FileName,
            FilePath = request.FilePath,
            FileSize = request.FileSize,
            Hash = request.Hash,
            IsArchive = isArchive
        };
        var result = await _gameFileRepository.AddAsync(gameFile);
        return result == null ? null : GamesEntityToDtoMapper.MapToGameFileDto(result);
    }

    public async Task<GameFileDto?> UpdateGameFile(GameFileUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameFileUpdateRequest(request, out var errors))
        {
            throw new ArgumentException(errors);
        }
        
        var existingGameFile = await _gameFileRepository.GetByIdAsync(request.Id);
        if (existingGameFile == null)
        {
            throw new KeyNotFoundException($"GameFile with ID {request.Id} not found.");
        }
        
        existingGameFile.FileSize = request.FileSize;
        existingGameFile.Hash = request.Hash;
        
        var result = await _gameFileRepository.UpdateAsync(existingGameFile);
        return result == null ? null : GamesEntityToDtoMapper.MapToGameFileDto(result);
    }

    public async Task<GameFileDto?> GetGameFileByGameVersionIdAndFileName(Guid gameVersionId, string fileName)
    {
        if (gameVersionId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(gameVersionId));
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Invalid fileName.", nameof(fileName));
        }
        
        var result = await _gameFileRepository.GetGameFileByGameVersionIdAndFileName(gameVersionId, fileName);
        return result == null ? null : GamesEntityToDtoMapper.MapToGameFileDto(result);
    }
}