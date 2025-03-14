namespace Docsm.Exceptions
{
    public class ExistException : Exception, IBaseException
    {

        public int StatusCode => StatusCodes.Status409Conflict;
        public string ErrorMessage {  get;  }
        public ExistException(string message) : base(message)
        {
            ErrorMessage = message;
        }

    }
    public class ExistException<T> : ExistException 
    {
        public ExistException():base (typeof(T).Name +" is exist")
        {

        }
    }

}
