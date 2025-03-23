namespace Forja.API.Controllers.Games;

/// <summary>
/// Controller for managing Genres and ProductGenres associations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;
    private readonly IProductGenresService _productGenresService;

    public GenresController(IGenreService genreService, IProductGenresService productGenresService)
    {
        _genreService = genreService;
        _productGenresService = productGenresService;
    }

    #region Genres Endpoints

    /// <summary>
    /// Gets all genres.
    /// </summary>
    [HttpGet("genres")]
    public async Task<IActionResult> GetAllGenresAsync()
    {
        try
        {
            var genres = await _genreService.GetAllAsync();
            if (!genres.Any())
            {
                return NoContent();
            }

            return Ok(genres);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Gets all deleted genres.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("genres/deleted")]
    public async Task<IActionResult> GetAllDeletedGenresAsync()
    {
        try
        {
            var genres = await _genreService.GetAllDeletedAsync();
            if (!genres.Any())
            {
                return NoContent();
            }

            return Ok(genres);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Gets a genre by its ID.
    /// </summary>
    [HttpGet("genres/{id}")]
    public async Task<IActionResult> GetGenreByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The genre ID cannot be empty." });

        try
        {
            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null)
                return NotFound(new { error = $"Genre with ID {id} not found." });

            return Ok(genre);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Creates a new genre.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("genres")]
    public async Task<IActionResult> CreateGenreAsync([FromBody] GenreCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdGenre = await _genreService.CreateAsync(request);
            return createdGenre != null ? Ok(createdGenre) : BadRequest(new { error = "Failed to create genre." });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing genre.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("genres")]
    public async Task<IActionResult> UpdateGenreAsync([FromBody] GenreUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedGenre = await _genreService.UpdateAsync(request);
            if (updatedGenre == null)
                return NotFound(new { error = $"Genre with ID {request.Id} not found." });

            return Ok(updatedGenre);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
            
    }

    /// <summary>
    /// Deletes an existing genre.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("genres/{id}")]
    public async Task<IActionResult> DeleteGenreAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The genre ID cannot be empty." });

        try
        {
            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null)
                return NotFound(new { error = $"Genre with ID {id} not found." });

            await _genreService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    #endregion

    #region ProductGenres Endpoints

    /// <summary>
    /// Gets all product genre associations.
    /// </summary>
    [HttpGet("product-genres")]
    public async Task<IActionResult> GetAllProductGenresAsync()
    {
        try
        {
            var productGenres = await _productGenresService.GetAllAsync();
            if (!productGenres.Any())
            {
                return NoContent();
            }

            return Ok(productGenres);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Gets a product-genre association by its ID.
    /// </summary>
    [HttpGet("product-genres/{id}")]
    public async Task<IActionResult> GetProductGenreByIdAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The product-genre ID cannot be empty." });

        try
        {
            var productGenre = await _productGenresService.GetByIdAsync(id);
            if (productGenre == null)
                return NotFound(new { error = $"ProductGenre with ID {id} not found." });

            return Ok(productGenre);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Gets product-genre associations by Product ID.
    /// </summary>
    [HttpGet("product-genres/by-product/{productId}")]
    public async Task<IActionResult> GetProductGenresByProductIdAsync([FromRoute] Guid productId)
    {
        if (productId == Guid.Empty)
            return BadRequest(new { error = "The product ID cannot be empty." });

        try
        {
            var productGenres = await _productGenresService.GetByProductIdAsync(productId);
            if (!productGenres.Any())
                return NoContent();

            return Ok(productGenres);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Gets product-genre associations by Genre ID.
    /// </summary>
    [HttpGet("product-genres/by-genre/{genreId}")]
    public async Task<IActionResult> GetProductGenresByGenreIdAsync([FromRoute] Guid genreId)
    {
        if (genreId == Guid.Empty)
            return BadRequest(new { error = "The genre ID cannot be empty." });

        try
        {
            var productGenres = await _productGenresService.GetByGenreIdAsync(genreId);
            if (!productGenres.Any())
                return NoContent();

            return Ok(productGenres);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
            
    }

    /// <summary>
    /// Creates a new product-genre association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("product-genres")]
    public async Task<IActionResult> CreateProductGenreAsync([FromBody] ProductGenresCreateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdProductGenre = await _productGenresService.CreateAsync(request);
            
            return createdProductGenre != null ? Ok(createdProductGenre) : BadRequest(new { error = "Failed to create product-genre association." });
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    /// <summary>
    /// Updates an existing product-genre association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPut("product-genres/{id}")]
    public async Task<IActionResult> UpdateProductGenreAsync([FromBody] ProductGenresUpdateRequest request, [FromRoute] Guid id)
    {
        if (id != request.Id)
            return BadRequest(new { error = "ProductGenre ID mismatch between URL and request body." });

        try
        {
            var updatedProductGenre = await _productGenresService.UpdateAsync(request);
            if (updatedProductGenre == null)
                return NotFound(new { error = $"ProductGenre with ID {id} not found." });

            return Ok(updatedProductGenre);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
        
    }

    /// <summary>
    /// Deletes an existing product-genre association.
    /// </summary>
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("product-genres/{id}")]
    public async Task<IActionResult> DeleteProductGenreAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest(new { error = "The product-genre ID cannot be empty." });

        try
        {
            var productGenre = await _productGenresService.GetByIdAsync(id);
            if (productGenre == null)
                return NotFound(new { error = $"ProductGenre with ID {id} not found." });

            await _productGenresService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    #endregion
}