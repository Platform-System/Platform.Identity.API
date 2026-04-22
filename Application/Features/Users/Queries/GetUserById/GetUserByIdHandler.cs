using Platform.Application.Abstractions.Data;
using Platform.Application.Messaging;
using Platform.BuildingBlocks.Responses;
using Platform.Identity.API.Application.Features.Users.Shared;
using Platform.Identity.API.Infrastructure.Persistence.Models;

namespace Platform.Identity.API.Application.Features.Users.Queries.GetUserById
{
    public sealed class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var userModel = await _unitOfWork
                .GetRepository<UserModel>()
                .FindAsync(x => x.Id == query.UserId, false, cancellationToken);

            if (userModel is null)
                return Result<UserResponse>.Failure("User not found.");

            return Result<UserResponse>.Success(userModel.ToResponse());
        }
    }
}
