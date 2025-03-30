namespace Forja.Application.Services.UserProfile;

/// <summary>
/// Service class to manage user library operations such as managing games and add-ons in a user's library.
/// </summary>
public class UserLibraryService : IUserLibraryService
{
    private readonly IUserLibraryGameRepository _userLibraryGameRepository;
    private readonly IUserLibraryAddonRepository _userLibraryAddonRepository;

    public UserLibraryService(IUserLibraryGameRepository userLibraryGameRepository, IUserLibraryAddonRepository userLibraryAddonRepository)
    {
        _userLibraryGameRepository = userLibraryGameRepository ?? throw new ArgumentNullException(nameof(userLibraryGameRepository));
        _userLibraryAddonRepository = userLibraryAddonRepository ?? throw new ArgumentNullException(nameof(userLibraryAddonRepository));
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
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(result);
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
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(result);
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

        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(result);
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto?> GetUserLibraryGameByIdAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game ID cannot be an empty Guid.", nameof(userLibraryGameId));
        }
        
        var userLibraryGame = await _userLibraryGameRepository.GetByIdAsync(userLibraryGameId);
        
        return userLibraryGame == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(userLibraryGame);
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto?> GetDeletedUserLibraryGameByIdAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game ID cannot be an empty Guid.", nameof(userLibraryGameId));
        }
        
        var deletedUserLibraryGame = await _userLibraryGameRepository.GetDeletedByIdAsync(userLibraryGameId);
        
        return deletedUserLibraryGame == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(deletedUserLibraryGame);
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
        
        return userLibraryGame == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryGameDto(userLibraryGame);
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameDto>> GetUserLibraryGamesByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var userLibraryGames = await _userLibraryGameRepository.GetByGameIdAsync(gameId);

        return userLibraryGames.Select(UserProfileEntityToDtoMapper.MapToUserLibraryGameDto).ToList();
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
        
        return userLibraryGames.Select(UserProfileEntityToDtoMapper.MapToUserLibraryGameDto).ToList();
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
        
        return userLibraryGames.Select(UserProfileEntityToDtoMapper.MapToUserLibraryGameDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameDto>> GetAllUserLibraryGamesByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        var allUserLibraryGames = await _userLibraryGameRepository.GetAllByUserIdAsync(userId);
        
        var userLibraryGames = allUserLibraryGames.ToList();
        if (!userLibraryGames.Any())
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        
        return userLibraryGames.Select(UserProfileEntityToDtoMapper.MapToUserLibraryGameDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameDto>> GetAllDeletedUserLibraryGamesByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be an empty Guid.", nameof(userId));
        }
        
        var allDeletedUserLibraryGames = await _userLibraryGameRepository.GetAllDeletedByUserIdAsync(userId);
        
        var userLibraryGames = allDeletedUserLibraryGames.ToList();
        if (!userLibraryGames.Any())
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        
        return userLibraryGames.Select(UserProfileEntityToDtoMapper.MapToUserLibraryGameDto).ToList();
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
    
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(result);
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
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(result);
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
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(result);
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto?> GetUserLibraryAddonByIdAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var result = await _userLibraryAddonRepository.GetByIdAsync(userLibraryAddonId);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(result);
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto?> GetDeletedUserLibraryAddonByIdAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var result = await _userLibraryAddonRepository.GetDeletedByIdAsync(userLibraryAddonId);
        
        return result == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(result);
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
        
        return userLibraryAddon == null ? null : UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto(userLibraryAddon);
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetUserLibraryAddonByAddonIdAsync(Guid addonId)
    {
        if (addonId == Guid.Empty)
        {
            throw new ArgumentException("Addon ID cannot be an empty Guid.", nameof(addonId));
        }
        
        var result = await _userLibraryAddonRepository.GetByAddonIdAsync(addonId);
        
        return result.Select(UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllUserLibraryAddonsAsync()
    {
        var allUserLibraryAddons = await _userLibraryAddonRepository.GetAllAsync();
        
        var userLibraryAddons = allUserLibraryAddons.ToList();
        //if (!userLibraryAddons.Any())
        //{
        //    throw new KeyNotFoundException("No user library addons were found.");
        //}

        return userLibraryAddons.Select(UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllDeletedUserLibraryAddonsAsync()
    {
        var allDeletedUserLibraryAddons = await _userLibraryAddonRepository.GetAllDeletedAsync();
        
        var userLibraryAddons = allDeletedUserLibraryAddons.ToList();
        if (!userLibraryAddons.Any())
        {
            throw new KeyNotFoundException("No user library addons were found.");
        }

        return userLibraryAddons.Select(UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllUserLibraryAddonsByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var allUserLibraryAddons = await _userLibraryAddonRepository.GetAllByGameIdAsync(gameId);
        
        var userLibraryAddons = allUserLibraryAddons.ToList();
        if (!userLibraryAddons.Any())
        {
            throw new KeyNotFoundException("No user library addons were found.");
        }
        
        return userLibraryAddons.Select(UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllDeletedUserLibraryAddonsByGameIdAsync(Guid gameId)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Game ID cannot be an empty Guid.", nameof(gameId));
        }
        
        var allDeletedUserLibraryAddons = await _userLibraryAddonRepository.GetAllDeletedByGameIdAsync(gameId);
        
        var userLibraryAddons = allDeletedUserLibraryAddons.ToList();
        if (!userLibraryAddons.Any())
        {
            throw new KeyNotFoundException("No user library addons were found.");
        }
        
        return userLibraryAddons.Select(UserProfileEntityToDtoMapper.MapToUserLibraryAddonDto).ToList();
    }

    #endregion
}