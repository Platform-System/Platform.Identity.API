using Platform.Application.Abstractions.Data;
using Platform.Application.Messaging;
using Platform.BuildingBlocks.Responses;
using Platform.Identity.API.Application.Mappers;
using Platform.Identity.API.Infrastructure.Persistence.Models;
using Platform.SystemContext.Abstractions;

namespace Platform.Identity.API.Application.Features.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserHandler : IQueryHandler<GetCurrentUserQuery, CurrentUserResponse>
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public GetCurrentUserHandler(IUserContext userContext, IUnitOfWork unitOfWork)
        {
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CurrentUserResponse>> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
        {
            if (!_userContext.IsAuthenticated || _userContext.UserId is null)
                return Result<CurrentUserResponse>.Failure("Unauthorized: Login information not found.");

            if (string.IsNullOrWhiteSpace(_userContext.UserName) || string.IsNullOrWhiteSpace(_userContext.Email))
                return Result<CurrentUserResponse>.Failure("Invalid token data.");

            var userModel = await _unitOfWork
                .GetRepository<UserModel>()
                .FindAsync(x => x.IdentityId == _userContext.UserId.Value, false, cancellationToken);

            if (userModel is null)
                return Result<CurrentUserResponse>.Failure("User profile not found. Please sync the session first.");

            var response = userModel.ToDomain().ToCurrentUserResponse();

            return Result<CurrentUserResponse>.Success(response);
        }
    }
}
