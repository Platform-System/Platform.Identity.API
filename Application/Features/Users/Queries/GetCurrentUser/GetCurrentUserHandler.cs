using Platform.Application.Messaging;
using Platform.BuildingBlocks.Responses;
using Platform.SystemContext.Abstractions;

namespace Platform.Identity.API.Application.Features.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserHandler : IQueryHandler<GetCurrentUserQuery, CurrentUserResponse>
    {
        private readonly IUserContext _userContext;

        public GetCurrentUserHandler(IUserContext userContext)
        {
            _userContext = userContext;
        }

        public Task<Result<CurrentUserResponse>> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
        {
            if (!_userContext.IsAuthenticated || _userContext.UserId is null)
                return Task.FromResult(Result<CurrentUserResponse>.Failure("Unauthorized: Login information not found."));

            if (string.IsNullOrWhiteSpace(_userContext.UserName) || string.IsNullOrWhiteSpace(_userContext.Email))
                return Task.FromResult(Result<CurrentUserResponse>.Failure("Invalid token data."));

            var response = _userContext.ToCurrentUserResponse();

            return Task.FromResult(Result<CurrentUserResponse>.Success(response));
        }
    }
}
