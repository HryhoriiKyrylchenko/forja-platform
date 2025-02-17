namespace Forja.Infrastructure.Keycloak;

public interface IKeycloakClient
{
    Task<string> CreateUserAsync(KeycloakUser user);
}