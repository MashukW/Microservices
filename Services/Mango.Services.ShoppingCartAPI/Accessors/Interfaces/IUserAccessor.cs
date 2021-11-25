using Mango.Services.ShoppingCartAPI.Models;

namespace Mango.Services.ShoppingCartAPI.Accessors.Interfaces
{
    public interface IUserAccessor
    {
        AppUser GetAppUser();
    }
}
