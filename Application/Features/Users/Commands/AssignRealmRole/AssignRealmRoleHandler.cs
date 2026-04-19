using Platform.Application.Messaging;
using Platform.BuildingBlocks.Responses;
using Platform.Identity.API.Application.Abstractions;

namespace Platform.Identity.API.Application.Features.Users.Commands.AssignRealmRole;

public sealed class AssignRealmRoleHandler : ICommandHandler<AssignRealmRoleCommand, AssignRealmRoleResponse>
{
    private readonly IKeycloakAdminClient _keycloakAdminClient;

    public AssignRealmRoleHandler(IKeycloakAdminClient keycloakAdminClient)
    {
        _keycloakAdminClient = keycloakAdminClient;
    }

    public async Task<Result<AssignRealmRoleResponse>> Handle(AssignRealmRoleCommand command, CancellationToken cancellationToken)
    {
        if (command.Request.UserId == Guid.Empty)
            return Result<AssignRealmRoleResponse>.Failure("UserId is required.");

        if (string.IsNullOrWhiteSpace(command.Request.RoleName))
            return Result<AssignRealmRoleResponse>.Failure("RoleName is required.");

        var roleName = command.Request.RoleName.Trim();

        await _keycloakAdminClient.AssignRealmRoleAsync(command.Request.UserId, roleName, cancellationToken);

        return Result<AssignRealmRoleResponse>.Success(new AssignRealmRoleResponse
        {
            UserId = command.Request.UserId,
            RoleName = roleName
        });
    }
}
