namespace MyEngine2.Common.Logger
{
    public class LoggerManager
    {
        public static Logger Logger
        {
            get
            {
                return _Logger ?? InitLogger();
            }
        }

        private static Logger? _Logger;

        private static Logger InitLogger()
        {
            _Logger = new Logger(Level.Debug);
            _Logger.AddAppender(new ConsoleAppender(new Formatter()));
            return _Logger;
        }
    }
}