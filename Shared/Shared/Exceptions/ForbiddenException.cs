namespace Shared.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException()
            : base("Access is denied. You are not authorized.")
        {

        }
    }
}
