using Microsoft.AspNetCore.Identity;

namespace WebApi.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
