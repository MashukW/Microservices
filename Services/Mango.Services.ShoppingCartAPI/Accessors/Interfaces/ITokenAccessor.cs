namespace Mango.Services.ShoppingCartAPI.Accessors.Interfaces
{
    public interface ITokenAccessor
    {
        Task<string> GetAccessToken();
    }
}
