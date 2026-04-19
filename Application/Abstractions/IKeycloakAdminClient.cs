namespace Platform.Identity.API.Application.Abstractions;

public interface IKeycloakAdminClient
{
    Task AssignRealmRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken);
}
