namespace Platform.Identity.API.Application.Features.Users.Shared
{
    public class UserResponse
    {
        public required Guid Id { get; set; }
        public required Guid IdentityId { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
    }
}
