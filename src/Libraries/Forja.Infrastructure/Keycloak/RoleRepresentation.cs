namespace Forja.Infrastructure.Keycloak;

public class RoleRepresentation
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("composite")]
    public bool Composite { get; set; } = false;
    
    [JsonPropertyName("clientRole")]
    public bool ClientRole { get; set; } = true;
    
    [JsonPropertyName("containerId")]
    public string ContainerId { get; set; } = string.Empty;
}