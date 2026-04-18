using Platform.Identity.API.Domain;

namespace Platform.Identity.API.Application.Features.Users.Queries.GetCurrentUser
{
    public static class CurrentUserMapper
    {
        public static CurrentUserResponse ToCurrentUserResponse(this User user)
        {
            return new CurrentUserResponse
            {
                Id = user.Id,
                IdentityId = user.IdentityId,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
