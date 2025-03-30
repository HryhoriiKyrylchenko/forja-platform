namespace Forja.API.Controllers.Storage;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileManagerService _fileManagerService;
    private readonly IUserService _userService;
    private readonly IGameService _gameService;
    private readonly IGameAddonService _gameAddonService;
    private readonly IUserLibraryService _userLibraryService;
    private readonly IAuditLogService _auditLogService;

    public FilesController(IFileManagerService fileManagerService, 
        IUserService userService, 
        IGameService gameService, 
        IGameAddonService gameAddonService,
        IUserLibraryService userLibraryService,
        IAuditLogService auditLogService)
    {
        _fileManagerService = fileManagerService;
        _userService = userService;
        _gameService = gameService;
        _gameAddonService = gameAddonService;
        _userLibraryService = userLibraryService;
        _auditLogService = auditLogService;
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("games/upload")]
    public async Task<IActionResult> UploadGameFiles([FromBody] GameFilesUploadRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var resultPath = await _fileManagerService.UploadGameFilesAsync(request);
            return Ok(new { Message = "Game files successfully uploaded.", Path = resultPath });
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
                        { "Message", $"Failed to upload game files from: {request.FolderPath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"An error occurred while uploading game files: {ex.Message}");
        }
    }

    [Authorize]
    [HttpGet("games/download")]
    public async Task<IActionResult> DownloadGameFiles([FromBody] GameFilesDownloadRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            if (user == null)
            {
                return NotFound(new { error = $"User not found." });
            }

            var game = await _gameService.GetByStorageUrlAsync(request.SourcePath);
            if (game == null)
            {
                return NotFound(new { error = $"Game with storage URL {request.SourcePath} not found." });
            }
        
            var userLibraryGame = await _userLibraryService.GetUserLibraryGameByGameIdAndUserIdAsync(game.Id, user.Id);
            if (userLibraryGame == null)
            {
                return StatusCode(403, new { error = $"User does not have access to game with storage URL {request.SourcePath}." });
            }
        
            await _fileManagerService.DownloadGameFilesAsync(request);
            return Ok("Game files successfully downloaded.");
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
                        { "Message", $"Failed to download game files from: {request.SourcePath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"An error occurred while downloading game files: {ex.Message}");
        }
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("games")]
    public async Task<IActionResult> DeleteGameFiles([FromBody] GameFilesDeleteRequest request)
    {
        if (!ModelState.IsValid)
        {
            throw new Exception("Invalid request.");
        }

        try
        {
            await _fileManagerService.DeleteGameFilesAsync(request);
            return Ok($"Game files for source path '{request.SourcePath}' successfully deleted.");
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
                        { "Message", $"Failed to delete game files from: {request.SourcePath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"An error occurred while deleting game files: {ex.Message}");
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("addons/upload")]
    public async Task<IActionResult> UploadAddonFiles([FromBody] AddonFilesUploadRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var resultPath = await _fileManagerService.UploadAddonFilesAsync(request);
            return Ok(new { Message = "Addon files successfully uploaded.", Path = resultPath });
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
                        { "Message", $"Failed to upload addon files from: {request.FolderPath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"An error occurred while uploading addon files: {ex.Message}");
        }
    }

    [Authorize]
    [HttpGet("addons/download")]
    public async Task<IActionResult> DownloadAddonFiles([FromBody] AddonFilesDownloadRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            if (user == null)
            {
                return NotFound(new { error = $"User not found." });
            }

            var addon = await _gameAddonService.GetByStorageUrlAsync(request.SourcePath);
            if (addon == null)
            {
                return NotFound(new { error = $"Game with storage URL {request.SourcePath} not found." });
            }
        
            var userLibraryAddon = await _userLibraryService.GetUserLibraryAddonByAddonIdAndUserIdAsync(addon.Id, user.Id);
            if (userLibraryAddon == null)
            {
                return StatusCode(403, new { error = $"User does not have access to addon with storage URL {request.SourcePath}." });
            }
            
            await _fileManagerService.DownloadAddonFilesAsync(request);
            return Ok("Addon files successfully downloaded.");
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
                        { "Message", $"Failed to download addon files from: {request.SourcePath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"An error occurred while downloading addon files: {ex.Message}");
        }
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("addons")]
    public async Task<IActionResult> DeleteAddonFiles([FromBody] AddonFilesDeleteRequest request)
    {
        if (!ModelState.IsValid)
        {
            throw new Exception("Invalid request.");
        }

        try
        {
            await _fileManagerService.DeleteAddonFilesAsync(request);
            return Ok($"Addon files for source path '{request.SourcePath}' successfully deleted.");
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
                        { "Message", $"Failed to delete addon files from: {request.SourcePath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"An error occurred while deleting addon files: {ex.Message}");
        }
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("images/upload")]
    public async Task<IActionResult> UploadImageFile([FromBody] ImageFileUploadRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var resultPath = await _fileManagerService.UploadImageFileAsync(request);
            return Ok(new { Message = "Image file successfully uploaded.", Path = resultPath });
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
                        { "Message", $"Failed to upload image file from: {request.FilePath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"An error occurred while uploading image file: {ex.Message}");
        }
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpGet("images/download")]
    public async Task<IActionResult> DownloadImageFile([FromBody] ImageFileDownloadRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _fileManagerService.DownloadImageFileAsync(request);
            return Ok("Image file successfully downloaded.");
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
                        { "Message", $"Failed to download image file from: {request.SourcePath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"An error occurred while downloading image file: {ex.Message}");
        }
    }

    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("images")]
    public async Task<IActionResult> DeleteImageFile([FromBody] ImageFileDeleteRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _fileManagerService.DeleteImageFileAsync(request);
            return Ok($"Image file at source path '{request.SourcePath}' successfully deleted.");
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
                        { "Message", $"Failed to delete image file from: {request.SourcePath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"An error occurred while deleting image file: {ex.Message}");
        }
    }
    
    [Authorize]
    [HttpPost("user-images/upload")]
    public async Task<IActionResult> UploadUserImageFile([FromBody] ImageFileUploadRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            if (user == null)
            {
                return NotFound(new { error = $"User not found." });
            }

            string oldAvatarUrl = ""; 
            if (user.AvatarUrl != null)
            {
                oldAvatarUrl = user.AvatarUrl;
            }
            
            var resultPath = await _fileManagerService.UploadUserImageFileAsync(request);

            try
            {
                await _userService.UpdateUserAvatarAsync(new UserUpdateAvatarRequest
                {
                    Id = user.Id,
                    AvatarUrl = resultPath
                });
            }
            catch (Exception e)
            {
                await _fileManagerService.DeleteUserImageFileAsync(new ImageFileDeleteRequest
                {
                    SourcePath = resultPath
                });
                return StatusCode(500, new { error = $"An error occurred while updating user avatar: {e.Message}" });
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(oldAvatarUrl))
                {
                    await _fileManagerService.DeleteUserImageFileAsync(new ImageFileDeleteRequest
                    {
                        SourcePath = oldAvatarUrl
                    });
                }
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    Message = "Image file successfully uploaded.",
                    Path = resultPath,
                    OldAvatarUrl = oldAvatarUrl,
                    Error = $"An error occurred while deleting old avatar: {e.Message}"
                });
            }
            
            return Ok(new { Message = "Image file successfully uploaded.", Path = resultPath });
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
                        { "Message", $"Failed to upload user image file from: {request.FilePath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, new { error = $"An error occurred while uploading image file: {ex.Message}" });
        }
    }

    [Authorize]
    [HttpDelete("user-images")]
    public async Task<IActionResult> DeleteUserImageFile([FromBody] ImageFileDeleteRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(keycloakUserId))
            {
                return Unauthorized(new { error = "User ID not found in token claims." });
            }

            var user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
            if (user == null)
            {
                return NotFound(new { error = $"User not found." });
            }

            if (user.AvatarUrl != request.SourcePath)
            {
                return StatusCode(403, new { error = $"User does not have access to image with storage URL {request.SourcePath}." });
            }
            
            await _fileManagerService.DeleteImageFileAsync(request);
            return Ok($"Image file at source path '{request.SourcePath}' successfully deleted.");
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
                        { "Message", $"Failed to delete user image file from: {request.SourcePath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"An error occurred while deleting image file: {ex.Message}");
        }
    }
    
    [HttpGet("images/url")]
    public async Task<IActionResult> GetImageUrl([FromQuery] string objectPath)
    {
        if (string.IsNullOrWhiteSpace(objectPath))
        {
            return BadRequest("Object path is required.");
        }

        try
        {
            var path = objectPath.Split("/").First();
            if (path != "images" || path != "user-images")
            {
                throw new Exception("Invalid object path.");
            }
            
            string url = await _fileManagerService.GetPresignedUrlAsync(objectPath);
            return Ok(new { url });
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
                        { "Message", $"Failed to get image url from: {objectPath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"Error generating image URL: {ex.Message}");
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpPost("profile-hat-variant/upload")]
    public async Task<IActionResult> UploadProfileHatVariantFile([FromBody] ProfileHatVariantFileUploadRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var resultPath = await _fileManagerService.UploadProfileHatVariantFileAsync(request);
            return Ok(new { Message = $"Profile hat variant '{request.ProfileHatVariantId}' file successfully uploaded.", Path = resultPath });
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
                        { "Message", $"Failed to profile hat variant file from: {request.FilePath}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, new { error = $"An error occurred while uploading image file: {ex.Message}" });
        }
    }
    
    [Authorize(Policy = "ContentManagePolicy")]
    [HttpDelete("profile-hat-variant")]
    public async Task<IActionResult> DeleteProfileHatVariantFile([FromBody] ProfileHatVariantFileDeleteRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _fileManagerService.DeleteProfileHatVariantFileAsync(request);
            return Ok($"Profile hat variant '{request.ProfileHatVariantId}' successfully deleted.");
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
                        { "Message", $"Failed to profile hat variant file with id: {request.ProfileHatVariantId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"An error occurred while deleting image file: {ex.Message}");
        }
    }
    
    [HttpGet("profile-hat-variant/url")]
    public async Task<IActionResult> GetProfileHatVariantUrl([FromQuery] ProfileHatVariantGetByIdRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            string url = await _fileManagerService.GetPresignedProfileHatVariantUrlAsync(request);
            return Ok(new { url });
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
                        { "Message", $"Failed to get profile hat variant url with id: {request.ProfileHatVariantId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return StatusCode(500, $"Error generating image URL: {ex.Message}");
        }
    }
}