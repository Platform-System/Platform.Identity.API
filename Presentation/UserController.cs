using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Identity.API.Application.Features.Users.Commands.AssignRealmRole;
using Platform.Identity.API.Application.Features.Users.Commands.SyncUserSession;
using Platform.Identity.API.Application.Features.Users.Queries.GetCurrentUser;
using Platform.Identity.API.Application.Features.Users.Queries.GetUserById;
using Platform.BuildingBlocks.Responses;

namespace Platform.Identity.API.Presentation
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetCurrentUserQuery(), cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUserByIdQuery(id), cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost("sync")]
        public async Task<IActionResult> Sync(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new SyncUserSessionCommand(), cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost("roles")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AssignRealmRole([FromBody] AssignRealmRoleRequest request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new AssignRealmRoleCommand(request), cancellationToken);
            return result.ToActionResult();
        }
    }
}
