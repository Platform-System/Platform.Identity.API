using Platform.Identity.API.Application.Models;
using Platform.Identity.API.Domain;
using Platform.Identity.API.Infrastructure.Persistence.Models;

namespace Platform.Identity.API.Application.Mappers
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
