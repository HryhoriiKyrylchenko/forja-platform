namespace Forja.Application.Validators;

/// <summary>
/// Provides static validation methods for various game-related request models.
/// </summary>
public static class GamesRequestsValidator
{
    /// <summary>
    /// Validates if the given <see cref="GameCreateRequest"/> contains the required and valid data
    /// to create a new game entry.
    /// </summary>
    /// <param name="request">The <see cref="GameCreateRequest"/> object containing the game data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Title is not null, empty, or whitespace.
    /// - The MinimalAge is greater than 0.
    /// - The ReleaseDate is not the default date.
    /// </returns>
    public static bool ValidateGamesCreateRequest(GameCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title)) return false;
        if (request.MinimalAge <= 0) return false;
        if (request.ReleaseDate == default) return false;
        return true;
    }

    /// <summary>
    /// Validates the specified game update request by ensuring the provided data meets the required criteria.
    /// The validation checks include verifying that the ID is not an empty GUID,
    /// the title is not null or whitespace, the minimal age is greater than zero,
    /// and the release date is not the default value.
    /// </summary>
    /// <param name="request">The game update request to validate.</param>
    /// <returns>
    /// Returns <c>true</c> if the game update request is valid; otherwise, <c>false</c>.
    /// </returns>
    public static bool ValidateGamesUpdateRequest(GameUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (string.IsNullOrWhiteSpace(request.Title)) return false;
        if (request.MinimalAge <= 0) return false;
        if (request.ReleaseDate == default) return false;
        return true;
    }

    /// <summary>
    /// Validates the GameAddonCreateRequest object to ensure all required properties meet the expected criteria.
    /// </summary>
    /// <param name="request">The GameAddonCreateRequest object to validate, containing details of the game addon to be created.</param>
    /// <returns>
    /// A boolean value indicating the validation result.
    /// Returns true if the request is valid; otherwise, false.
    /// </returns>
    public static bool ValidateGameAddonCreateRequest(GameAddonCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title)) return false;
        if (request.MinimalAge <= 0) return false;
        if (request.ReleaseDate == default) return false;
        if (request.GameId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates the provided <see cref="GameAddonUpdateRequest"/> to ensure all required fields
    /// meet the specified requirements for a game addon update operation.
    /// </summary>
    /// <param name="request">The <see cref="GameAddonUpdateRequest"/> instance containing the game addon details to validate.</param>
    /// <returns>
    /// A boolean indicating whether the provided <see cref="GameAddonUpdateRequest"/> is valid.
    /// Returns true if the validation succeeds; otherwise, false.
    /// </returns>
    public static bool ValidateGameAddonUpdateRequest(GameAddonUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (string.IsNullOrWhiteSpace(request.Title)) return false;
        if (request.MinimalAge <= 0) return false;
        if (request.ReleaseDate == default) return false;
        if (request.GameId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates a given <see cref="BundleCreateRequest"/> to determine if it satisfies the required constraints.
    /// </summary>
    /// <param name="request">The <see cref="BundleCreateRequest"/> object to be validated.</param>
    /// <returns>True if the request is valid; otherwise, false.</returns>
    public static bool ValidateBundleCreateRequest(BundleCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title)) return false;
        if (request.TotalPrice <= 0) return false;
        if (request.ExpirationDate != null && request.ExpirationDate < DateTime.UtcNow) return false;
        return true;
    }

    /// <summary>
    /// Validates a <see cref="BundleUpdateRequest"/> to ensure it meets the required criteria for updating a game bundle.
    /// </summary>
    /// <param name="request">The <see cref="BundleUpdateRequest"/> containing the bundle update data to validate.</param>
    /// <returns>
    /// A boolean value indicating whether the validation was successful.
    /// Returns <c>false</c> if the request has an invalid ID, an empty or whitespace title,
    /// or a non-positive total price; otherwise, returns <c>true</c>.
    /// </returns>
    public static bool ValidateBundleUpdateRequest(BundleUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (string.IsNullOrWhiteSpace(request.Title)) return false;
        if (request.TotalPrice <= 0) return false;
        return true;
    }
    
    public static bool ValidateBundleProductsCreateRequest(BundleProductsCreateRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        if (request == null)
        {
            errorMessage = "The request is null.";
            return false;
        }

        if (request.BundleId == Guid.Empty)
        {
            errorMessage = "The bundle ID is empty.";
            return false;
        }

        if (request.ProductIds.Count < 2)
        {
            errorMessage = "The bundle must contain at least two products.";
            return false;
        }

        if (request.BundleTotalPrice < request.ProductIds.Count * 0.01m)
        {
            errorMessage = "The bundle total price must be at least 1% of the total price of the products.";
            return false;
        }
        return true;
    }

    /// <summary>
    /// Validates the properties of a <see cref="BundleProductUpdateRequest"/> to ensure they are correctly populated.
    /// </summary>
    /// <param name="request">The <see cref="BundleProductUpdateRequest"/> object to validate.</param>
    /// <returns>
    /// A boolean value indicating whether the request is valid.
    /// Returns true if the request contains valid data; otherwise, false.
    /// </returns>
    public static bool ValidateBundleProductUpdateRequest(BundleProductUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (request.BundleId == Guid.Empty) return false;
        if (request.ProductId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="GameMechanicCreateRequest"/> contains the required and valid data
    /// to associate a mechanic with a game.
    /// </summary>
    /// <param name="request">The <see cref="GameMechanicCreateRequest"/> object containing the mechanic and game association data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The GameId is not an empty GUID.
    /// - The MechanicId is not an empty GUID.
    /// </returns>
    public static bool ValidateGameMechanicCreateRequest(GameMechanicCreateRequest request)
    {
        if (request.GameId == Guid.Empty) return false;
        if (request.MechanicId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="GameMechanicUpdateRequest"/> contains the required and valid data
    /// to update a game mechanic within a specific game.
    /// </summary>
    /// <param name="request">The <see cref="GameMechanicUpdateRequest"/> object containing the data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Id is not an empty GUID.
    /// - The GameId is not an empty GUID.
    /// - The MechanicId is not an empty GUID.
    /// </returns>
    public static bool ValidateGameMechanicUpdateRequest(GameMechanicUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (request.GameId == Guid.Empty) return false;
        if (request.MechanicId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="GameTagCreateRequest"/> contains the required and valid data
    /// to create a new association between a game and a tag.
    /// </summary>
    /// <param name="request">The <see cref="GameTagCreateRequest"/> object containing the association data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The GameId is not an empty GUID.
    /// - The TagId is not an empty GUID.
    /// </returns>
    public static bool ValidateGameTagCreateRequest(GameTagCreateRequest request)
    {
        if (request.GameId == Guid.Empty) return false;
        if (request.TagId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="GameTagUpdateRequest"/> contains the required and valid data
    /// to update a game tag entry.
    /// </summary>
    /// <param name="request">The <see cref="GameTagUpdateRequest"/> object containing the tag update data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Id is not an empty GUID.
    /// - The GameId is not an empty GUID.
    /// - The TagId is not an empty GUID.
    /// </returns>
    public static bool ValidateGameTagUpdateRequest(GameTagUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (request.GameId == Guid.Empty) return false;
        if (request.TagId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="GenreCreateRequest"/> contains the required and valid data
    /// to create a new genre entry.
    /// </summary>
    /// <param name="request">The <see cref="GenreCreateRequest"/> object containing the genre data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Name is not null, empty, or whitespace.
    /// - The Name does not exceed the maximum allowed length.
    /// </returns>
    public static bool ValidateGenreCreateRequest(GenreCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return false;
        return true;
    }

    /// <summary>
    /// Validates if the provided <see cref="GenreUpdateRequest"/> contains the required and valid data
    /// for updating an existing genre entry.
    /// </summary>
    /// <param name="request">The <see cref="GenreUpdateRequest"/> object containing the genre data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Id is not an empty Guid.
    /// - The Name is not null, empty, or whitespace.
    /// </returns>
    public static bool ValidateGenreUpdateRequest(GenreUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (string.IsNullOrWhiteSpace(request.Name)) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="ItemImageCreateRequest"/> contains the required and valid data
    /// to create a new item image entry.
    /// </summary>
    /// <param name="request">The <see cref="ItemImageCreateRequest"/> object containing the item image data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The ImageUrl is not null, empty, or whitespace.
    /// - The ImageAlt is not null, empty, or whitespace.
    /// </returns>
    public static bool ValidateItemImageCreateRequest(ItemImageCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ImageUrl)) return false;
        if (string.IsNullOrWhiteSpace(request.ImageAlt)) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="ItemImageUpdateRequest"/> contains the required and valid data
    /// to update the image information of an item.
    /// </summary>
    /// <param name="request">The <see cref="ItemImageUpdateRequest"/> object containing the item image update data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Id is not empty.
    /// - The ImageUrl is not null, empty, or whitespace.
    /// - The ImageAlt is not null, empty, or whitespace.
    /// </returns>
    public static bool ValidateItemImageUpdateRequest(ItemImageUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (string.IsNullOrWhiteSpace(request.ImageUrl)) return false;
        if (string.IsNullOrWhiteSpace(request.ImageAlt)) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="MatureContentCreateRequest"/> contains the required and valid data
    /// to create a new mature content entry.
    /// </summary>
    /// <param name="request">The <see cref="MatureContentCreateRequest"/> object containing the mature content data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Name is not null, empty, or whitespace.
    /// - The Name does not exceed 50 characters.
    /// </returns>
    public static bool ValidateMatureContentCreateRequest(MatureContentCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="MatureContentUpdateRequest"/> contains the required and valid data
    /// to update a mature content entry.
    /// </summary>
    /// <param name="request">The <see cref="MatureContentUpdateRequest"/> object containing the mature content data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Id is not an empty GUID.
    /// - The Name is not null, empty, or whitespace and does not exceed 50 characters.
    /// </returns>
    public static bool ValidateMatureContentUpdateRequest(MatureContentUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="MechanicCreateRequest"/> contains the required and valid data
    /// to create a new mechanic entry.
    /// </summary>
    /// <param name="request">The <see cref="MechanicCreateRequest"/> object containing the mechanic data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Name is not null, empty, or whitespace.
    /// - The Name does not exceed 50 characters in length.
    /// </returns>
    public static bool ValidateMechanicCreateRequest(MechanicCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="MechanicUpdateRequest"/> contains the required and valid data
    /// to update an existing mechanic entry.
    /// </summary>
    /// <param name="request">The <see cref="MechanicUpdateRequest"/> object containing the mechanic data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Id is not an empty GUID.
    /// - The Name is not null, empty, or whitespace, and its length does not exceed 50 characters.
    /// </returns>
    public static bool ValidateMechanicUpdateRequest(MechanicUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="ProductGenresCreateRequest"/> contains the required and valid data
    /// to establish a relationship between a product and a genre.
    /// </summary>
    /// <param name="request">The <see cref="ProductGenresCreateRequest"/> object containing the product-genre data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The ProductId is not an empty GUID.
    /// - The GenreId is not an empty GUID.
    /// </returns>
    public static bool ValidateProductGenresCreateRequest(ProductGenresCreateRequest request)
    {
        if (request.ProductId == Guid.Empty) return false;
        if (request.GenreId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="ProductGenresUpdateRequest"/> contains the required and valid data
    /// to update the genres associated with a product.
    /// </summary>
    /// <param name="request">The <see cref="ProductGenresUpdateRequest"/> object containing the data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Id is not an empty GUID.
    /// - The ProductId is not an empty GUID.
    /// - The GenreId is not an empty GUID.
    /// </returns>
    public static bool ValidateProductGenresUpdateRequest(ProductGenresUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (request.ProductId == Guid.Empty) return false;
        if (request.GenreId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="ProductImagesCreateRequest"/> contains the required and valid data
    /// to create an association between a product and an image.
    /// </summary>
    /// <param name="request">The <see cref="ProductImagesCreateRequest"/> object containing the association data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The ProductId is not an empty GUID.
    /// - The ItemImageId is not an empty GUID.
    /// </returns>
    public static bool ValidateProductImagesCreateRequest(ProductImagesCreateRequest request)
    {
        if (request.ProductId == Guid.Empty) return false;
        if (request.ItemImageId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="ProductImagesUpdateRequest"/> contains the required and valid data
    /// to update a product's images in the system.
    /// </summary>
    /// <param name="request">The <see cref="ProductImagesUpdateRequest"/> object containing the data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Id is not empty.
    /// - The ProductId is not empty.
    /// - The ItemImageId is not empty.
    /// </returns>
    public static bool ValidateProductImagesUpdateRequest(ProductImagesUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (request.ProductId == Guid.Empty) return false;
        if (request.ItemImageId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="ProductMatureContentCreateRequest"/> contains the required and valid data
    /// to associate a product with mature content.
    /// </summary>
    /// <param name="request">The <see cref="ProductMatureContentCreateRequest"/> object containing the data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The ProductId is not an empty GUID.
    /// - The MatureContentId is not an empty GUID.
    /// </returns>
    public static bool ValidateProductMatureContentCreateRequest(ProductMatureContentCreateRequest request)
    {
        if (request.ProductId == Guid.Empty) return false;
        if (request.MatureContentId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="ProductMatureContentUpdateRequest"/> contains the required and valid data
    /// to update the mature content information for a specific product.
    /// </summary>
    /// <param name="request">The <see cref="ProductMatureContentUpdateRequest"/> object containing the mature content update data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Id is not an empty GUID.
    /// - The ProductId is not an empty GUID.
    /// - The MatureContentId is not an empty GUID.
    /// </returns>
    public static bool ValidateProductMatureContentUpdateRequest(ProductMatureContentUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (request.ProductId == Guid.Empty) return false;
        if (request.MatureContentId == Guid.Empty) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="TagCreateRequest"/> contains the required and valid data
    /// to create a new tag entry.
    /// </summary>
    /// <param name="request">The <see cref="TagCreateRequest"/> object containing the tag data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Title is not null, empty, or whitespace.
    /// - The Title does not exceed 30 characters.
    /// </returns>
    public static bool ValidateTagCreateRequest(TagCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 30) return false;
        return true;
    }

    /// <summary>
    /// Validates if the given <see cref="TagUpdateRequest"/> contains the required and valid data
    /// to update an existing tag entry.
    /// </summary>
    /// <param name="request">The <see cref="TagUpdateRequest"/> object containing the tag data to be validated.</param>
    /// <returns>
    /// True if the request is valid, otherwise false.
    /// A request is considered valid if:
    /// - The Id is not empty.
    /// - The Title is not null, empty, or whitespace, and its length is 30 characters or less.
    /// </returns>
    public static bool ValidateTagUpdateRequest(TagUpdateRequest request)
    {
        if (request.Id == Guid.Empty) return false;
        if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 30) return false;
        return true;
    }

    public static bool ValidateGameVersionCreateRequest(GameVersionCreateRequest? request, out string? errorMessage)
    {
        errorMessage = null;

        if (request == null)
        {
            errorMessage = "Request is null";
            return false;
        }

        if (request.GameId == Guid.Empty)
        {
            errorMessage = "GameId is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Version))
        {
            errorMessage = "Version is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.StorageUrl))
        {
            errorMessage = "StorageUrl is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.Hash))
        {
            errorMessage = "Hash is required";
            return false;
        }

        if (request.FileSize <= 0)
        {
            errorMessage = "File size is required";
            return false;
        }

        if (request.ReleaseDate > DateTime.UtcNow)
        {
            errorMessage = "Release date cannot be in the future";
            return false;
        }
        
        return true;
    }

    public static bool ValidateGameVersionUpdateRequest(GameVersionUpdateRequest? request, out string? errorMessage)
    {
        errorMessage = null;

        if (request == null)
        {
            errorMessage = "Request is null";
            return false;
        }

        if (request.Id == Guid.Empty)
        {
            errorMessage = "Id is required";
            return false;
        }

        if (request.ReleaseDate > DateTime.UtcNow)
        {
            errorMessage = "Release date cannot be in the future";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Hash))
        {
            errorMessage = "Hash is required";
            return false;
        }

        if (request.FileSize <= 0)
        {
            errorMessage = "File size is required";
            return false;
        }
        
        return true;
    }

    public static bool ValidateGameFileCreateRequest(GameFileCreateRequest? request, out string? errorMessage)
    {
        errorMessage = null;

        if (request == null)
        {
            errorMessage = "Request is null";
            return false;
        }

        if (request.GameVersionId == Guid.Empty)
        {
            errorMessage = "GameVersionId is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            errorMessage = "FileName is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.FilePath))
        {
            errorMessage = "FilePath is required";
            return false;
        }

        if (request.FileSize == 0)
        {
            errorMessage = "FileSize is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Hash))
        {
            errorMessage = "Hash is required";
            return false;
        }
        
        return true;
    }

    public static bool ValidateGameFileUpdateRequest(GameFileUpdateRequest? request, out string? errorMessage)
    {
        errorMessage = null;

        if (request == null)
        {
            errorMessage = "Request is null";
            return false;
        }

        if (request.Id == Guid.Empty)
        {
            errorMessage = "Id is required";
            return false;
        }
        
        if (request.FileSize == 0)
        {
            errorMessage = "FileSize is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Hash))
        {
            errorMessage = "Hash is required";
            return false;
        }
        
        return true;
    }

    public static bool ValidateGamePatchCreateRequest(GamePatchCreateRequest? request, out string? errorMessage)
    {
        errorMessage = null;

        if (request == null)
        {
            errorMessage = "Request is null";
            return false;
        }

        if (request.GameId == Guid.Empty)
        {
            errorMessage = "GameId is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errorMessage = "Name is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.FromVersion))
        {
            errorMessage = "FromVersion is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.ToVersion))
        {
            errorMessage = "ToVersion is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.PatchUrl))
        {
            errorMessage = "PatchUrl is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.Hash))
        {
            errorMessage = "Hash is required";
            return false;
        }

        if (request.FileSize <= 0)
        {
            errorMessage = "File size is required";
            return false;
        }

        return true;
    }

    public static bool ValidateGamePatchUpdateRequest(GamePatchUpdateRequest? request, out string? errorMessage)
    {
        errorMessage = null;
        
        if (request == null)
        {
            errorMessage = "Request is null";
            return false;
        }

        if (request.Id == Guid.Empty)
        {
            errorMessage = "Id is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errorMessage = "Name is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.FromVersion))
        {
            errorMessage = "FromVersion is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.ToVersion))
        {
            errorMessage = "ToVersion is required";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(request.Hash))
        {
            errorMessage = "Hash is required";
            return false;
        }
        
        if (request.FileSize <= 0)
        {
            errorMessage = "File size is required";
            return false;
        }
        
        return true;
    }
}