using Platform.SystemContext.Abstractions;

namespace Platform.Identity.API.Application.Features.Users.Queries.GetCurrentUser
{
    public static class CurrentUserMapper
    {
        public static CurrentUserResponse ToCurrentUserResponse(this IUserContext userContext)
        {
            return new CurrentUserResponse
            {
                Id = userContext.UserId!.Value,
                UserName = userContext.UserName!,
                Email = userContext.Email!
            };
        }
    }
}
