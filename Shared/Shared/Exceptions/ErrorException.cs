namespace Shared.Exceptions
{
    public class ErrorException : Exception
    {
        public ErrorException()
            : base("Internal Server Error")
        {

        }
    }
}
