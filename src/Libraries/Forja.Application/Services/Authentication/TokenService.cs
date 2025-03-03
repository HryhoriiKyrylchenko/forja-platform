namespace Forja.Application.Services.Authentication;

/// <summary>
/// Provides methods for generating and validating JSON Web Tokens (JWTs) used in the application.
/// </summary>
public class TokenService : ITokenService
{
    private readonly string _jwtSecret;

    public TokenService(IConfiguration configuration)
    {
        _jwtSecret = configuration["JWT:JwtSecret"] ?? throw new Exception("JwtSecret not found");
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }
    
    /// <inheritdoc />
    public string GeneratePasswordResetToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("userId", user.Id.ToString()),
                new Claim("email", user.Email)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    /// <inheritdoc />
    public string GenerateEmailConfirmationToken(Guid userId, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("userId", userId.ToString()),
                new Claim("email", email),
                new Claim("purpose", "EmailConfirmation")
            ]),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <inheritdoc />
    public async Task<bool> ValidatePasswordResetToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }
        
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var result = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });

            if (!result.Claims.Any())
            {
                return false;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    /// <inheritdoc />
    public Task<bool> ValidateEmailConfirmationToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Task.FromResult(false);
        }

        var principal = GetPrincipalFromToken(token);
        if (principal == null)
        {
            return Task.FromResult(false);
        }

        var purposeClaim = principal.Claims.FirstOrDefault(c => c.Type == "purpose");
        return Task.FromResult(purposeClaim is { Value: "EmailConfirmation" });
        
        // var purposeClaim = principal.FindFirst("purpose");
        // return purposeClaim != null && purposeClaim.Value == "EmailConfirmation";
    }
    
    /// <inheritdoc />
    public Task<string?> GetEmailFromEmailConfirmationToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Task.FromResult<string?>(null);
        }
    
        var principal = GetPrincipalFromToken(token);
        if (principal == null)
        {
            return Task.FromResult<string?>(null);
        }

        var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == "email");
        
        //var emailClaim = principal.FindFirst(ClaimTypes.Email);
        return Task.FromResult(emailClaim?.Value);
    }
    
    private ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSecret);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            NameClaimType = ClaimTypes.NameIdentifier
        };

        try
        {
            return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        }
        catch (Exception)
        {
            return null;
        }
    }
}