using System.Diagnostics;

namespace MyEngine2.Common.Logger
{
    public class Event
    {
        private static string UnknownFile = "Unknown_File";

        private static string UnknownThread = "Unknown_Thread";

        public Level Level { get; }
        public DateTime Time { get; }

        public string FullFileName
        { get { return StackFrame.GetFileName() ?? UnknownFile; } }

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

        public int LineNumber
        { get { return StackFrame.GetFileLineNumber(); } }

        public string ThreadName
        { get { return Thread.Name ?? UnknownThread; } }

        public int ThreadId
        { get { return Thread.ManagedThreadId; } }

        public string Message { get; }

        private StackFrame StackFrame;
        private Thread Thread;

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