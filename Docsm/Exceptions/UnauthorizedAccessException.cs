namespace Docsm.Exceptions
{
    public class UnauthorizedAccessException : Exception, IBaseException
    {

        public int StatusCode => StatusCodes.Status401Unauthorized;


        public string ErrorMessage { get; }


        public UnauthorizedAccessException(string message) : base(message)
        {
            ErrorMessage = message;
        }

    }

    public class UnauthorizedAccessException<T> : UnauthorizedAccessException
    {
        public UnauthorizedAccessException() : base(typeof(T).Name+ " is not authorized")
        {
        }
    }
    
}
