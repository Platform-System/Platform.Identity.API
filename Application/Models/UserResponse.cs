namespace Platform.Identity.API.Application.Models
{
    public class UserResponse
    {
        public required Guid Id { get; set; }
        public required string UserName { get; set; } 
        public required string Email { get; set; }
    }
}
