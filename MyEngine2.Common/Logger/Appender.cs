namespace MyEngine2.Common.Logger
{
    /// <summary>
    /// 日志输出地抽像类，需要实现内部的 Append 方法
    /// </summary>
    public abstract class Appender
    {
        /// <summary>
        /// 格式化器
        /// </summary>
        public Formatter Formatter { get; }

        /// <summary>
        /// 日志等级阈值
        /// </summary>
        public Level Level { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="level">日志等级阈值</param>
        /// <param name="formatter">格式化器</param>
        public Appender(Level level, Formatter formatter)
        {
            Level = level;
            Formatter = formatter;
        }

        /// <summary>
        /// <para>在正式的 Append 前，做日志等级阈值的判断</para>
        /// <para>该函数由 Logger 调用</para>
        /// </summary>
        /// <param name="event">日志事件</param>
        public void PreAppend(Event @event)
        {
            if (Level <= @event.Level)
            {
                Append(@event);
            }
        }

        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="event">日志事件</param>
        public abstract void Append(Event @event);
    }
}