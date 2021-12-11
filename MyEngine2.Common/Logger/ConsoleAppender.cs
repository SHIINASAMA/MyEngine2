namespace MyEngine2.Common.Logger
{
    /// <summary>
    /// 控制台日志输出地
    /// </summary>
    public class ConsoleAppender : Appender
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="level">日志等级阈值</param>
        /// <param name="formatter">格式化器</param>
        public ConsoleAppender(Level level, Formatter formatter) : base(level, formatter)
        {
        }

        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="event">日志事件</param>
        public override void Append(Event @event)
        {
            Console.WriteLine(Formatter.Format(@event));
        }
    }
}