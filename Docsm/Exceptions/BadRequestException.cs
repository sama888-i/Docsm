namespace Docsm.Exceptions
{
    public class BadRequestException : Exception, IBaseException
    {
        public int StatusCode => StatusCodes.Status400BadRequest;
        public string ErrorMessage { get; }

      
        public BadRequestException(string message) : base(message)
        {
            ErrorMessage = message;
        }
    }

    public class BadRequestException<T> : BadRequestException
    {
      
        public BadRequestException() : base($"{typeof(T).Name} has invalid data or request")
        {
        }
    }
}
