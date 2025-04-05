namespace Forja.API.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;
    private readonly IAuditLogService _auditLogService;
    private readonly IUserService _userService;
    private readonly IAnalyticsEventService _analyticsEventService;
    private readonly IDistributedCache _cache;

    public HomeController(IHomeService homeService,
        IAuditLogService auditLogService,
        IUserService userService,
        IAnalyticsEventService analyticsEventService,
        IDistributedCache cache)
    {
        _homeService = homeService;
        _auditLogService = auditLogService;
        _userService = userService;
        _analyticsEventService = analyticsEventService;
        _cache = cache;
    }

    [HttpGet]
    public async Task<ActionResult<HomepageDto>> GetHomePageDataAsync()
    {
        try
        {
            var cachedHomepageData = await _cache.GetStringAsync("homepage-data");
            if (!string.IsNullOrWhiteSpace(cachedHomepageData))
            {
                var data = JsonSerializer.Deserialize<HomepageDto>(cachedHomepageData);
                return Ok(data);
            }
            
            var homepageData = await _homeService.GetHomepageDataAsync();
            
            var serializedData = JsonSerializer.Serialize(homepageData);

            await _cache.SetStringAsync("homepage-data", serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
            
            try
            {
                var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                UserProfileDto? user = null;
                if (!string.IsNullOrEmpty(keycloakUserId))
                {
                    user = await _userService.GetUserByKeycloakIdAsync(keycloakUserId);
                }
                await _analyticsEventService.AddEventAsync(AnalyticEventType.PageView,
                    user?.Id,
                    new Dictionary<string, string>
                    {
                        { "Page", "Home" },
                        { "Date", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) }
                    });
            }
            catch (Exception)
            {
                Console.WriteLine("Analytics event creation failed.");
            }
            
            return Ok(homepageData);
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
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Product,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get home page data." }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }
}