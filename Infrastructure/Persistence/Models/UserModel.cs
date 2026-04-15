using Platform.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Identity.API.Infrastructure.Persistence.Models
{
    [Table("Users")] 
    public class UserModel : Entity
    {
        [Required]
        public Guid IdentityId { get; set; } 
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }

        public UserModel() { }

        public UserModel(Guid id, Guid identityId, string userName, string email, string? avatarUrl, string? bio)
        {
            Id = id; 
            IdentityId = identityId;
            UserName = userName;
            Email = email;
            AvatarUrl = avatarUrl;
            Bio = bio;
        }
    }
}
