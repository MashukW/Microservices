using Mango.Web.Accessors.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace Mango.Web.Accessors
{
    public class TokenAccessor : ITokenAccessor
    {
        private IHttpContextAccessor _httpContextAccessor;

        public TokenAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetAccessToken()
        {
            var accessToken = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            }

            return accessToken ?? string.Empty;
        }
    }
}
