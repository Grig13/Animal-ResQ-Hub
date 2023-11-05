namespace Application_API.Exceptions
{
    public class NotFoundException : Exception
    {
        public int StatusCode { get; }

        public NotFoundException(string message, int statusCode = 404) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
