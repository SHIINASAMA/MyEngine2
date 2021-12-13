namespace MyEngine2.Common.Service
{
    /// <summary>
    /// Servlet 相关错误
    /// </summary>
    public class ServletException : Exception
    {
        public ServletException() : base()
        {
        }

        public ServletException(string message) : base(message)
        {
        }
    }
}