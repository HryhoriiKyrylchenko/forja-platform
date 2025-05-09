namespace Forja.Application.Services.Storage;

public class FileManagerService : IFileManagerService
{
    private readonly IStorageService _storageService;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IAchievementRepository _achievementRepository;
    private readonly IMatureContentRepository _matureContentRepository;
    private readonly IMechanicRepository _mechanicRepository;
    private readonly IProductImagesRepository _productImagesRepository;
    private readonly INewsArticleRepository _newsArticleRepository;
    private readonly IProductVersionRepository _productVersionRepository;
    private readonly IProductFileRepository _productFileRepository;
    private readonly IGamePatchRepository _gamePatchRepository;

    public FileManagerService(IStorageService storageService,
        IUserRepository userRepository,
        IProductRepository productRepository,
        IAchievementRepository achievementRepository,
        IMatureContentRepository matureContentRepository,
        IMechanicRepository mechanicRepository,
        IProductImagesRepository productImagesRepository,
        INewsArticleRepository newsArticleRepository,
        IProductVersionRepository productVersionRepository,
        IProductFileRepository productFileRepository,
        IGamePatchRepository gamePatchRepository)
    {
        _storageService = storageService;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _achievementRepository = achievementRepository;
        _matureContentRepository = matureContentRepository;
        _mechanicRepository = mechanicRepository;
        _productImagesRepository = productImagesRepository;
        _newsArticleRepository = newsArticleRepository;
        _productVersionRepository = productVersionRepository;
        _productFileRepository = productFileRepository;
        _gamePatchRepository = gamePatchRepository;
    }

    /// <inheritdoc />
    public async Task<string> StartChunkedUploadAsync(StartChunkedUploadRequest request)
    {
        if (!StorageRequestsValidator.ValidateStartChunkedUploadRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        var uploadId = await _storageService.StartChunkedUploadAsync(request.FileName, request.FileSize, request.TotalChunks, request.UserId, request.ContentType);
        return uploadId;
    }

    /// <inheritdoc />
    public async Task<HttpStatusCode> UploadChunkAsync(UploadChunkRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadChunkRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (request.ChunkFile == null)
        {
            throw new ArgumentException("Chunk stream cannot be null.", nameof(request.ChunkFile));
        }
        
        await using var stream = request.ChunkFile.OpenReadStream();
        
        var result = await _storageService.UploadChunkAsync(request.UploadId, request.ChunkNumber, stream, request.ChunkSize);
        return result.ResponseStatusCode;
    }

    /// <inheritdoc />
    public async Task<ChankedUploadResponse> CompleteChunkedUploadAsync(CompleteChunkedUploadRequest request)
    {
        if (!StorageRequestsValidator.ValidateCompleteChunkedUploadRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        string destinationPath = request.FileType switch
        {
            FileType.FullGame => $"games/{request.GameId}/{request.Platform.ToString().ToLowerInvariant()}/versions/{request.Version}/{request.FinalFileName}",
            FileType.GameFile => $"games/{request.GameId}/{request.Platform.ToString().ToLowerInvariant()}/versions/{request.Version}/files/{request.FinalFileName}",
            FileType.GameAddon => $"games/{request.GameId}/{request.Platform.ToString().ToLowerInvariant()}/addons/{request.AddonId}/versions/{request.Version}/{request.FinalFileName}",
            FileType.AddonFile => $"games/{request.GameId}/{request.Platform.ToString().ToLowerInvariant()}/addons/{request.AddonId}/versions/{request.Version}/files/{request.FinalFileName}",
            FileType.GamePatch => $"games/{request.GameId}/{request.Platform.ToString().ToLowerInvariant()}/patches/{request.FromVersion}-{request.ToVersion}/{request.FinalFileName}",
            _ => throw new ArgumentOutOfRangeException(nameof(request.FileType), request.FileType, "Invalid file type")
        };

        var result = await _storageService.CompleteChunkedUploadAsync(request.UploadId, destinationPath);
        if (result.ResponseStatusCode == HttpStatusCode.OK)
        {
            switch (request.FileType)
            {
                case FileType.FullGame:
                    var gameVersion = await _productVersionRepository.GetByProductIdPlatformAndVersionAsync(request.GameId, request.Platform, request.Version);
                    if (gameVersion != null)
                    {
                        gameVersion.StorageUrl = result.ObjectPath;
                        gameVersion.FileSize = result.FileSize;
                        gameVersion.Hash = result.FileHash;
                        var updatedVersion = await _productVersionRepository.UpdateAsync(gameVersion);
                        if (updatedVersion == null)
                        {
                            throw new InvalidOperationException("Failed to update game version.");
                        }
                    }
                    else
                    {
                        var newGameVersion = new ProductVersion
                        {
                            Id = Guid.NewGuid(),
                            ProductId = request.GameId,
                            Platform = request.Platform,
                            Version = request.Version,
                            StorageUrl = result.ObjectPath,
                            FileSize = result.FileSize,
                            Hash = result.FileHash,
                            Changelog = request.ChangeLog,
                            ReleaseDate = request.ReleaseDate ?? DateTime.UtcNow
                        };
                        var versionResult = await _productVersionRepository.AddAsync(newGameVersion);
                        if (versionResult == null)
                        {
                            await _storageService.DeleteFileAsync(result.ObjectPath);
                            throw new InvalidOperationException("Failed to add game version.");
                        }
                    }
                    break;
                case FileType.GameFile:
                    if (request.FilePath == null)
                    {
                        throw new ArgumentException("FilePath cannot be null.", nameof(request.FilePath));
                    }
                    var gameFile = await _productFileRepository.FindByPlatformVersionProductIdAndNameAsync(request.Platform, request.Version, request.GameId, request.FinalFileName);
                    if (gameFile != null)
                    {
                        gameFile.FileName = request.FinalFileName;
                        gameFile.FilePath = request.FilePath;
                        gameFile.StorageUrl = result.ObjectPath;
                        gameFile.FileSize = result.FileSize;
                        gameFile.Hash = result.FileHash;
                        await _productFileRepository.UpdateAsync(gameFile);
                    }
                    else
                    {
                        var gameVersionForFile = await _productVersionRepository.GetByProductIdPlatformAndVersionAsync(request.GameId, request.Platform, request.Version);
                        if (gameVersionForFile == null)
                        {
                            var newGameVersionForFile = new ProductVersion
                            {
                                Id = Guid.NewGuid(),
                                ProductId = request.GameId,
                                Platform = request.Platform,
                                Version = request.Version,
                                StorageUrl = result.ObjectPath,
                                FileSize = result.FileSize,
                                Hash = result.FileHash,
                                Changelog = request.ChangeLog,
                                ReleaseDate = request.ReleaseDate ?? DateTime.UtcNow
                            };
                            gameVersionForFile = await _productVersionRepository.AddAsync(newGameVersionForFile);
                            if (gameVersionForFile == null)
                            {
                                throw new InvalidOperationException("Failed to add game version.");
                            }
                        }
                        
                        var isArchive = request.FinalFileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);
                        var newGameFile = new ProductFile
                        {
                            Id = Guid.NewGuid(),
                            ProductVersionId = gameVersionForFile.Id,
                            FileName = request.FinalFileName,
                            FilePath = request.FilePath,
                            FileSize = result.FileSize,
                            Hash = result.FileHash,
                            StorageUrl = result.ObjectPath,
                            IsArchive = isArchive
                        };
                        
                        var fileResult = await _productFileRepository.AddAsync(newGameFile);
                        if (fileResult == null)
                        {
                            await _storageService.DeleteFileAsync(result.ObjectPath);
                            throw new InvalidOperationException("Failed to add game file.");
                        }
                    }
                    break;
                case FileType.GameAddon:
                    if (request.AddonId == null || request.AddonId == Guid.Empty)
                    {
                        throw new ArgumentException("AddonId cannot be null or empty.", nameof(request.AddonId));
                    }
                    var addonVersion = await _productVersionRepository.GetByProductIdPlatformAndVersionAsync((Guid)request.AddonId, request.Platform, request.Version);
                    if (addonVersion != null)
                    {
                        addonVersion.StorageUrl = result.ObjectPath;
                        addonVersion.FileSize = result.FileSize;
                        addonVersion.Hash = result.FileHash;
                        var updatedVersion = await _productVersionRepository.UpdateAsync(addonVersion);
                        if (updatedVersion == null)
                        {
                            throw new InvalidOperationException("Failed to update game version.");
                        }
                    }
                    else
                    {
                        var newAddonVersion = new ProductVersion
                        {
                            Id = Guid.NewGuid(),
                            ProductId = (Guid)request.AddonId,
                            Platform = request.Platform,
                            Version = request.Version,
                            StorageUrl = result.ObjectPath,
                            FileSize = result.FileSize,
                            Hash = result.FileHash,
                            Changelog = request.ChangeLog,
                            ReleaseDate = request.ReleaseDate ?? DateTime.UtcNow
                        };
                        var versionResult = await _productVersionRepository.AddAsync(newAddonVersion);
                        if (versionResult == null)
                        {
                            await _storageService.DeleteFileAsync(result.ObjectPath);
                            throw new InvalidOperationException("Failed to add game version.");
                        }
                    }
                    break;
                case FileType.AddonFile:
                    if (request.AddonId == null || request.FilePath == null)
                    {
                        throw new ArgumentException("AddonId and FilePath cannot be null or empty.", nameof(request.AddonId));
                    }
                    var addonFile = await _productFileRepository.FindByPlatformVersionProductIdAndNameAsync(request.Platform, request.Version, (Guid)request.AddonId, request.FinalFileName);
                    if (addonFile != null)
                    {
                        addonFile.FileName = request.FinalFileName;
                        addonFile.FilePath = request.FilePath;
                        addonFile.StorageUrl = result.ObjectPath;
                        addonFile.FileSize = result.FileSize;
                        addonFile.Hash = result.FileHash;
                        await _productFileRepository.UpdateAsync(addonFile);
                    }
                    else
                    {
                        var addonVersionForFile = await _productVersionRepository.GetByProductIdPlatformAndVersionAsync((Guid)request.AddonId, request.Platform, request.Version);
                        if (addonVersionForFile == null)
                        {
                            var newAddonVersionForFile = new ProductVersion
                            {
                                Id = Guid.NewGuid(),
                                ProductId = (Guid)request.AddonId,
                                Platform = request.Platform,
                                Version = request.Version,
                                StorageUrl = result.ObjectPath,
                                FileSize = result.FileSize,
                                Hash = result.FileHash,
                                Changelog = request.ChangeLog,
                                ReleaseDate = request.ReleaseDate ?? DateTime.UtcNow
                            };
                            addonVersionForFile = await _productVersionRepository.AddAsync(newAddonVersionForFile);
                            if (addonVersionForFile == null)
                            {
                                throw new InvalidOperationException("Failed to add addon version.");
                            }
                        }
                        
                        var isArchive = request.FinalFileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);
                        var newAddonFile = new ProductFile
                        {
                            Id = Guid.NewGuid(),
                            ProductVersionId = addonVersionForFile.Id,
                            FileName = request.FinalFileName,
                            FilePath = request.FilePath,
                            FileSize = result.FileSize,
                            Hash = result.FileHash,
                            StorageUrl = result.ObjectPath,
                            IsArchive = isArchive
                        };
                        
                        var fileResult = await _productFileRepository.AddAsync(newAddonFile);
                        if (fileResult == null)
                        {
                            await _storageService.DeleteFileAsync(result.ObjectPath);
                            throw new InvalidOperationException("Failed to add addon file.");
                        }
                    }
                    break;
                case FileType.GamePatch:
                    if (string.IsNullOrWhiteSpace(request.FromVersion) || string.IsNullOrWhiteSpace(request.ToVersion))
                    {
                        throw new ArgumentException("FromVersion and ToVersion data should not be null when creating patch");
                    }
                    var gamePatch = await _gamePatchRepository.GetByGameIdPlatformAndVersionsAsync(request.GameId, request.Platform, request.FromVersion, request.ToVersion);
                    if (gamePatch != null)
                    {
                        gamePatch.PatchUrl = result.ObjectPath;
                        gamePatch.FileSize = result.FileSize;
                        gamePatch.Hash = result.FileHash;
                        gamePatch.ReleaseDate = request.ReleaseDate ?? DateTime.UtcNow;
                        var updatedPatch = await _gamePatchRepository.UpdateAsync(gamePatch);
                        if (updatedPatch == null)
                        {
                            await _storageService.DeleteFileAsync(result.ObjectPath);
                            throw new InvalidOperationException("Failed to update game patch.");
                        }
                    }
                    else
                    {
                        var newGamePatch = new GamePatch
                        {
                            Id = Guid.NewGuid(),
                            GameId = request.GameId,
                            Platform = request.Platform,
                            Name = $"{request.FromVersion}-{request.ToVersion}",
                            FromVersion = request.FromVersion,
                            ToVersion = request.ToVersion,
                            PatchUrl = result.ObjectPath,
                            FileSize = result.FileSize,
                            Hash = result.FileHash,
                            ReleaseDate = request.ReleaseDate ?? DateTime.UtcNow
                        };
                        var addedPatch = await _gamePatchRepository.AddAsync(newGamePatch);
                        if (addedPatch == null)
                        {
                            throw new InvalidOperationException("Failed to add game patch.");
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request.FileType), request.FileType, "Invalid file type");
            }
        }
            
        return result;
    }

    /// <inheritdoc />
    public async Task DeleteGameFileAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        if (!request.ObjectPath.Contains("games"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }

    /// <inheritdoc />
    public async Task<string> UploadProductLogoAsync(UploadObjectImageRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadObjectImageRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (request.File == null)
        {
            throw new ArgumentException("File cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/product/logo/";
        string extension = Path.GetExtension(request.FileName);
        string filenameWithoutExtension = $"product-logo_{request.ObjectId}";
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}{extension}";
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.ObjectSize, request.ContentType);
        return destinationFilePath;
    }

    /// <inheritdoc />
    public async Task DeleteProductLogoAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (!request.ObjectPath.Contains("images/product/logo/product-logo"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }
    
    /// <inheritdoc />
    public async Task<string> UploadProductImageAsync(UploadImageRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadImageRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        if (request.File == null)
        {
            throw new ArgumentException("File cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/product/album/";
        string extension = Path.GetExtension(request.FileName);
        string uniqueSuffix = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string filenameWithoutExtension = "product-image";
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}-{uniqueSuffix}{extension}";
        
        destinationFilePath = await EnsureUniqueObjectPathAsync(destinationFilePath);
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.ObjectSize, request.ContentType);
        return destinationFilePath;
    }

    /// <inheritdoc />
    public async Task DeleteProductImageAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (!request.ObjectPath.Contains("images/product/album/product-image"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }

    /// <inheritdoc />
    public async Task<string> UploadUserAvatarAsync(UploadObjectImageRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadObjectImageRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        if (request.File == null)
        {
            throw new ArgumentException("Stream cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/user/avatars/";
        string extension = Path.GetExtension(request.FileName);
        string filenameWithoutExtension = $"user-avatar_{request.ObjectId}";
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}{extension}";
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.ObjectSize, request.ContentType);
        return destinationFilePath;
    }

    /// <inheritdoc />
    public async Task DeleteUserAvatarAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (!request.ObjectPath.Contains("images/user/avatars/user-avatar"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }

    ///<inheritdoc/>
    public async Task<string> UploadAchievementImageAsync(UploadObjectImageRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadObjectImageRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        if (request.File == null)
        {
            throw new ArgumentException("Stream cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/achievements/";
        string extension = Path.GetExtension(request.FileName);
        string filenameWithoutExtension = $"achievement_{request.ObjectId}";
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}{extension}";
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.ObjectSize, request.ContentType);
        return destinationFilePath;
    }

    ///<inheritdoc/>
    public async Task DeleteAchievementImageAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (!request.ObjectPath.Contains("images/achievements/"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }

    ///<inheritdoc/>
    public async Task<string> UploadMatureContentImageAsync(UploadObjectImageRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadObjectImageRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        if (request.File == null)
        {
            throw new ArgumentException("Stream cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/mature-content/";
        string extension = Path.GetExtension(request.FileName);
        string filenameWithoutExtension = $"mature-content_{request.ObjectId}";
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}{extension}";
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.ObjectSize, request.ContentType);
        return destinationFilePath;
    }

    ///<inheritdoc/>
    public async Task DeleteMatureContentImageAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (!request.ObjectPath.Contains("images/mature-content/"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }

    ///<inheritdoc/>
    public async Task<string> UploadMechanicImageAsync(UploadObjectImageRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadObjectImageRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        if (request.File == null)
        {
            throw new ArgumentException("Stream cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/mechanics/";
        string extension = Path.GetExtension(request.FileName);
        string filenameWithoutExtension = $"mechanic_{request.ObjectId}";
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}{extension}";
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.ObjectSize, request.ContentType);
        return destinationFilePath;
    }

    ///<inheritdoc/>
    public async Task DeleteMechanicImageAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (!request.ObjectPath.Contains("images/mechanics/"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }

    ///<inheritdoc/>
    public async Task<string> UploadNewsArticleImageAsync(UploadObjectImageRequest request)
    {
        if (!StorageRequestsValidator.ValidateUploadObjectImageRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        if (request.File == null)
        {
            throw new ArgumentException("Stream cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/news-articles/";
        string extension = Path.GetExtension(request.FileName);
        string filenameWithoutExtension = $"news-article_{request.ObjectId}";
        string destinationFilePath = $"{destinationPath}{filenameWithoutExtension}{extension}";
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.ObjectSize, request.ContentType);
        return destinationFilePath;
    }

    ///<inheritdoc/>
    public async Task DeleteNewsArticleImageAsync(DeleteObjectRequest request)
    {
        if (!StorageRequestsValidator.ValidateDeleteObjectRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (!request.ObjectPath.Contains("images/news-articles/"))
        {
            throw new ArgumentException("Wrong object path", nameof(request.ObjectPath));
        }
        
        await _storageService.DeleteFileAsync(request.ObjectPath);
    }

    /// <inheritdoc />
    public async Task<string> GetPresignedUrlAsync(string objectPath, int expiresInSeconds = 3600)
    {
        if (expiresInSeconds <= 10)
        {
            throw new ArgumentException("Expiration time cannot be less than 10 seconds.", nameof(expiresInSeconds));
        }
        
        var result = await _storageService.GetPresignedUrlAsync(objectPath, expiresInSeconds);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for object path: {objectPath}");
        }
        
        return result;
    }
    
    /// <inheritdoc />
    public async Task<string> UploadProfileHatVariantFileAsync(ProfileHatVariantFileUploadRequest request)
    {
        if (!StorageRequestsValidator.ValidateProfileHatVariantFileUploadRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        if (request.File == null)
        {
            throw new ArgumentException("Stream cannot be null.", nameof(request.File));
        }

        string destinationPath = "images/user/profile-hat-variants/";
        string extension = Path.GetExtension(request.FileName);
        string destinationFilePath = $"{destinationPath}{request.ProfileHatVariantId}{extension}";
        
        await using var stream = request.File.OpenReadStream();
        
        await UploadStreamAsync(destinationFilePath, stream, request.FileSize, request.ContentType);
        return destinationFilePath;
    }

    /// <inheritdoc />
    public async Task DeleteProfileHatVariantFileAsync(ProfileHatVariantFileDeleteRequest request)
    {
        if (!StorageRequestsValidator.ValidateProfileHatVariantFileDeleteRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }

        var variantPath = await FindProfileHatVariantPathAsync(request.ProfileHatVariantId);
        if (string.IsNullOrWhiteSpace(variantPath))
        {
            throw new InvalidOperationException($"Profile hat variant with ID {request.ProfileHatVariantId} not found.");
        }
        
        await _storageService.DeleteFileAsync(variantPath);
    }

    /// <inheritdoc />
    public async Task<string> GetPresignedProfileHatVariantUrlAsync(ProfileHatVariantGetByIdRequest request)
    {
        if (!StorageRequestsValidator.ValidateProfileHatVariantGetByIdRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Error: {errors}", nameof(request));
        }
        
        var variantPath = await FindProfileHatVariantPathAsync(request.ProfileHatVariantId);
        if (string.IsNullOrWhiteSpace(variantPath))
        {
            throw new InvalidOperationException($"Profile hat variant with ID {request.ProfileHatVariantId} not found.");
        }
        
        var result = await _storageService.GetPresignedUrlAsync(variantPath);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for hat variant: {request.ProfileHatVariantId}");
        }
        
        return result;
    }

    ///<inheritdoc/>
    public async Task<List<string>> GetPresignedProductImagesUrlsAsync(Guid productId, int expiresInSeconds = 3600)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }
        
        var productImages = await _productImagesRepository.GetByProductIdAsync(productId);
        if (productImages == null)
        {
            throw new InvalidOperationException($"Product with ID {productId} not found.");
        }

        var productImagesList = productImages.ToList();
        if (!productImagesList.Any())
        {
            return new List<string>();
        }

        var presignedUrls = await Task.WhenAll(
            productImagesList.Select(async pi =>
                await _storageService.GetPresignedUrlAsync(pi.ItemImage.ImageUrl, expiresInSeconds))
        );

        return presignedUrls.ToList();
    }

    ///<inheritdoc/>
    public async Task<string> GetPresignedUserAvatarUrlAsync(Guid userId, int expiresInSeconds = 3600)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }

        if (string.IsNullOrWhiteSpace(user.AvatarUrl))
        {
            return string.Empty;
        }
        
        var result = await _storageService.GetPresignedUrlAsync(user.AvatarUrl, expiresInSeconds);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for user avatar: {userId}");
        }
        
        return result;
    }

    ///<inheritdoc/>
    public async Task<string> GetPresignedProductLogoUrlAsync(Guid productId, int expiresInSeconds = 3600)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }
        
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {productId} not found.");
        }

        if (string.IsNullOrWhiteSpace(product.LogoUrl))
        {
            return string.Empty;
        }
        
        var result = await _storageService.GetPresignedUrlAsync(product.LogoUrl, expiresInSeconds);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for product logo: {productId}");
        }
        
        return result;
    }

    ///<inheritdoc/>
    public async Task<string> GetPresignedAchievementImageUrlAsync(Guid achievementId, int expiresInSeconds = 3600)
    {
        if (achievementId == Guid.Empty)
        {
            throw new ArgumentException("Achievement ID cannot be empty.", nameof(achievementId));
        }
        
        var achievement = await _achievementRepository.GetByIdAsync(achievementId);
        if (achievement == null)
        {
            throw new InvalidOperationException($"Achievement with ID {achievementId} not found.");
        }
        
        if (string.IsNullOrWhiteSpace(achievement.LogoUrl))
        {
            return string.Empty;
        }

        var result = await _storageService.GetPresignedUrlAsync(achievement.LogoUrl, expiresInSeconds);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for achievement logo: {achievementId}");
        }
        
        return result;
    }

    ///<inheritdoc/>
    public async Task<string> GetPresignedMatureContentImageUrlAsync(Guid matureContentId, int expiresInSeconds = 3600)
    {
        if (matureContentId == Guid.Empty)
        {
            throw new ArgumentException("Mature content ID cannot be empty.", nameof(matureContentId));
        }
        
        var matureContent = await _matureContentRepository.GetByIdAsync(matureContentId);
        if (matureContent == null)
        {
            throw new InvalidOperationException($"Mature content with ID {matureContentId} not found.");
        }
        
        if (string.IsNullOrWhiteSpace(matureContent.LogoUrl))
        {
            return string.Empty;
        }
        
        var result = await _storageService.GetPresignedUrlAsync(matureContent.LogoUrl, expiresInSeconds);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for mature content logo: {matureContentId}");
        }
        
        return result;
    }

    ///<inheritdoc/>
    public async Task<string> GetPresignedMechanicImageUrlAsync(Guid mechanicId, int expiresInSeconds = 3600)
    {
        if (mechanicId == Guid.Empty)
        {
            throw new ArgumentException("Mechanic ID cannot be empty.", nameof(mechanicId));
        }

        var mechanic = await _mechanicRepository.GetByIdAsync(mechanicId);
        if (mechanic == null)
        {
            throw new InvalidOperationException($"Mechanic with ID {mechanicId} not found.");
        }
        
        if (string.IsNullOrWhiteSpace(mechanic.LogoUrl))
        {
            return string.Empty;
        }
        
        var result = await _storageService.GetPresignedUrlAsync(mechanic.LogoUrl, expiresInSeconds);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for mechanic logo: {mechanicId}");
        }
        
        return result;
    }

    ///<inheritdoc/>
    public async Task<string> GetPresignedNewsArticleImageUrlAsync(Guid newsArticleId, int expiresInSeconds = 3600)
    {
        if (newsArticleId == Guid.Empty)
        {
            throw new ArgumentException("News article ID cannot be empty.", nameof(newsArticleId));
        }

        var newsArticle = await _newsArticleRepository.GetNewsArticleByIdAsync(newsArticleId);
        if (newsArticle == null)
        {
            throw new InvalidOperationException($"News article with ID {newsArticleId} not found.");
        }
        
        if (string.IsNullOrWhiteSpace(newsArticle.ImageUrl))
        {
            return string.Empty;
        }
        
        var result = await _storageService.GetPresignedUrlAsync(newsArticle.ImageUrl, expiresInSeconds);
        if (string.IsNullOrWhiteSpace(result))
        {
            throw new InvalidOperationException($"Failed to get presigned URL for news article image: {newsArticleId}");
        }
        
        return result;
    }

    ///<inheritdoc/>
    public async Task<Stream> DownloadFileChunkAsync(string objectPath, long offset, long length)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
            throw new ArgumentException("Object path cannot be empty.", nameof(objectPath));

        if (offset < 0)
            throw new ArgumentOutOfRangeException(nameof(offset));

        if (length <= 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        return await _storageService.DownloadChunkViaHttpAsync(objectPath, offset, length);
    }

    ///<inheritdoc/>
    public async Task<FileMetadataDto?> GetFileMetadataAsync(string objectPath)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
            throw new ArgumentException("Object path cannot be empty.", nameof(objectPath));
        
        var fileMetadata = await _storageService.GetFileMetadataAsync(objectPath);
        if (fileMetadata == null)
        {
            throw new InvalidOperationException($"File with path {objectPath} not found.");
        }
        
        return new FileMetadataDto
        {
            ObjectPath = fileMetadata.ObjectPath,
            Size = fileMetadata.Size,
            Hash = fileMetadata.GetHashCode(),
            ContentType = fileMetadata.ContentType,
            LastModified = fileMetadata.LastModified
        };
    }

    private async Task<string?> FindProfileHatVariantPathAsync(short profileHatVariantId)
    {
        string[] possibleExtensions = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp"];
        foreach (var extension in possibleExtensions)
        {
            string path = $"images/user/profile-hat-variants/{profileHatVariantId}{extension}";
            if (await _storageService.IsObjectExistsAsync(path))
            {
                return path;
            }
        }
        return null;
    }

    private async Task<string> EnsureUniqueObjectPathAsync(string objectPath)
    {
        int count = 0;
        string uniqueObjectPath = objectPath;
        
        if (uniqueObjectPath.Contains("games"))
        {
            uniqueObjectPath = uniqueObjectPath.Replace("games", "_");
        }
        
        while (await _storageService.IsObjectExistsAsync(objectPath))
        {
            count++;
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(objectPath);
            string extension = Path.GetExtension(objectPath);
            uniqueObjectPath = $"{nameWithoutExtension}({count}){extension}";
        }

        return uniqueObjectPath;
    }
    
    private async Task UploadStreamAsync(string objectPath, Stream dataStream, long objectSize, string contentType)
    {
        var result = await _storageService.UploadStreamAsync(objectPath, dataStream, objectSize, contentType);
        if (result.ResponseStatusCode != HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Failed to upload file, status code: {result.ResponseStatusCode}");
        }
    }
}