using Platform.Identity.API.Domain;

namespace Platform.Identity.API.Application.Features.Users.Shared
{
    public static class UserMapper
    {
        public static UserResponse ToResponse(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
