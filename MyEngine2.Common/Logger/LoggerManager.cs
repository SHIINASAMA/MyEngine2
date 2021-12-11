namespace MyEngine2.Common.Logger
{
    /// <summary>
    /// 全局日志管理器
    /// </summary>
    public class LoggerManager
    {
        /// <summary>
        /// 日志输出器 - 单例
        /// </summary>
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
            _Logger = new Logger();
            _Logger.AddAppender(new ConsoleAppender(Level.Debug, new Formatter()));
            return _Logger;
        }
    }
}