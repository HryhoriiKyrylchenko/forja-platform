namespace Forja.API.Controllers.Storage;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileManagerService _fileManagerService;
    private readonly IUserService _userService;
    private readonly IUserLibraryService _userLibraryService;
    private readonly IItemImageService _itemImageService;
    private readonly IProductImagesService _productImagesService;
    private readonly IAuditLogService _auditLogService;
    private readonly IDistributedCache _cache;

    public FilesController(IFileManagerService fileManagerService, 
        IUserService userService,
        IUserLibraryService userLibraryService,
        IItemImageService itemImageService,
        IProductImagesService productImagesService,
        IAuditLogService auditLogService,
        IDistributedCache cache)
    {
        _fileManagerService = fileManagerService;
        _userService = userService;
        _userLibraryService = userLibraryService;
        _itemImageService = itemImageService;
        _productImagesService = productImagesService;
        _auditLogService = auditLogService;
        _cache = cache;
    }
 
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("start-chunked-upload")]
    public async Task<IActionResult> StartChunkedUpload([FromForm] StartChunkedUploadRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var uploadId = await _fileManagerService.StartChunkedUploadAsync(request);
            return Ok(uploadId);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to start chunked upload for file {request.FileName}." }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("upload-chunk")]
    public async Task<IActionResult> UploadChunk([FromForm] UploadChunkRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _fileManagerService.UploadChunkAsync(request);
            return StatusCode((int)result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to upload chunk." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }

            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("complete-chunked-upload")]
    public async Task<IActionResult> CompleteChunkedUpload([FromForm] CompleteChunkedUploadRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await _fileManagerService.CompleteChunkedUploadAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to complete chunked upload." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("file")]
    public async Task<IActionResult> DeleteGameFile([FromForm] DeleteObjectRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _fileManagerService.DeleteGameFileAsync(request);
            return NoContent(); 
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to delete game file." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("avatar")]
    public async Task<IActionResult> UploadUserAvatar([FromForm] UploadObjectImageRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var cachedProfile = await _cache.GetStringAsync($"user_profile_{keycloakUserId}");
            UserProfileDto? userProfile;
            if (!string.IsNullOrWhiteSpace(cachedProfile))
            {
                userProfile = JsonSerializer.Deserialize<UserProfileDto>(cachedProfile);
            }
            else
            {
                userProfile = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            }

            if (userProfile == null || userProfile.Id != request.ObjectId)
            {
                return Unauthorized(new { error = "User ID does not match the user ID in the token claims." });
            }
            
            var avatarUrl = await _fileManagerService.UploadUserAvatarAsync(request);
            return Ok(avatarUrl); 
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to upload user avatar." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize]
    [HttpDelete("avatar")]
    public async Task<IActionResult> DeleteUserAvatar([FromForm] DeleteObjectRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var cachedProfile = await _cache.GetStringAsync($"user_profile_{keycloakUserId}");
            UserProfileDto? userProfile;
            if (!string.IsNullOrWhiteSpace(cachedProfile))
            {
                userProfile = JsonSerializer.Deserialize<UserProfileDto>(cachedProfile);
            }
            else
            {
                userProfile = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            }

            if (userProfile == null || !request.ObjectPath.Contains(userProfile.Id.ToString()))
            {
                return Unauthorized(new { error = "User ID does not match the user ID in the token claims." });
            }
            
            await _fileManagerService.DeleteUserAvatarAsync(request);
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to delete user avatar." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "UserManagePolicy")]
    [HttpDelete("delete-avatar")]
    public async Task<IActionResult> DeleteUserAvatarForManager([FromForm] DeleteObjectRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _fileManagerService.DeleteUserAvatarAsync(request);
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to delete user avatar." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("product-logo")]
    public async Task<IActionResult> UploadProductLogo([FromForm] UploadObjectImageRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _fileManagerService.UploadProductLogoAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to upload product logo." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("product-logo")]
    public async Task<IActionResult> DeleteProductLogo([FromForm] DeleteObjectRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _fileManagerService.DeleteProductLogoAsync(request);
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to delete product logo." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("achievement-image")]
    public async Task<IActionResult> UploadAchievementImage([FromForm] UploadObjectImageRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _fileManagerService.UploadAchievementImageAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to upload achievement image." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("achievement-image")]
    public async Task<IActionResult> DeleteAchievementImageLogo([FromForm] DeleteObjectRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _fileManagerService.DeleteAchievementImageAsync(request);
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to delete achievement image." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("news-article")]
    public async Task<IActionResult> UploadNewsArticleImage([FromForm] UploadObjectImageRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _fileManagerService.UploadNewsArticleImageAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to upload news article image." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("news-article")]
    public async Task<IActionResult> DeleteNewsArticleImageLogo([FromForm] DeleteObjectRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _fileManagerService.DeleteNewsArticleImageAsync(request);
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to delete news article image." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("mature-content-image")]
    public async Task<IActionResult> UploadMatureContentImage([FromForm] UploadObjectImageRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _fileManagerService.UploadMatureContentImageAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to upload mature content image." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("mature-content-image")]
    public async Task<IActionResult> DeleteMatureContentImageLogo([FromForm] DeleteObjectRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _fileManagerService.DeleteMatureContentImageAsync(request);
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to delete mature content image." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("mechanic-image")]
    public async Task<IActionResult> UploadMechanicImage([FromForm] UploadObjectImageRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _fileManagerService.UploadMechanicImageAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to upload mechanic image." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("mechanic-image")]
    public async Task<IActionResult> DeleteMechanicImageLogo([FromForm] DeleteObjectRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _fileManagerService.DeleteMechanicImageAsync(request);
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to delete mechanic image." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("product-image")]
    public async Task<IActionResult> UploadProductImage([FromForm] UploadProductImageRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var imageUrl = await _fileManagerService.UploadProductImageAsync(new UploadImageRequest
            {
                File = request.File,
                ObjectSize = request.ObjectSize,
                ContentType = request.ContentType,
                FileName = request.FileName
            });
            
            if (string.IsNullOrWhiteSpace(imageUrl))
                return BadRequest(new { error = "Failed to upload product image." });

            var itemImage = await _itemImageService.CreateAsync(new ItemImageCreateRequest
            {
                ImageUrl = imageUrl,
                ImageAlt = request.ImageAlt
            });

            if (itemImage == null)
            {
                await _fileManagerService.DeleteProductImageAsync(new DeleteObjectRequest
                {
                    ObjectPath = imageUrl
                });
                
                return BadRequest(new { error = "Failed to create product image." });
            }

            var productImage = await _productImagesService.CreateAsync(new ProductImagesCreateRequest
            {
                ProductId = request.ProductId,
                ItemImageId = itemImage.Id
            });

            if (productImage == null)
            {
                await _itemImageService.DeleteAsync(itemImage.Id);
                await _fileManagerService.DeleteProductImageAsync(new DeleteObjectRequest
                {
                    ObjectPath = imageUrl
                });
                
                return BadRequest(new { error = "Failed to create product image." });
            }
            
            return Ok(productImage);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to upload product image." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("product-image")]
    public async Task<IActionResult> DeleteProductImage([FromForm] DeleteObjectRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _fileManagerService.DeleteProductImageAsync(request);
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to delete product image." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("profile-hat-variant")]
    public async Task<IActionResult> UploadProfileHatVariantFile([FromForm] ProfileHatVariantFileUploadRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _fileManagerService.UploadProfileHatVariantFileAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to upload profile hat variant file." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("profile-hat-variant")]
    public async Task<IActionResult> DeleteProfileHatVariantFile([FromForm] ProfileHatVariantFileDeleteRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _fileManagerService.DeleteProfileHatVariantFileAsync(request);
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to delete profile hat variant file." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [Authorize]
    [HttpGet("presigned-file-url")]
    public async Task<IActionResult> GetPresignedFileUrl([FromQuery] GetPresignedFileUrlRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!StorageRequestsValidator.ValidateGetPresignedFileUrlRequest(request, out var errors))
        {
            return BadRequest(new { error = errors });
        }
        
        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var cachedProfile = await _cache.GetStringAsync($"user_profile_{keycloakUserId}");
            UserProfileDto? userProfile;
            if (!string.IsNullOrWhiteSpace(cachedProfile))
            {
                userProfile = JsonSerializer.Deserialize<UserProfileDto>(cachedProfile);
            }
            else
            {
                userProfile = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            }

            if (userProfile == null)
            {
                return Unauthorized(new { error = "User ID does not match the user ID in the token claims." });
            }

            if (!await _userLibraryService.IsUserOwnedProductAsync(userProfile.Id, request.ProductId))
            {
                return Unauthorized(new { error = "User does not own the product." });
            }
            
            var presignedUrl = await _fileManagerService.GetPresignedUrlAsync(request.ObjectPath, request.ExpirationInSeconds);
            return Ok(presignedUrl);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to get  file Url." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [HttpGet("presigned-image-url")]
    public async Task<IActionResult> GetPresignedImageUrl([FromQuery] GetPresignedImageUrlRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (!StorageRequestsValidator.ValidateGetPresignedImageUrlRequest(request, out var errors))
        {
            return BadRequest(new { error = errors });
        }
        
        try
        {
            var presignedUrl = await _fileManagerService.GetPresignedUrlAsync(request.ObjectPath, request.ExpirationInSeconds);
            return Ok(presignedUrl);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to get presigned image Url." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [HttpGet("profile-hat-variant")]
    public async Task<IActionResult> GetPresignedProfileHatVariantUrl([FromQuery] ProfileHatVariantGetByIdRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _fileManagerService.GetPresignedProfileHatVariantUrlAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.ApiError,
                    EntityType = AuditEntityType.Other,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", "Failed to get presigned Url for profile hat variant." }
                    }
                };

                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
}