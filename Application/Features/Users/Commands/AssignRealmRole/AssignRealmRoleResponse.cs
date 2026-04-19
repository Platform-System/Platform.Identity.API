namespace Platform.Identity.API.Application.Features.Users.Commands.AssignRealmRole;

public sealed class AssignRealmRoleResponse
{
    public Guid UserId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
