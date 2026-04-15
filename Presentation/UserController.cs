using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Identity.API.Application.Commands.SyncUserSession;
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

        [HttpPost("sync")]
        public async Task<IActionResult> Sync(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new SyncUserSessionCommand(), cancellationToken);
            return result.ToActionResult();
        }
    }
}
