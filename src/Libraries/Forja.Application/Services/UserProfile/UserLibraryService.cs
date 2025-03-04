namespace Forja.Application.Services.UserProfile;

/// <summary>
/// Service class to manage user library operations such as managing games and add-ons in a user's library.
/// </summary>
public class UserLibraryService : IUserLibraryService
{
    private readonly IUserLibraryGameRepository _userLibraryGameRepository;
    private readonly IUserLibraryAddonRepository _userLibraryAddonRepository;
    private readonly IUserRepository _userRepository;

    public UserLibraryService(IUserLibraryGameRepository userLibraryGameRepository, IUserLibraryAddonRepository userLibraryAddonRepository, IUserRepository userRepository)
    {
        _userLibraryGameRepository = userLibraryGameRepository ?? throw new ArgumentNullException(nameof(userLibraryGameRepository));
        _userLibraryAddonRepository = userLibraryAddonRepository ?? throw new ArgumentNullException(nameof(userLibraryAddonRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }
    
    /// <inheritdoc />
    public async Task AddUserLibraryGameAsync(UserLibraryGameDto userLibraryGameDto)
    {
        if (userLibraryGameDto == null)
        {
            throw new ArgumentNullException(nameof(userLibraryGameDto), "User library game cannot be null.");
        }

        if (!ApplicationDtoValidator.ValidateUserLibraryGameDto(userLibraryGameDto))
        {
            throw new ArgumentException("User library game DTO is invalid.", nameof(userLibraryGameDto));
        }
        
        var userLibraryGame = new UserLibraryGame()
        {
            Id = userLibraryGameDto.Id,
            UserId = userLibraryGameDto.User.Id,
            GameId = userLibraryGameDto.Game.Id,
            PurchaseDate = userLibraryGameDto.PurchaseDate
        };
        
        await _userLibraryGameRepository.AddAsync(userLibraryGame);
    }

    /// <inheritdoc />
    public async Task UpdateUserLibraryGameAsync(UserLibraryGameDto userLibraryGameDto)
    {
        if (userLibraryGameDto == null)
        {
            throw new ArgumentNullException(nameof(userLibraryGameDto), "User library game cannot be null.");
        }
        
        if (!ApplicationDtoValidator.ValidateUserLibraryGameDto(userLibraryGameDto))
        {
            throw new ArgumentException("User library game DTO is invalid.", nameof(userLibraryGameDto));
        }
        
        var userLibraryGame = await _userLibraryGameRepository.GetByIdAsync(userLibraryGameDto.Id);
        if (userLibraryGame == null)
        {
            throw new InvalidOperationException($"User library game with ID {userLibraryGameDto.Id} does not exist.");
        }
        
        userLibraryGame.PurchaseDate = userLibraryGameDto.PurchaseDate;
        userLibraryGame.UserId = userLibraryGameDto.User.Id;
        userLibraryGame.GameId = userLibraryGameDto.Game.Id;
        
        await _userLibraryGameRepository.UpdateAsync(userLibraryGame);
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
    public async Task<UserLibraryGameDto> RestoreUserLibraryGameAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game ID cannot be an empty Guid.", nameof(userLibraryGameId));
        }
        
        var userLibraryGame = await _userLibraryGameRepository.RestoreAsync(userLibraryGameId);

        return new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
        };
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto> GetUserLibraryGameByIdAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game ID cannot be an empty Guid.", nameof(userLibraryGameId));
        }
        
        var userLibraryGame = await _userLibraryGameRepository.GetByIdAsync(userLibraryGameId);
        if (userLibraryGame == null)
        {
            throw new KeyNotFoundException($"User library game with ID '{userLibraryGameId}' was not found.");
        }
        
        return new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
            PurchaseDate = userLibraryGame.PurchaseDate
        };
    }

    /// <inheritdoc />
    public async Task<UserLibraryGameDto> GetDeletedUserLibraryGameByIdAsync(Guid userLibraryGameId)
    {
        if (userLibraryGameId == Guid.Empty)
        {
            throw new ArgumentException("User library game ID cannot be an empty Guid.", nameof(userLibraryGameId));
        }
        
        var deletedUserLibraryGame = await _userLibraryGameRepository.GetDeletedByIdAsync(userLibraryGameId);
        if (deletedUserLibraryGame == null)
        {
            throw new KeyNotFoundException($"User library game with ID '{userLibraryGameId}' was not found.");
        }
        
        return new UserLibraryGameDto()
        {
            Id = deletedUserLibraryGame.Id,
            User = deletedUserLibraryGame.User,
            Game = deletedUserLibraryGame.Game,
            PurchaseDate = deletedUserLibraryGame.PurchaseDate
        };
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
        
        return userLibraryGames.Select(userLibraryGame => new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
            PurchaseDate = userLibraryGame.PurchaseDate
        }).ToList();
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
        
        return userLibraryGames.Select(userLibraryGame => new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
            PurchaseDate = userLibraryGame.PurchaseDate
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameDto>> GetAllUserLibraryGamesByUserKeycloakIdAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _userRepository.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        var allUserLibraryGames = await _userLibraryGameRepository.GetAllByUserIdAsync(user.Id);
        
        var userLibraryGames = allUserLibraryGames.ToList();
        if (!userLibraryGames.Any())
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        
        return userLibraryGames.Select(userLibraryGame => new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
            PurchaseDate = userLibraryGame.PurchaseDate
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryGameDto>> GetAllDeletedUserLibraryGamesByUserKeycloakIdAsync(string userKeycloakId)
    {
        if (string.IsNullOrWhiteSpace(userKeycloakId))
        {
            throw new ArgumentNullException(nameof(userKeycloakId), "User Keycloak ID cannot be null or empty.");
        }
        
        var user = await _userRepository.GetByKeycloakIdAsync(userKeycloakId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        
        var allDeletedUserLibraryGames = await _userLibraryGameRepository.GetAllDeletedByUserIdAsync(user.Id);
        
        var userLibraryGames = allDeletedUserLibraryGames.ToList();
        if (!userLibraryGames.Any())
        {
            throw new KeyNotFoundException("No user library games were found.");
        }
        
        return userLibraryGames.Select(userLibraryGame => new UserLibraryGameDto()
        {
            Id = userLibraryGame.Id,
            User = userLibraryGame.User,
            Game = userLibraryGame.Game,
            PurchaseDate = userLibraryGame.PurchaseDate
        }).ToList();
    }

    /// <inheritdoc />
    public async Task AddUserLibraryAddonAsync(UserLibraryAddonDto userLibraryAddonDto)
    {
        if (!ApplicationDtoValidator.ValidateUserLibraryAddonDto(userLibraryAddonDto))
        {
            throw new ArgumentException("User library addon DTO is invalid.", nameof(userLibraryAddonDto));
        }
        
        var userLibraryAddon = new UserLibraryAddon()
        {
            Id = userLibraryAddonDto.Id,
            UserLibraryGameId = userLibraryAddonDto.UserLibraryGame.Id,
            AddonId = userLibraryAddonDto.GameAddon.Id,
            PurchaseDate = userLibraryAddonDto.PurchaseDate
        };
        
        await _userLibraryAddonRepository.AddAsync(userLibraryAddon);
    }

    /// <inheritdoc />
    public async Task UpdateUserLibraryAddonAsync(UserLibraryAddonDto userLibraryAddonDto)
    {
        if (!ApplicationDtoValidator.ValidateUserLibraryAddonDto(userLibraryAddonDto))
        {
            throw new ArgumentException("User library addon DTO is invalid.", nameof(userLibraryAddonDto));
        }

        var userLibraryAddon = await _userLibraryAddonRepository.GetByIdAsync(userLibraryAddonDto.Id);
        if (userLibraryAddon == null)
        {
            throw new InvalidOperationException($"User library addon with ID {userLibraryAddonDto.Id} does not exist.");
        }
        
        userLibraryAddon.PurchaseDate = userLibraryAddonDto.PurchaseDate;
        userLibraryAddon.UserLibraryGameId = userLibraryAddonDto.UserLibraryGame.Id;
        userLibraryAddon.AddonId = userLibraryAddonDto.GameAddon.Id;
        
        await _userLibraryAddonRepository.UpdateAsync(userLibraryAddon);
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
    public async Task<UserLibraryAddonDto> RestoreUserLibraryAddonAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var userLibraryAddon = await _userLibraryAddonRepository.RestoreAsync(userLibraryAddonId);
        
        return new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        };
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto> GetUserLibraryAddonByIdAsync(Guid userLibraryAddonId)
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
        
        return new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        };
    }

    /// <inheritdoc />
    public async Task<UserLibraryAddonDto> GetDeletedUserLibraryAddonByIdAsync(Guid userLibraryAddonId)
    {
        if (userLibraryAddonId == Guid.Empty)
        {
            throw new ArgumentException("User library addon ID cannot be an empty Guid.", nameof(userLibraryAddonId));
        }
        
        var deletedUserLibraryAddon = await _userLibraryAddonRepository.GetDeletedByIdAsync(userLibraryAddonId);
        if (deletedUserLibraryAddon == null)
        {
            throw new KeyNotFoundException($"User library addon with ID '{userLibraryAddonId}' was not found.");
        }
        
        return new UserLibraryAddonDto()
        {
            Id = deletedUserLibraryAddon.Id,
            UserLibraryGame = deletedUserLibraryAddon.UserLibraryGame,
            GameAddon = deletedUserLibraryAddon.GameAddon,
            PurchaseDate = deletedUserLibraryAddon.PurchaseDate
        };
    }

    /// <inheritdoc />
    public async Task<List<UserLibraryAddonDto>> GetAllUserLibraryAddonsAsync()
    {
        var allUserLibraryAddons = await _userLibraryAddonRepository.GetAllAsync();
        
        var userLibraryAddons = allUserLibraryAddons.ToList();
        if (!userLibraryAddons.Any())
        {
            throw new KeyNotFoundException("No user library addons were found.");
        }

        return userLibraryAddons.Select(userLibraryAddon => new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        }).ToList();
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

        return userLibraryAddons.Select(userLibraryAddon => new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        }).ToList();
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
        
        return userLibraryAddons.Select(userLibraryAddon => new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        }).ToList();
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
        
        return userLibraryAddons.Select(userLibraryAddon => new UserLibraryAddonDto()
        {
            Id = userLibraryAddon.Id,
            UserLibraryGame = userLibraryAddon.UserLibraryGame,
            GameAddon = userLibraryAddon.GameAddon,
            PurchaseDate = userLibraryAddon.PurchaseDate
        }).ToList();
    }
}