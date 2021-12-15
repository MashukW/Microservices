using Mango.Web.Accessors.Interfaces;
using Mango.Web.Models;
using System.Security.Claims;

namespace Mango.Web.Accessors
{
    public class UserAccessor : IUserAccessor
    {
        private IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AppUser GetAppUser()
        {
            var appUser = new AppUser
            {
                Id = Guid.Parse(GetClaimValue(ClaimTypes.NameIdentifier)),
                Email = GetClaimValue(ClaimTypes.Email),
                FirstName = GetClaimValue(ClaimTypes.GivenName),
                LastName = GetClaimValue(ClaimTypes.Surname),
                Roles = GetClaimValues(ClaimTypes.Role)
            };

            return appUser;
        }

        private string GetClaimValue(string claimType)
        {
            if (_httpContextAccessor?.HttpContext?.User != null)
            {
                var user = _httpContextAccessor?.HttpContext?.User;
                if (user != null)
                {
                    var claimValue = user.FindFirstValue(claimType);
                    return claimValue ?? string.Empty;
                }
            }

            return string.Empty;
        }

        private string[] GetClaimValues(string claimType)
        {
            if (_httpContextAccessor?.HttpContext?.User != null)
            {
                var user = _httpContextAccessor?.HttpContext?.User;
                if (user != null)
                {
                    var claimValues = user.FindAll(claimType).Select(x => x.Value).ToArray();
                    return claimValues ?? Array.Empty<string>();
                }
            }

            return Array.Empty<string>();
        }
    }
}
