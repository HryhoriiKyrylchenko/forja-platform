using Forja.Application.DTOs.Games;

namespace Forja.Application.Mapping;

/// <summary>
/// Provides mapping functionality to transform a Game entity into a GameDto.
/// </summary>
public static class GamesEntityToDtoMapper
{
    /// <summary>
    /// Maps a <see cref="Game"/> entity to a <see cref="GameDto"/>.
    /// </summary>
    /// <param name="game">The game entity to be mapped.</param>
    /// <returns>A <see cref="GameDto"/> that represents the mapped game entity.</returns>
    public static GameDto MapToGameDto(Game game)
    {
        return new GameDto
        {
            Id = game.Id,
            Title = game.Title,
            ShortDescription = game.ShortDescription,
            Description = game.Description,
            Developer = game.Developer,
            MinimalAge = game.MinimalAge.ToString(),
            Platforms = game.Platforms,
            Price = game.Price,
            LogoUrl = game.LogoUrl,
            ReleaseDate = game.ReleaseDate,
            IsActive = game.IsActive,
            InterfaceLanguages = game.InterfaceLanguages,
            AudioLanguages = game.AudioLanguages,
            SubtitlesLanguages = game.SubtitlesLanguages,
            SystemRequirements = game.SystemRequirements,
            StorageUrl = game.StorageUrl
        };
    }

    /// <summary>
    /// Maps a <see cref="GameAddon"/> entity to a <see cref="GameAddonDto"/>.
    /// </summary>
    /// <param name="gameAddon">The game addon entity to be mapped.</param>
    /// <returns>A <see cref="GameAddonDto"/> that represents the mapped game addon entity.</returns>
    public static GameAddonDto MapGameAddonDto(GameAddon gameAddon)
    {
        return new GameAddonDto
        {
            Id = gameAddon.Id,
            Title = gameAddon.Title,
            ShortDescription = gameAddon.ShortDescription,
            Description = gameAddon.Description,
            Developer = gameAddon.Developer,
            MinimalAge = gameAddon.MinimalAge.ToString(),
            Platforms = gameAddon.Platforms,
            Price = gameAddon.Price,
            LogoUrl = gameAddon.LogoUrl,
            ReleaseDate = gameAddon.ReleaseDate,
            IsActive = gameAddon.IsActive,
            InterfaceLanguages = gameAddon.InterfaceLanguages,
            AudioLanguages = gameAddon.AudioLanguages,
            SubtitlesLanguages = gameAddon.SubtitlesLanguages,
            GameId = gameAddon.GameId,
            StorageUrl = gameAddon.StorageUrl
        };
    }
}