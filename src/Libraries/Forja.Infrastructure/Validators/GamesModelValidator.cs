namespace Forja.Infrastructure.Validators;

/// <summary>
/// Validates models within the games domain.
/// </summary>
public static class GamesModelValidator
{
    /// <summary>
    /// Validates the properties of a <see cref="Bundle"/> object to ensure its values meet the required business rules.
    /// </summary>
    /// <param name="bundle">The <see cref="Bundle"/> instance to validate.</param>
    /// <param name="errorMessage">Contains the validation error message if the method returns false.</param>
    /// <returns>
    /// True if the provided <see cref="Bundle"/> is valid; otherwise, false.
    /// </returns>
    /// <remarks>
    /// Validation rules include:
    /// - The bundle cannot be null.
    /// - The Title property is required and must not exceed 200 characters.
    /// - The Description property must not exceed 1000 characters.
    /// - The TotalPrice property must be greater than or equal to zero.
    /// - The CreatedAt property cannot specify a future date.
    /// - At least one item must exist in the BundleProducts collection.
    /// </remarks>
    public static bool ValidateBundle(Bundle? bundle, out string errorMessage)
    {
        if (bundle == null)
        {
            errorMessage = "Bundle cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(bundle.Title))
        {
            errorMessage = "Bundle Title is required.";
            return false;
        }

        if (bundle.Title.Length > 200)
        {
            errorMessage = "Bundle Title must not exceed 200 characters.";
            return false;
        }

        if (bundle.Description.Length > 1000)
        {
            errorMessage = "Bundle Description must not exceed 1000 characters.";
            return false;
        }

        if (bundle.TotalPrice < 0)
        {
            errorMessage = "Bundle Total Price must be greater than or equal to zero.";
            return false;
        }

        if (bundle.CreatedAt > DateTime.UtcNow)
        {
            errorMessage = "Bundle CreatedAt cannot be set in the future.";
            return false;
        }

        if (!bundle.BundleProducts.Any())
        {
            errorMessage = "Bundle must contain at least one product.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the provided <see cref="BundleProduct"/> instance by ensuring it contains valid data.
    /// </summary>
    /// <param name="bundleProduct">The <see cref="BundleProduct"/> instance to validate.</param>
    /// <param name="errorMessage">
    /// An output parameter that will contain the error message if validation fails.
    /// If validation is successful, this parameter will be an empty string.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether the validation was successful.
    /// Returns true if the <see cref="BundleProduct"/> instance is valid; otherwise, false.
    /// </returns>
    public static bool ValidateBundleProduct(BundleProduct? bundleProduct, out string errorMessage)
    {
        if (bundleProduct == null)
        {
            errorMessage = "BundleProduct cannot be null.";
            return false;
        }

        if (bundleProduct.BundleId == Guid.Empty)
        {
            errorMessage = "BundleProduct BundleId is required.";
            return false;
        }

        if (bundleProduct.ProductId == Guid.Empty)
        {
            errorMessage = "BundleProduct ProductId is required.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the specified game instance to ensure its properties meet the required criteria.
    /// </summary>
    /// <param name="game">The game instance to validate.</param>
    /// <param name="errorMessage">Outputs an error message describing the first validation failure, if any.</param>
    /// <returns>
    /// Returns <c>true</c> if the game instance is valid; otherwise, <c>false</c>.
    /// </returns>
    public static bool ValidateGame(Game? game, out string errorMessage)
    {
        if (game == null)
        {
            errorMessage = "Game cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(game.Title))
        {
            errorMessage = "Game Title is required.";
            return false;
        }

        if (game.Title.Length > 200)
        {
            errorMessage = "Game Title must not exceed 200 characters.";
            return false;
        }

        if (game.SystemRequirements?.Length > 2000)
        {
            errorMessage = "Game System Requirements must not exceed 2000 characters.";
            return false;
        }

        if (game.StorageUrl?.Length > 500)
        {
            errorMessage = "Game Storage URL must not exceed 500 characters.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a given GameAddon object and checks for missing or invalid properties.
    /// </summary>
    /// <param name="gameAddon">The GameAddon object to validate, which may be null.</param>
    /// <param name="errorMessage">Outputs error messages if validation fails, or an empty string if validation succeeds.</param>
    /// <returns>
    /// Returns true if the GameAddon object is valid, otherwise false.
    /// </returns>
    public static bool ValidateGameAddon(GameAddon? gameAddon, out string errorMessage)
    {
        if (gameAddon == null)
        {
            errorMessage = "GameAddon cannot be null.";
            return false;
        }

        if (gameAddon.GameId == Guid.Empty)
        {
            errorMessage = "GameAddon GameId is required.";
            return false;
        }

        if (gameAddon.StorageUrl?.Length > 500)
        {
            errorMessage = "GameAddon Storage URL must not exceed 500 characters.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a GameMechanic object to ensure it meets defined criteria.
    /// </summary>
    /// <param name="gameMechanic">
    /// The GameMechanic object to validate. This object represents the mapping between a game and a specific game mechanic.
    /// </param>
    /// <param name="errorMessage">
    /// An output parameter that will contain an error message if the validation fails; otherwise, it will be an empty string.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether the GameMechanic is valid. Returns true if valid; otherwise, false.
    /// </returns>
    public static bool ValidateGameMechanic(GameMechanic? gameMechanic, out string errorMessage)
    {
        if (gameMechanic == null)
        {
            errorMessage = "GameMechanic cannot be null.";
            return false;
        }

        if (gameMechanic.GameId == Guid.Empty)
        {
            errorMessage = "GameMechanic GameId is required.";
            return false;
        }

        if (gameMechanic.MechanicId == Guid.Empty)
        {
            errorMessage = "GameMechanic MechanicId is required.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the provided GameTag object to ensure it meets the required criteria.
    /// </summary>
    /// <param name="gameTag">The GameTag object to validate.</param>
    /// <param name="errorMessage">An output parameter that will contain the validation error message if validation fails; otherwise, it will be an empty string.</param>
    /// <returns>True if the GameTag is valid; otherwise, false.</returns>
    public static bool ValidateGameTag(GameTag? gameTag, out string errorMessage)
    {
        if (gameTag == null)
        {
            errorMessage = "GameTag cannot be null.";
            return false;
        }

        if (gameTag.GameId == Guid.Empty)
        {
            errorMessage = "GameTag GameId is required.";
            return false;
        }

        if (gameTag.TagId == Guid.Empty)
        {
            errorMessage = "GameTag TagId is required.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a specified <see cref="Genre"/> object.
    /// </summary>
    /// <param name="genre">The genre object to validate. Can be null.</param>
    /// <param name="errorMessage">Outputs an error message if validation fails. Will be an empty string if validation succeeds.</param>
    /// <returns>
    /// A boolean value indicating whether the provided genre is valid.
    /// Returns true if the genre is valid; otherwise, false.
    /// </returns>
    public static bool ValidateGenre(Genre? genre, out string errorMessage)
    {
        if (genre == null)
        {
            errorMessage = "Genre cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(genre.Name))
        {
            errorMessage = "Genre Name is required.";
            return false;
        }

        if (genre.Name.Length > 100)
        {
            errorMessage = "Genre Name must not exceed 100 characters.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the provided <see cref="ItemImage"/> instance to ensure it contains valid data.
    /// </summary>
    /// <param name="itemImage">The <see cref="ItemImage"/> instance to validate. Can be null.</param>
    /// <param name="errorMessage">Outputs a descriptive error message if the validation fails, or an empty string on success.</param>
    /// <returns>
    /// True if the <see cref="ItemImage"/> is valid; otherwise, false.
    /// </returns>
    public static bool ValidateItemImage(ItemImage? itemImage, out string errorMessage)
    {
        if (itemImage == null)
        {
            errorMessage = "ItemImage cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(itemImage.ImageUrl))
        {
            errorMessage = "ItemImage URL is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(itemImage.ImageAlt))
        {
            errorMessage = "ItemImage Alt Text is required.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the provided MatureContent object to ensure it meets defined requirements.
    /// </summary>
    /// <param name="matureContent">The MatureContent object to validate. Can be null.</param>
    /// <param name="errorMessage">
    /// An output parameter that returns a descriptive error message if validation fails.
    /// Returns an empty string if validation is successful.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether the MatureContent object is valid.
    /// Returns true if the object is valid; otherwise, false.
    /// </returns>
    public static bool ValidateMatureContent(MatureContent? matureContent, out string errorMessage)
    {
        if (matureContent == null)
        {
            errorMessage = "MatureContent cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(matureContent.Name))
        {
            errorMessage = "MatureContent Name is required.";
            return false;
        }

        if (matureContent.Name.Length > 50)
        {
            errorMessage = "MatureContent Name must not exceed 50 characters.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a <see cref="Mechanic"/> instance to ensure it meets defined criteria.
    /// </summary>
    /// <param name="mechanic">The <see cref="Mechanic"/> instance to validate.</param>
    /// <param name="errorMessage">Outputs an error message if validation fails. Contains an empty string if validation succeeds.</param>
    /// <returns>
    /// A boolean indicating whether the validation was successful.
    /// Returns <c>true</c> if the <see cref="Mechanic"/> is valid; otherwise, <c>false</c>.
    /// </returns>
    public static bool ValidateMechanic(Mechanic? mechanic, out string errorMessage)
    {
        if (mechanic == null)
        {
            errorMessage = "Mechanic cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(mechanic.Name))
        {
            errorMessage = "Mechanic Name is required.";
            return false;
        }

        if (mechanic.Name.Length > 50)
        {
            errorMessage = "Mechanic Name must not exceed 50 characters.";
            return false;
        }

        if (mechanic.Description.Length > 1000)
        {
            errorMessage = "Mechanic Description must not exceed 1000 characters.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a ProductGenres object to ensure its properties meet the required criteria.
    /// </summary>
    /// <param name="productGenres">The ProductGenres object to validate. This may represent the relationship between a product and a genre.</param>
    /// <param name="errorMessage">
    /// Outputs an error message describing why the validation failed,
    /// or an empty string if the validation was successful.
    /// </param>
    /// <returns>
    /// Returns true if the ProductGenres object is valid, otherwise returns false.
    /// </returns>
    public static bool ValidateProductGenres(ProductGenres? productGenres, out string errorMessage)
    {
        if (productGenres == null)
        {
            errorMessage = "ProductGenres cannot be null.";
            return false;
        }

        if (productGenres.ProductId == Guid.Empty)
        {
            errorMessage = "ProductGenres ProductId is required.";
            return false;
        }

        if (productGenres.GenreId == Guid.Empty)
        {
            errorMessage = "ProductGenres GenreId is required.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the provided ProductImages object for required data consistency and integrity.
    /// </summary>
    /// <param name="productImages">The ProductImages object to validate.</param>
    /// <param name="errorMessage">
    /// Outputs an error message if validation fails. An empty string is provided if validation is successful.
    /// </param>
    /// <returns>
    /// Returns true if the ProductImages object is valid, otherwise returns false.
    /// </returns>
    public static bool ValidateProductImages(ProductImages? productImages, out string errorMessage)
    {
        if (productImages == null)
        {
            errorMessage = "ProductImages cannot be null.";
            return false;
        }

        if (productImages.ProductId == Guid.Empty)
        {
            errorMessage = "ProductImages ProductId is required.";
            return false;
        }

        if (productImages.ItemImageId == Guid.Empty)
        {
            errorMessage = "ProductImages ItemImageId is required.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a ProductMatureContent object to ensure it meets the required criteria.
    /// </summary>
    /// <param name="productMatureContent">
    /// The ProductMatureContent object to validate. Can be null.
    /// </param>
    /// <param name="errorMessage">
    /// Outputs an error message if the validation fails. Otherwise, outputs an empty string.
    /// </param>
    /// <returns>
    /// Returns true if the ProductMatureContent object is valid; otherwise, returns false.
    /// </returns>
    public static bool ValidateProductMatureContent(ProductMatureContent? productMatureContent, out string errorMessage)
    {
        if (productMatureContent == null)
        {
            errorMessage = "ProductMatureContent cannot be null.";
            return false;
        }

        if (productMatureContent.ProductId == Guid.Empty)
        {
            errorMessage = "ProductMatureContent ProductId is required.";
            return false;
        }

        if (productMatureContent.MatureContentId == Guid.Empty)
        {
            errorMessage = "ProductMatureContent MatureContentId is required.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates a <see cref="Tag"/> object to ensure it meets the required criteria.
    /// </summary>
    /// <param name="tag">The Tag object to validate. This parameter can be null.</param>
    /// <param name="errorMessage">
    /// An output parameter that contains the error message if validation fails.
    /// If validation is successful, this will be an empty string.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether the Tag object is valid.
    /// Returns true if the object passes all validation checks; otherwise, false.
    /// </returns>
    public static bool ValidateTag(Tag? tag, out string errorMessage)
    {
        if (tag == null)
        {
            errorMessage = "Tag cannot be null.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(tag.Title))
        {
            errorMessage = "Tag Title is required.";
            return false;
        }

        if (tag.Title.Length > 30)
        {
            errorMessage = "Tag Title must not exceed 30 characters.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}