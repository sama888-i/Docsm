namespace Docsm.Exceptions
{
    public class BadRequestException : Exception, IBaseException
    {
        public int StatusCode => StatusCodes.Status400BadRequest;

        public string ErrorMessage => throw new NotImplementedException();
    }
}
