namespace Shared.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
            : base("Not authorized")
        {

        }
    }
}
