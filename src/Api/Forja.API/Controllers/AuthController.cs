namespace Forja.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRegistrationService _registrationService;
    
    public AuthController(IUserRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
        try
        {
            await _registrationService.RegisterUserAsync(request);
            return Ok("Registration successful");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}