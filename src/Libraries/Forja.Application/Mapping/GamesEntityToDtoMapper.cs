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
            SystemRequirements = game.SystemRequirements
        };
    }
    
    public static GameExtendedDto MapToGameExtendedDto(Game game, 
                                                        string fullLogoUrl, 
                                                        List<MatureContentDto> matureContents, 
                                                        List<string> images, 
                                                        List<GameAddonShortDto> gameAddons, 
                                                        List<MechanicDto> mechanics,
                                                        (int positiveReviews, int negativeReviews) rating)
    {
        return new GameExtendedDto
        {
            Id = game.Id,
            Title = game.Title,
            ShortDescription = game.ShortDescription,
            Description = game.Description,
            Developer = game.Developer,
            MinimalAge = game.MinimalAge,
            Platforms = game.Platforms,
            Price = game.Price,
            LogoUrl = game.LogoUrl,
            ReleaseDate = game.ReleaseDate,
            InterfaceLanguages = game.InterfaceLanguages,
            AudioLanguages = game.AudioLanguages,
            SubtitlesLanguages = game.SubtitlesLanguages,
            SystemRequirements = game.SystemRequirements,
            Images = images,
            Addons = gameAddons,
            Discounts = game.ProductDiscounts.Select(pd => StoreEntityToDtoMapper.MapToDiscountDto(pd.Discount))
                .Where(dto => 
                    (!dto.StartDate.HasValue || dto.StartDate <= DateTime.UtcNow) && 
                    (!dto.EndDate.HasValue || dto.EndDate >= DateTime.UtcNow))
                .OrderBy(dto => dto.StartDate)
                .ToList(),
            Genres = game.ProductGenres.Select(pg => MapToGenreDto(pg.Genre)).ToList(),
            Tags = game.GameTags.Select(gt => MapToTagDto(gt.Tag)).ToList(),
            Mechanics = mechanics,
            MatureContent = matureContents,
            Rating = rating
        };
    }

    public static GameCatalogDto MapToGameCatalogDto(Game game, 
                                                    string fullLogoUrl, 
                                                    (int positiveReviews, int negativeReviews) rating)
    {
        return new GameCatalogDto
        {
            Id = game.Id,
            Title = game.Title,
            LogoUrl = fullLogoUrl,
            ReleaseDate = game.ReleaseDate,
            Genres = game.ProductGenres.Select(pg => MapToGenreDto(pg.Genre)).ToList(),
            Tags = game.GameTags.Select(gt => MapToTagDto(gt.Tag)).ToList(),
            Discounts = game.ProductDiscounts.Select(pd => StoreEntityToDtoMapper.MapToDiscountDto(pd.Discount))
                .Where(dto => 
                    (!dto.StartDate.HasValue || dto.StartDate <= DateTime.UtcNow) && 
                    (!dto.EndDate.HasValue || dto.EndDate >= DateTime.UtcNow))
                .OrderBy(dto => dto.StartDate)
                .ToList(),
            Rating = rating
        };
    }

    public static GameAddonDto MapToGameAddonDto(GameAddon gameAddon, 
                                                string fullLogoUrl, 
                                                List<MatureContentDto> matureContent, 
                                                (int positiveReviews, int negativeReviews) rating)
    {
        return new GameAddonDto
        {
            Id = gameAddon.Id,
            Title = gameAddon.Title,
            ShortDescription = gameAddon.ShortDescription,
            Description = gameAddon.Description,
            Developer = gameAddon.Developer,
            MinimalAge = gameAddon.MinimalAge,
            Platforms = gameAddon.Platforms,
            Price = gameAddon.Price,
            LogoUrl = fullLogoUrl,
            ReleaseDate = gameAddon.ReleaseDate,
            IsActive = gameAddon.IsActive,
            InterfaceLanguages = gameAddon.InterfaceLanguages,
            AudioLanguages = gameAddon.AudioLanguages,
            SubtitlesLanguages = gameAddon.SubtitlesLanguages,
            GameId = gameAddon.GameId,
            StorageUrl = gameAddon.StorageUrl,
            Discounts = gameAddon.ProductDiscounts.Select(pd => StoreEntityToDtoMapper.MapToDiscountDto(pd.Discount))
                .Where(dto => 
                    (!dto.StartDate.HasValue || dto.StartDate <= DateTime.UtcNow) && 
                    (!dto.EndDate.HasValue || dto.EndDate >= DateTime.UtcNow))
                .OrderBy(dto => dto.StartDate)
                .ToList(),
            MatureContent = matureContent,
            Rating = rating
        };
    }

    public static GameAddonShortDto MapToGameAddonShortDto(GameAddon gameAddon, string fullLogoUrl)
    {
        return new GameAddonShortDto
        {
            Id = gameAddon.Id,
            Title = gameAddon.Title,
            ShortDescription = gameAddon.ShortDescription,
            LogoUrl = fullLogoUrl,
            Price = gameAddon.Price,
            Discounts = gameAddon.ProductDiscounts.Select(pd => StoreEntityToDtoMapper.MapToDiscountDto(pd.Discount))
                .Where(dto => 
                    (!dto.StartDate.HasValue || dto.StartDate <= DateTime.UtcNow) && 
                    (!dto.EndDate.HasValue || dto.EndDate >= DateTime.UtcNow))
                .OrderBy(dto => dto.StartDate)
                .ToList()
        };
    }
    
    /// <summary>
    /// Maps a <see cref="Bundle"/> entity to a <see cref="BundleDto"/>.
    /// </summary>
    public static BundleDto MapToBundleDto(Bundle bundle)
    {
        var bundleProducts = bundle.BundleProducts.Select(bp => new ProductDto
        {
            Id = bp.ProductId,
            Title = bp.Product.Title,
            ShortDescription = bp.Product.ShortDescription,
            Description = bp.Product.Description,
            Price = bp.Product.Price,
            LogoUrl = bp.Product.LogoUrl,
            ReleaseDate = bp.Product.ReleaseDate,
            IsActive = bp.Product.IsActive
        }).ToList();
        
        return new BundleDto
        {
            Id = bundle.Id,
            Title = bundle.Title,
            Description = bundle.Description,
            TotalPrice = bundle.TotalPrice,
            CreatedAt = bundle.CreatedAt,
            IsActive = bundle.IsActive,
            BundleProducts = bundleProducts
        };
    }

    /// <summary>
    /// Maps a <see cref="BundleProduct"/> entity to a <see cref="BundleProductDto"/>.
    /// </summary>
    /// <param name="bundleProduct">The bundle product entity to be mapped.</param>
    /// <returns>A <see cref="BundleProductDto"/> that represents the mapped bundle product entity.</returns>
    public static BundleProductDto MapToBundleProductDto(BundleProduct bundleProduct)
    {
        return new BundleProductDto
        {
            Id = bundleProduct.Id,
            BundleId = bundleProduct.BundleId,
            ProductId = bundleProduct.ProductId
        };
    }

    /// <summary>
    /// Maps a <see cref="GameMechanic"/> entity to a <see cref="GameMechanicDto"/>.
    /// </summary>
    /// <param name="gameMechanic">The game mechanic entity to be mapped.</param>
    /// <returns>A <see cref="GameMechanicDto"/> that represents the mapped game mechanic entity.</returns>
    public static GameMechanicDto MapToGameMechanicDto(GameMechanic gameMechanic)
    {
        return new GameMechanicDto
        {
            Id = gameMechanic.Id,
            GameId = gameMechanic.GameId,
            MechanicId = gameMechanic.MechanicId
        };
    }

    /// <summary>
    /// Maps a <see cref="GameTag"/> entity to a <see cref="GameTagDto"/>.
    /// </summary>
    /// <param name="gameTag">The game tag entity to be mapped.</param>
    /// <returns>A <see cref="GameTagDto"/> that represents the mapped game tag entity.</returns>
    public static GameTagDto MapToGameTagDto(GameTag gameTag)
    {
        return new GameTagDto
        {
            Id = gameTag.Id,
            GameId = gameTag.GameId,
            TagId = gameTag.TagId
        };
    }

    /// <summary>
    /// Maps a <see cref="Genre"/> entity to a <see cref="GenreDto"/>.
    /// </summary>
    /// <param name="genre">The genre entity to be mapped.</param>
    /// <returns>A <see cref="GenreDto"/> that represents the mapped genre entity.</returns>
    public static GenreDto MapToGenreDto(Genre genre)
    {
        return new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }
    
    public static ItemImageDto MapToItemImageDto(ItemImage itemImage, string fullLogoUrl)
    {
        return new ItemImageDto
        {
            Id = itemImage.Id,
            ImageUrl = fullLogoUrl,
            ImageAlt = itemImage.ImageAlt
        };
    }
    
    public static MatureContentDto MapToMatureContentDto(MatureContent matureContent, string fullLogoUrl)
    {
        return new MatureContentDto
        {
            Id = matureContent.Id,
            Name = matureContent.Name,
            Description = matureContent.Description,
            LogoUrl = fullLogoUrl
        };
    }

    public static MechanicDto MapToMechanicDto(Mechanic mechanic, string fullLogoUrl)
    {
        return new MechanicDto
        {
            Id = mechanic.Id,
            Name = mechanic.Name,
            Description = mechanic.Description,
            LogoUrl = fullLogoUrl,
            IsDeleted = mechanic.IsDeleted
        };
    }

    /// <summary>
    /// Maps a <see cref="ProductGenres"/> entity to a <see cref="ProductGenresDto"/>.
    /// </summary>
    /// <param name="productGenres">The product genres entity to be mapped.</param>
    /// <returns>A <see cref="ProductGenresDto"/> that represents the mapped product genres entity.</returns>
    public static ProductGenresDto MapToProductGenresDto(ProductGenres productGenres)
    {
        return new ProductGenresDto
        {
            Id = productGenres.Id,
            ProductId = productGenres.ProductId,
            GenreId = productGenres.GenreId
        };
    }

    /// <summary>
    /// Maps a <see cref="ProductImages"/> entity to a <see cref="ProductImagesDto"/>.
    /// </summary>
    /// <param name="productImages">The product images entity to be mapped.</param>
    /// <returns>A <see cref="ProductImagesDto"/> that represents the mapped product images entity.</returns>
    public static ProductImagesDto MapToProductImagesDto(ProductImages productImages)
    {
        return new ProductImagesDto
        {
            Id = productImages.Id,
            ProductId = productImages.ProductId,
            ItemImageId = productImages.ItemImageId
        };
    }

    /// <summary>
    /// Maps a <see cref="ProductMatureContent"/> entity to a <see cref="ProductMatureContentDto"/>.
    /// </summary>
    /// <param name="productMatureContent">The product mature content entity to be mapped.</param>
    /// <returns>A <see cref="ProductMatureContentDto"/> that represents the mapped product mature content entity.</returns>
    public static ProductMatureContentDto MapToProductMatureContentDto(ProductMatureContent productMatureContent)
    {
        return new ProductMatureContentDto
        {
            Id = productMatureContent.Id,
            ProductId = productMatureContent.ProductId,
            MatureContentId = productMatureContent.MatureContentId
        };
    }

    /// <summary>
    /// Maps a <see cref="Tag"/> entity to a <see cref="TagDto"/>.
    /// </summary>
    /// <param name="tag">The tag entity to be mapped.</param>
    /// <returns>A <see cref="TagDto"/> that represents the mapped tag entity.</returns>
    public static TagDto MapToTagDto(Tag tag)
    {
        return new TagDto
        {
            Id = tag.Id,
            Title = tag.Title
        };
    }

    public static GameVersionDto MapToGameVersionDto(GameVersion gameVersion)
    {
        return new GameVersionDto
        {
            Id = gameVersion.Id,
            GameId = gameVersion.GameId,
            Version = gameVersion.Version,
            StorageUrl = gameVersion.StorageUrl,
            FileSize = gameVersion.FileSize,
            Hash = gameVersion.Hash,
            Changelog = gameVersion.Changelog,
            ReleaseDate = gameVersion.ReleaseDate
        };
    }

    public static GameFileDto MapToGameFileDto(GameFile gameFile)
    {
        return new GameFileDto
        {
            Id = gameFile.Id,
            GameVersionId = gameFile.GameVersionId,
            FileName = gameFile.FileName,
            FilePath = gameFile.FilePath,
            FileSize = gameFile.FileSize,
            Hash = gameFile.Hash,
            IsArchive = gameFile.IsArchive
        };
    }

    public static GamePatchDto MapToGamePatchDto(GamePatch gamePatch)
    {
        return new GamePatchDto
        {
            Id = gamePatch.Id,
            GameId = gamePatch.GameId,
            Name = gamePatch.Name,
            FromVersion = gamePatch.FromVersion,
            ToVersion = gamePatch.ToVersion,
            PatchUrl = gamePatch.PatchUrl,
            FileSize = gamePatch.FileSize,
            Hash = gamePatch.Hash,
            ReleaseDate = gamePatch.ReleaseDate
        };
    }
}