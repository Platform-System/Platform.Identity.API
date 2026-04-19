using Platform.Identity.API.Domain;
using Platform.Identity.API.Infrastructure.Persistence.Models;

namespace Platform.Identity.API.Application.Mappers;

public static class PersistenceMapper
{
    public static User ToDomain(this UserModel model)
    {
        return User.Load(model.Id, model.IdentityId, model.UserName, model.Email);
    }
    public static UserModel ToPersistence(this User domain)
    {
        return new UserModel
        {
            Id = domain.Id,
            IdentityId = domain.IdentityId,
            UserName = domain.UserName,
            Email = domain.Email,
            AvatarUrl = domain.AvatarUrl,
            Bio = domain.Bio
        };
    }
    public static void UpdateIdentity(this UserModel model, User domain)
    {
        model.UserName = domain.UserName;
        model.Email = domain.Email;
    }
    public static void UpdateProfile(this UserModel model, User domain)
    {
        model.Bio = domain.Bio;
        model.AvatarUrl = domain.AvatarUrl;
    }
}
