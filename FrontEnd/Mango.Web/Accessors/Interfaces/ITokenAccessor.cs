namespace Mango.Web.Accessors.Interfaces
{
    public interface ITokenAccessor
    {
        Task<string> GetAccessToken();
    }
}
