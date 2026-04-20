using Platform.Application.Messaging;

namespace Platform.Identity.API.Application.Features.Users.Commands.AssignRealmRole;

public sealed class AssignRealmRoleCommand : ICommand<AssignRealmRoleResponse>
{
    public AssignRealmRoleRequest Request { get; }
    public AssignRealmRoleCommand(AssignRealmRoleRequest request)
    {
        Request = request;
    }
}
