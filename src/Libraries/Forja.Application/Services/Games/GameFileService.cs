namespace Forja.Application.Services.Games;

public class GameFileService : IGameFileService
{
    private readonly IProductFileRepository _productFileRepository;

    public GameFileService(IProductFileRepository productFileRepository)
    {
        _productFileRepository = productFileRepository;
    }
    
    ///<inheritdoc/>
    public async Task<ProductFileDto?> AddGameFile(GameFileCreateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameFileCreateRequest(request, out var errors))
        {
            throw new ArgumentException(errors);
        }
        
        var isArchive = request.FileName.EndsWith(".zip");
            
        var gameFile = new ProductFile
        {
            Id = Guid.NewGuid(),
            ProductVersionId = request.GameVersionId,
            FileName = request.FileName,
            FilePath = request.FilePath,
            FileSize = request.FileSize,
            Hash = request.Hash,
            IsArchive = isArchive,
            StorageUrl = request.StorageUrl
        };
        var result = await _productFileRepository.AddAsync(gameFile);
        return result == null ? null : GamesEntityToDtoMapper.MapToProductFileDto(result);
    }

    ///<inheritdoc/>
    public async Task<ProductFileDto?> UpdateGameFile(GameFileUpdateRequest request)
    {
        if (!GamesRequestsValidator.ValidateGameFileUpdateRequest(request, out var errors))
        {
            throw new ArgumentException(errors);
        }
        
        var existingGameFile = await _productFileRepository.GetByIdAsync(request.Id);
        if (existingGameFile == null)
        {
            throw new KeyNotFoundException($"GameFile with ID {request.Id} not found.");
        }
        
        existingGameFile.FileSize = request.FileSize;
        existingGameFile.Hash = request.Hash;
        existingGameFile.StorageUrl = request.StorageUrl;
        
        var result = await _productFileRepository.UpdateAsync(existingGameFile);
        return result == null ? null : GamesEntityToDtoMapper.MapToProductFileDto(result);
    }

    ///<inheritdoc/>
    public async Task<ProductFileDto?> GetGameFileByGameVersionIdAndFileName(Guid gameVersionId, string fileName)
    {
        if (gameVersionId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(gameVersionId));
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Invalid fileName.", nameof(fileName));
        }
        
        var result = await _productFileRepository.GetGameFileByGameVersionIdAndFileName(gameVersionId, fileName);
        return result == null ? null : GamesEntityToDtoMapper.MapToProductFileDto(result);
    }
}