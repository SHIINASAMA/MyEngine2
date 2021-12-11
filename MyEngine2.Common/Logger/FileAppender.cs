namespace MyEngine2.Common.Logger
{
    /// <summary>
    /// 文件日志输出地
    /// </summary>
    public class FileAppender : Appender
    {
        /// <summary>
        /// 日志文件名称
        /// </summary>
        public string FileName { get; }

        private StreamWriter StreamWriter;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">日志文件名称</param>
        /// <param name="level">日志等级阈值</param>
        /// <param name="formatter">格式化器</param>
        public FileAppender(string fileName, Level level, Formatter formatter) : base(level, formatter)
        {
            FileName = fileName;
            StreamWriter = new(fileName, true);
        }

        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="event">日志事件</param>
        public override void Append(Event @event)
        {
            StreamWriter.WriteLine(Formatter.Format(@event));
            StreamWriter.Flush();
        }

        ~FileAppender()
        {
            StreamWriter.Close();
            StreamWriter.Dispose();
        }
    }
}