namespace Docsm.Exceptions
{
    public static class ImageException
    {
        public class InvalidImageTypeException : BadRequestException
        {
            public InvalidImageTypeException(string message) : base(message)
            {
            }
        }

        public class InvalidImageSizeException : BadRequestException
        {
            public InvalidImageSizeException(string message) : base(message)
            {
            }
        }

    }
}
