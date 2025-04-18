namespace Forja.Application.Services.UserProfile;

/// <summary>
/// Service class to manage user library operations such as managing games and add-ons in a user's library.
/// </summary>
public class UserLibraryService : IUserLibraryService
{
    private readonly IUserLibraryGameRepository _userLibraryGameRepository;
    private readonly IUserLibraryAddonRepository _userLibraryAddonRepository;
    private readonly IFileManagerService _fileManagerService;

    public UserLibraryService(IUserLibraryGameRepository userLibraryGameRepository, 
        IUserLibraryAddonRepository userLibraryAddonRepository,
        IFileManagerService fileManagerService)
    {
        _userLibraryGameRepository = userLibraryGameRepository;
        _userLibraryAddonRepository = userLibraryAddonRepository;
        _fileManagerService = fileManagerService;
    }
    
    #region Games Methods

    /// <inheritdoc />
    public async Task<UserLibraryGameDto?> AddUserLibraryGameAsync(UserLibraryGameCreateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateUserLibraryGameCreateRequest(request))
        {
            throw new ArgumentException("User library game create request is invalid.", nameof(request));
        }
        
        var userLibraryGame = new UserLibraryGame()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            GameId = request.GameId,
            PurchaseDate = DateTime.UtcNow
        };
        
        var result = await _userLibraryGameRepository.AddAsync(userLibraryGame);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(
            result,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(userLibraryGame.GameId, 1900));
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto?> UpdateUserLibraryGameAsync(UserLibraryGameUpdateRequest request)
    {
       
        if (!UserProfileRequestsValidator.ValidateUserLibraryGameUpdateRequest(request))
        {
            throw new ArgumentException("User library game update request is invalid.", nameof(request));
        }
        
        var userLibraryGame = await _userLibraryGameRepository.GetByIdAsync(request.Id);
        if (userLibraryGame == null)
        {
            throw new InvalidOperationException($"User library game with ID {request.Id} does not exist.");
        }
        
        userLibraryGame.UserId = request.UserId;
        userLibraryGame.GameId = request.GameId;
        userLibraryGame.TimePlayed = request.TimePlayed;
        userLibraryGame.PurchaseDate = request.PurchaseDate;
        
        var result = await _userLibraryGameRepository.UpdateAsync(userLibraryGame);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(
            result,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(userLibraryGame.GameId, 1900));
    }

    /// <inheritdoc />
    public async Task DeleteUserLibraryGameAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            
        }
        
        var userLibraryGame = await _userLibraryGameRepository.GetByIdAsync(userLibraryGameId);
        if (userLibraryGame == null)
        {
            throw new KeyNotFoundException($"User library game with ID '{userLibraryGameId}' was not found.");
        }
        
        await _userLibraryGameRepository.DeleteAsync(userLibraryGameId);
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto?> RestoreUserLibraryGameAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game ID cannot be an empty Guid.", nameof(userLibraryGameId));
        }
        
        var result = await _userLibraryGameRepository.RestoreAsync(userLibraryGameId);

        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(
            result,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(result.GameId, 1900));
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto?> GetUserLibraryGameByIdAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game ID cannot be an empty Guid.", nameof(userLibraryGameId));
        }
        
        var userLibraryGame = await _userLibraryGameRepository.GetByIdAsync(userLibraryGameId);
        
        return userLibraryGame == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(
            userLibraryGame,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(userLibraryGame.GameId, 1900));
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto?> GetDeletedUserLibraryGameByIdAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game ID cannot be an empty Guid.", nameof(userLibraryGameId));
        }
        
        var deletedUserLibraryGame = await _userLibraryGameRepository.GetDeletedByIdAsync(userLibraryGameId);
        
        return deletedUserLibraryGame == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(
            deletedUserLibraryGame,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(deletedUserLibraryGame.GameId, 1900));
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto?> GetUserLibraryGameByGameIdAndUserIdAsync(Guid gameId, Guid userId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        var userLibraryGame = await _userLibraryGameRepository.GetByGameIdAndUserIdAsync(gameId, userId);
        
        return userLibraryGame == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(
            userLibraryGame,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(userLibraryGame.GameId, 1900));
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto?> GetUserLibraryGameByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var userLibraryGame = await _userLibraryGameRepository.GetByGameIdAsync(gameId);

        return userLibraryGame == null
            ? null
            : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(
                userLibraryGame,
                await _fileManagerService.GetPresignedProductLogoUrlAsync(userLibraryGame.GameId, 1900)
            );
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameDto>> GetAllUserLibraryGamesAsync()
    {
        var allUserLibraryGames = await _userLibraryGameRepository.GetAllAsync();

        var userLibraryGames = allUserLibraryGames.ToList();
        if (!userLibraryGames.Any())
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        
        var libraryGames = await Task.WhenAll(userLibraryGames.Select(async ulg => UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(
            ulg,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(ulg.GameId, 1900)
        )));
        
        return libraryGames.ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameDto>> GetAllDeletedUserLibraryGamesAsync()
    {
        var allDeletedUserLibraryGames = await _userLibraryGameRepository.GetAllDeletedAsync();

        var userLibraryGames = allDeletedUserLibraryGames.ToList();
        if (!userLibraryGames.Any())
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        
        var libraryGames = await Task.WhenAll(userLibraryGames.Select(async ulg => UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(
            ulg,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(ulg.GameId, 1900)
        )));
        
        return libraryGames.ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameExtendedDto>> GetAllUserLibraryGamesByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        var userLibraryGames = await _userLibraryGameRepository.GetAllByUserIdAsync(userId);
        if (userLibraryGames == null)
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        var userLibraryGamesList = userLibraryGames.ToList();
        
        var userLibraryGameDtos = new List<UserLibraryGameExtendedDto>();
        
        foreach (var userLibraryGame in userLibraryGamesList)
        {
            string gameLogoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(userLibraryGame.GameId, 1900);

            Dictionary<Guid, string> achievementImageUrls = [];     
            var userAchievements = userLibraryGame.User.UserAchievements
                    .Where(ua =>
                        ua.UserId == userId &&                        
                        ua.Achievement.GameId == userLibraryGame.Game.Id)
                    .ToList();
            foreach (var achievement in userAchievements)
            {
                achievementImageUrls[achievement.AchievementId] = await _fileManagerService.GetPresignedAchievementImageUrlAsync(achievement.AchievementId);
            }
                
            Dictionary<Guid, string> addonImageUrls = [];
            foreach (var addon in userLibraryGame.PurchasedAddons)
            {
                addonImageUrls[addon.AddonId] = await _fileManagerService.GetPresignedProductLogoUrlAsync(addon.AddonId, 1900);
            }

            var dto = UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(
                userLibraryGame,
                gameLogoUrl,
                userAchievements.Select(ua =>
                    UserProfileEntityToDtoMapper.MapToAchievementShortDto(
                        ua.Achievement,
                        achievementImageUrls[ua.AchievementId])).ToList(),
                userLibraryGame.PurchasedAddons.Select(a =>
                    UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(
                        a,
                        addonImageUrls[a.AddonId])).ToList());

            userLibraryGameDtos.Add(dto);
        }
        
        return userLibraryGameDtos;
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameExtendedDto>> GetAllDeletedUserLibraryGamesByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        var allDeletedUserLibraryGames = await _userLibraryGameRepository.GetAllDeletedByUserIdAsync(userId);
        if (allDeletedUserLibraryGames == null)
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        var userLibraryGamesList = allDeletedUserLibraryGames.ToList();
        
        var userLibraryGameDtos = new List<UserLibraryGameExtendedDto>();
        
        foreach (var userLibraryGame in userLibraryGamesList)
        {
            string gameLogoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(userLibraryGame.GameId, 1900);
            
            var achievementDtos = await Task.WhenAll(userLibraryGame.Game.Achievements
                .Select(async a =>
                {
                    var logoUrl = await _fileManagerService.GetPresignedAchievementImageUrlAsync(a.Id);
                    return UserProfileEntityToDtoMapper.MapToAchievementShortDto(a, logoUrl);
                }));

            var addonDtos = await Task.WhenAll(userLibraryGame.PurchasedAddons
                .Select(async addon =>
                {
                    var addonLogoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(addon.AddonId, 1900);
                    return UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(addon, addonLogoUrl);
                }));

            var dto = UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(
                userLibraryGame,
                gameLogoUrl,
                achievementDtos.ToList(),
                addonDtos.ToList()
            );

            userLibraryGameDtos.Add(dto);
        }
        
        return userLibraryGameDtos;
    }
    
    #endregion
    
    #region Addons Methods

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto?> AddUserLibraryAddonAsync(UserLibraryAddonCreateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateUserLibraryAddonCreateRequest(request))
        {
            throw new ArgumentException("User library addon create request is invalid.", nameof(request));
        }
        
        var userLibraryAddon = new UserLibraryAddon()
        {
            Id = Guid.NewGuid(),
            UserLibraryGameId = request.UserLibraryGameId,
            AddonId = request.AddonId,
            PurchaseDate = DateTime.UtcNow
        };
        
        var result = await _userLibraryAddonRepository.AddAsync(userLibraryAddon);
    
        if (result == null)
        {
            return null;
        }
        
        return UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(
            result, 
            await _fileManagerService.GetPresignedProductLogoUrlAsync(result.AddonId, 1900));
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto?> UpdateUserLibraryAddonAsync(UserLibraryAddonUpdateRequest request)
    {
        if (!UserProfileRequestsValidator.ValidateUserLibraryAddonUpdateRequest(request))
        {
            throw new ArgumentException("User library addon update request is invalid.", nameof(request));
        }

        var userLibraryAddon = await _userLibraryAddonRepository.GetByIdAsync(request.Id);
        if (userLibraryAddon == null)
        {
            throw new InvalidOperationException($"User library addon with ID {request.Id} does not exist.");
        }
        
        userLibraryAddon.UserLibraryGameId = request.UserLibraryGameId;
        userLibraryAddon.AddonId = request.AddonId;
        userLibraryAddon.PurchaseDate = request.PurchaseDate;
        
        var result = await _userLibraryAddonRepository.UpdateAsync(userLibraryAddon);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(
            result,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(userLibraryAddon.AddonId, 1900)
        );
    }

    /// <inheritdoc />
    public async Task DeleteUserLibraryAddonAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var userLibraryAddon = await _userLibraryAddonRepository.GetByIdAsync(userLibraryAddonId);
        if (userLibraryAddon == null)
        {
            throw new KeyNotFoundException($"User library addon with ID '{userLibraryAddonId}' was not found.");
        }
        
        await _userLibraryAddonRepository.DeleteAsync(userLibraryAddonId);
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto?> RestoreUserLibraryAddonAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var result = await _userLibraryAddonRepository.RestoreAsync(userLibraryAddonId);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(
            result,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(result.AddonId, 1900)
        );
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto?> GetUserLibraryAddonByIdAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var result = await _userLibraryAddonRepository.GetByIdAsync(userLibraryAddonId);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(
            result,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(result.AddonId, 1900)
        );
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto?> GetDeletedUserLibraryAddonByIdAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var result = await _userLibraryAddonRepository.GetDeletedByIdAsync(userLibraryAddonId);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(
            result,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(result.AddonId, 1900)
        );
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto?> GetUserLibraryAddonByAddonIdAndUserIdAsync(Guid addonId, Guid userId)
    {
        if (addonId == Guid.Empty)
        {
            throw new ArgumentException("Addon ID cannot be an empty Guid.", nameof(addonId));
        }
        
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        var userLibraryAddon = await _userLibraryAddonRepository.GetByGameIdAndUserIdAsync(addonId, userId);
        
        return userLibraryAddon == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(
            userLibraryAddon,
            await _fileManagerService.GetPresignedProductLogoUrlAsync(userLibraryAddon.AddonId, 1900)
        );
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetUserLibraryAddonByAddonIdAsync(Guid addonId)
    {
        if (addonId == Guid.Empty)
        {
            throw new ArgumentException("Addon ID cannot be an empty Guid.", nameof(addonId));
        }
        
        var libraryAddons = await _userLibraryAddonRepository.GetByAddonIdAsync(addonId);

        var addonDtos = await Task.WhenAll(
            libraryAddons.Select(async la =>
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(la.AddonId, 1900);
                return UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(la, logoUrl);
            })
        );

        return addonDtos.ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllUserLibraryAddonsAsync()
    {
        var libraryAddons = await _userLibraryAddonRepository.GetAllAsync();
        var libraryAddonsList = libraryAddons.ToList();
        Dictionary<Guid, string> addonImageUrls = [];
        foreach (var la in libraryAddonsList)
        {
            addonImageUrls[la.AddonId] = await _fileManagerService.GetPresignedProductLogoUrlAsync(la.AddonId, 1900);
        }

        return libraryAddonsList.Select(la =>
            
                UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(
                    la, 
                    addonImageUrls[la.AddonId])).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllDeletedUserLibraryAddonsAsync()
    {
        var allDeletedUserLibraryAddons = await _userLibraryAddonRepository.GetAllDeletedAsync();
        
        var libraryAddons = allDeletedUserLibraryAddons.ToList();
        var addonDtos = await Task.WhenAll(
            libraryAddons.Select(async la =>
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(la.AddonId, 1900);
                return UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(la, logoUrl);
            })
        );

        return addonDtos.ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllUserLibraryAddonsByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var libraryAddons = await _userLibraryAddonRepository.GetAllByGameIdAsync(gameId);
        
        var addonDtos = await Task.WhenAll(
            libraryAddons.Select(async la =>
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(la.AddonId, 1900);
                return UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(la, logoUrl);
            })
        );

        return addonDtos.ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllDeletedUserLibraryAddonsByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var libraryAddons = await _userLibraryAddonRepository.GetAllDeletedByGameIdAsync(gameId);
        
        var addonDtos = await Task.WhenAll(
            libraryAddons.Select(async la =>
            {
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(la.AddonId, 1900);
                return UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(la, logoUrl);
            })
        );

        return addonDtos.ToList();
    }

    #endregion
    
    /// <inheritdoc />
    public async Task<bool> IsUserOwnedProductAsync(Guid userId, Guid productId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be an empty Guid.", nameof(productId));
        }
        
        var userOwnedGame = await _userLibraryGameRepository.GetByGameIdAndUserIdAsync(productId, userId);
        var userOwnedAddon = await _userLibraryAddonRepository.GetByGameIdAndUserIdAsync(productId, userId);
        
        return userOwnedGame != null || userOwnedAddon != null;
    }

    #region Users Statistics Methods

    /// <inheritdoc />    
    public async Task<int> GetUsersGamesCountAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty.", nameof(userId));
        }

        var totalGames = await _userLibraryGameRepository.GetAllGamesCountByUserIdAsync(userId);

        return totalGames;
    }

    public async Task<int> GetUsersAddonsCountAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty.", nameof(userId));
        }

        var totalAddons = await _userLibraryAddonRepository.GetAllAddonsCountByUserIdAsync(userId);

        return totalAddons;
    }

    public async Task<List<Guid>> GetUserLibraryProductIdsByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty.", nameof(userId));
        }

        List<Guid> userLibraryProductIds = [];
        var userLibraryGames = await _userLibraryGameRepository.GetAllByUserIdAsync(userId);
        userLibraryProductIds.AddRange(userLibraryGames.Select(ug => ug.GameId));
        var userLibraryAddons = await _userLibraryAddonRepository.GetAllByUserIdAsync(userId);
        userLibraryProductIds.AddRange(userLibraryAddons.Select(ug => ug.AddonId));
        
        return userLibraryProductIds;
    }

    #endregion
}