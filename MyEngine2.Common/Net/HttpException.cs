namespace MyEngine2.Common.Net
{
    public class HttpException : Exception
    {
        public HttpException() : base()
        {
        }

        public HttpException(string message) : base(message)
        {
        }
    }
}