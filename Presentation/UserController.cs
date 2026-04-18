using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Identity.API.Application.Features.Users.Commands.SyncUserSession;
using Platform.Identity.API.Application.Features.Users.Queries.GetCurrentUser;
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

        [HttpPost("sync")]
        public async Task<IActionResult> Sync(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new SyncUserSessionCommand(), cancellationToken);
            return result.ToActionResult();
        }
    }
}
