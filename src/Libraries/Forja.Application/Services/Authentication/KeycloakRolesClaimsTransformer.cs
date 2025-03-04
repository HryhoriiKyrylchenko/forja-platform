namespace Forja.Application.Services.Authentication;

public class KeycloakRolesClaimsTransformer : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity!;

        if (identity.IsAuthenticated)
        {
            var resourceAccessClaim = identity.FindFirst("resource_access");
            if (resourceAccessClaim != null)
            {
                var resourceAccess = JsonDocument.Parse(resourceAccessClaim.Value);

                if (resourceAccess.RootElement.TryGetProperty("Forja.Api", out var clientRolesElement)
                    && clientRolesElement.TryGetProperty("roles", out var rolesElement))
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
