using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using PizzaApp.DataAccess.Repository.UserRepository;

namespace PizzaApp.WebApi.Identity;

public class ProfileService(IUserRepository users) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var userId = int.Parse(sub);

        var user = await users.GetByIdWithRolesAsync(userId);

        var roles = user.Roles.Select(r => r.Role.RoleType);

        context.IssuedClaims.AddRange(
            roles.Select(r => new Claim("role", r.ToString()))
        );
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        return Task.CompletedTask;
    }
}