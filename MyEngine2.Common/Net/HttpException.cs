namespace MyEngine2.Common.Net
{
    /// <summary>
    /// Http 相关错误
    /// </summary>
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