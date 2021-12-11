namespace MyEngine2.Common.Logger
{
    /// <summary>
    /// 日志输出器
    /// </summary>
    public class Logger
    {
        private LinkedList<Appender> Appenders = new LinkedList<Appender>();

        private void Log(Level level, string message)
        {
            Event @event = new Event(level, message);
            foreach (Appender appender in Appenders)
            {
                appender.PreAppend(@event);
            }
        }

        /// <summary>
        /// 添加日志输出地
        /// </summary>
        /// <param name="appender">日志输出地</param>
        public void AddAppender(Appender appender)
        {
            Appenders.AddFirst(appender);
        }

        /// <summary>
        /// 输出日志等级为 Debug 的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void Debug(string message)
        {
            Log(Level.Debug, message);
        }

        /// <summary>
        /// 输出日志等级为 Info 的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void Info(string message)
        {
            Log(Level.Info, message);
        }

        /// <summary>
        /// 输出日志等级为 Warn 的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void Warn(string message)
        {
            Log(Level.Warn, message);
        }

        /// <summary>
        /// 输出日志等级为 Error 的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void Error(string message)
        {
            Log(Level.Error, message);
        }

        /// <summary>
        /// 输出日志等级为 Fatal 的日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void Fatal(string message)
        {
            Log(Level.Fatal, message);
        }
    }
}