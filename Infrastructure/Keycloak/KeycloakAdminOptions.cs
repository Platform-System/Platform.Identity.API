namespace Platform.Identity.API.Infrastructure.Keycloak;

public sealed class KeycloakAdminOptions
{
    public const string SectionName = "KeycloakAdmin";

    public string BaseUrl { get; set; } = string.Empty;
    public string Realm { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
