namespace Forja.Application.Services.Authentication;

public class KeycloakRolesClaimsTransformer : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity!;

        if (identity.IsAuthenticated)
        {
            var realmAccessClaim = identity.FindFirst("realm_access");
            if (realmAccessClaim != null)
            {
                var resourceAccess = JsonDocument.Parse(realmAccessClaim.Value);

                if (resourceAccess.RootElement.TryGetProperty("roles", out var rolesElement))
                {
                    foreach (var role in rolesElement.EnumerateArray())
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, role.GetString() ?? throw new InvalidOperationException()));
                    }
                }
            }
        }

        return Task.FromResult(principal);
    }
}
