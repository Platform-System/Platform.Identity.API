using Grpc.Core;
using Platform.Application.Abstractions.Data;
using Platform.Common.Grpc;
using Platform.Identity.API.Infrastructure.Persistence.Models;
using Platform.Identity.Grpc;

namespace Platform.Identity.API.Presentation.Grpc;

public sealed class IdentityIntegrationService : IdentityIntegration.IdentityIntegrationBase
{
    private readonly IUnitOfWork _unitOfWork;

    public IdentityIntegrationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<GetUserEmailProfileResponse> GetUserEmailProfile(
        GetUserEmailProfileRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            return new GetUserEmailProfileResponse
            {
                Status = ResponseStatusExtensions.Failure("Invalid user id.")
            };
        }

        var userModel = await _unitOfWork
            .GetRepository<UserModel>()
            .FindAsync(x => x.Id == userId, true, context.CancellationToken);

        if (userModel is null)
        {
            return new GetUserEmailProfileResponse
            {
                Status = ResponseStatusExtensions.Failure("User not found.")
            };
        }

        return new GetUserEmailProfileResponse
        {
            Status = ResponseStatusExtensions.Success(),
            Data = new GetUserEmailProfileData
            {
                Id = userModel.Id.ToString(),
                IdentityId = userModel.IdentityId.ToString(),
                UserName = userModel.UserName,
                Email = userModel.Email
            }
        };
    }
}
