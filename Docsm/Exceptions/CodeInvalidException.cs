namespace Docsm.Exceptions
{
    public class CodeInvalidException : Exception, IBaseException
    {
        public int StatusCode => StatusCodes.Status400BadRequest;

        public string ErrorMessage { get; }
        public CodeInvalidException()
        {
            ErrorMessage = "Sending code is false";
        }
        public CodeInvalidException(string message)
        {
            ErrorMessage = message;
        }
    }
    
}
