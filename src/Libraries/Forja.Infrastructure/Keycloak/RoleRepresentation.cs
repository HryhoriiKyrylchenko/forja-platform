/// <summary>
/// Represents a role in Keycloak.
/// </summary>
public class RoleRepresentation
{
    /// <summary>
    /// Gets or sets the unique identifier of the role.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the role.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets a value indicating whether the role is composite.
    /// </summary>
    [JsonPropertyName("composite")]
    public bool Composite { get; set; } = false;
    
    /// <summary>
    /// Gets or sets a value indicating whether the role is a client role.
    /// </summary>
    [JsonPropertyName("clientRole")]
    public bool ClientRole { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the container identifier of the role.
    /// </summary>
    [JsonPropertyName("containerId")]
    public string ContainerId { get; set; } = string.Empty;
}