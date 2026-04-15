using Platform.Domain.Common;

namespace Platform.Identity.API.Domain
{
    public class User : AggregateRoot
    {
        public Guid IdentityId { get; private set; } = default;
        public string UserName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string? AvatarUrl { get; private set; }
        public string? Bio { get; private set; }
        private User() { }
        public static User Create(Guid identityId, string userName, string email)
        {
            var user = new User
            {
                IdentityId = identityId,
                UserName = userName,
                Email = email,
            };
            return user;
        }

        public void SyncIdentity(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }

        public void UpdateProfile(string? bio, string? avatarUrl)
        {
            Bio = bio;
            AvatarUrl = avatarUrl;
        }

        public static User Load(Guid id, Guid identityId, string userName, string email)
        {
            return new User
            {
                Id = id, 
                IdentityId = identityId,
                UserName = userName,
                Email = email
            };
        }
    }
}
