using Platform.Application.Abstractions.Data;
using Platform.Application.Messaging;
using Platform.Identity.API.Application.Mappers;
using Platform.Identity.API.Application.Models;
using Platform.Identity.API.Domain;
using Platform.Identity.API.Infrastructure.Persistence.Models;
using Platform.SharedKernel.Responses;
using Platform.SystemContext.Abstractions;

namespace Platform.Identity.API.Application.Commands.SyncUserSession
{
    public class SyncUserSessionHandler : ICommandHandler<SyncUserSessionCommand, UserResponse>
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public SyncUserSessionHandler(IUserContext userContext, IUnitOfWork unitOfWork)
        {
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserResponse>> Handle(SyncUserSessionCommand command, CancellationToken cancellationToken)
        {
            var identityId = _userContext.UserId;
            if (identityId == null)
                return Result<UserResponse>.Failure("Unauthorized: Login information not found.");

            if (string.IsNullOrEmpty(_userContext.UserName) || string.IsNullOrEmpty(_userContext.Email))
                return Result<UserResponse>.Failure("Invalid token data.");

            var userModel = await _unitOfWork.GetRepository<UserModel>().FindAsync(x => x.IdentityId == identityId.Value, false, cancellationToken);

            User user;
            if (userModel == null)
            {
                user = User.Create(identityId.Value, _userContext.UserName, _userContext.Email);
                await _unitOfWork.GetRepository<UserModel>().AddAsync(user.ToPersistence(), cancellationToken);
            }
            else
            {
                user = userModel.ToDomain();
                if (user.UserName != _userContext.UserName || user.Email != _userContext.Email)
                {
                    user.SyncIdentity(_userContext.UserName, _userContext.Email);
                    userModel.UpdateIdentity(user);
                    _unitOfWork.GetRepository<UserModel>().Update(userModel);
                }
            }

            return Result<UserResponse>.Success(user.ToResponse());
        }
    }
}
