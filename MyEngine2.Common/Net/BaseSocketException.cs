namespace MyEngine2.Common.Net
{
    public class BaseSocketException : Exception
    {
        public BaseSocketException() : base()
        {
        }

        public BaseSocketException(string message) : base(message)
        {
        }
    }
}