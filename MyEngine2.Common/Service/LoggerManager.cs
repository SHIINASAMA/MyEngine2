using MyEngine2.Common.Logger;

namespace MyEngine2.Common.Service
{
    /// <summary>
    /// 全局日志管理器
    /// </summary>
    public class LoggerManager
    {
        /// <summary>
        /// 日志输出器 - 单例
        /// </summary>
        public static Logger.Logger Logger
        {
            get
            {
                return _Logger ?? InitLogger();
            }
        }

        private static Logger.Logger? _Logger;

        private static Logger.Logger InitLogger()
        {
            _Logger = new();
            _Logger.AddAppender(new ConsoleAppender(Level.Debug, new Formatter()));
            return _Logger;
        }

        /// <summary>
        /// 初始全局 Logger
        /// </summary>
        /// <param name="loggerProfile">Logger 配置文件</param>
        public static void InitLogger(ServiceProfile.LoggerProfile loggerProfile)
        {
            _Logger = new();

            if (loggerProfile.ConsoleAppender.Enable)
            {
                _Logger.AddAppender(
                    new ConsoleAppender(
                        loggerProfile.ConsoleAppender.Level,
                        new Formatter(
                            loggerProfile.ConsoleAppender.Pattern,
                            loggerProfile.ConsoleAppender.DatePattern
                            )
                        )
                    );
            }

            if (loggerProfile.FileAppender.Enable)
            {
                _Logger.AddAppender(
                    new FileAppender(
                        DateTime.Now.ToString(loggerProfile.FileAppender.NamePattern),
                        loggerProfile.FileAppender.Level,
                        new Formatter(
                            loggerProfile.FileAppender.Pattern,
                            loggerProfile.FileAppender.DatePattern
                            )
                        )
                    );
            }
        }
    }
}