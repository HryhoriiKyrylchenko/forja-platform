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
                new Claim("keycloakUserId", user.KeycloakUserId.ToString()),
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

    /// <inheritdoc/>
    public async Task<string?> GetkeycloakUserIdFromToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
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

             var keycloakUserId = result.ClaimsIdentity?.Claims
                        .FirstOrDefault(c => c.Type == "keycloakUserId")?.Value;

            return string.IsNullOrWhiteSpace(keycloakUserId) ? null : keycloakUserId;
        }
        catch
        {
            return null;
        }
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
        
        return Task.FromResult(emailClaim?.Value);
    }
    
    /// <inheritdoc />
    public string GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, 
                ValidateAudience = false, 
                ValidateLifetime = true 
            }, out _);

            var userIdClaim = claimsPrincipal.FindFirst("userId");
            if (userIdClaim?.Value == null)
            {
                throw new Exception("Invalid token.");
            }
            return userIdClaim.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return string.Empty;
        }
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