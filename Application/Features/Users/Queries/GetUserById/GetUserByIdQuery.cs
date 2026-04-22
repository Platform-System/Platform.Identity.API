using Platform.Application.Messaging;
using Platform.Identity.API.Application.Features.Users.Shared;

namespace Platform.Identity.API.Application.Features.Users.Queries.GetUserById
{
    public sealed class GetUserByIdQuery : IQuery<UserResponse>
    {
        public Guid UserId { get; }
        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }

    }
}
