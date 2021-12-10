namespace MyEngine2.Common.Logger
{
    public class FileAppender : Appender
    {
        public string FileName { get; }

        private StreamWriter StreamWriter;

        public FileAppender(string fileName, Level level, Formatter formatter) : base(level, formatter)
        {
            FileName = fileName;
            StreamWriter = new(fileName, true);
        }

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