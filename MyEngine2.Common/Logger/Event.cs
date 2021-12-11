using System.Diagnostics;

namespace MyEngine2.Common.Logger
{
    /// <summary>
    /// 日志事件
    /// </summary>
    public class Event
    {
        private static string UnknownFile = "Unknown_File";

        private static string UnknownThread = "Unknown_Thread";

        /// <summary>
        /// 日志等级
        /// </summary>
        public Level Level { get; }

        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        /// 日志发生文件完整名称
        /// </summary>
        public string FullFileName
        { get { return StackFrame.GetFileName() ?? UnknownFile; } }

        /// <summary>
        /// 日志发生文件简短名称
        /// </summary>
        public string FileName
        {
            get
            {
                string? fileName = StackFrame.GetFileName();
                if (fileName != null)
                {
                    int index = fileName.LastIndexOf('\\');
                    if (index == -1)
                    {
                        index = fileName.LastIndexOf('/');
                    }
                    return fileName.Substring(index + 1, fileName.Length - index - 1);
                }
                else
                {
                    return UnknownFile;
                }
            }
        }

        /// <summary>
        /// 日志发生文件行号
        /// </summary>
        public int LineNumber
        { get { return StackFrame.GetFileLineNumber(); } }

        /// <summary>
        /// 日志发生线程名称
        /// </summary>
        public string ThreadName
        { get { return Thread.Name ?? UnknownThread; } }

        /// <summary>
        /// 日志发生线程ID
        /// </summary>
        public int ThreadId
        { get { return Thread.ManagedThreadId; } }

        /// <summary>
        /// 日志消息
        /// </summary>
        public string Message { get; }

        private StackFrame StackFrame;
        private Thread Thread;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">日志笑嘻嘻</param>
        public Event(Level level, string message)
        {
            Level = level;
            Message = message;
            Time = DateTime.Now;

            Thread = Thread.CurrentThread;

            var stackTrace = new StackTrace(3, true);
            StackFrame = stackTrace.GetFrame(0) ?? new StackFrame();
        }
    }
}