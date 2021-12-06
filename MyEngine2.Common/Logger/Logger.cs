namespace MyEngine2.Common.Logger
{
    public class Logger
    {
        public Level Level { get; }

        private LinkedList<Appender> Appenders = new LinkedList<Appender>();

        private void Log(Level level, string message)
        {
            if (Level <= level)
            {
                Event @event = new Event(level, message);
                foreach (Appender appender in Appenders)
                {
                    appender.Append(@event);
                }
            }
        }

        public Logger(Level level = Level.Info)
        {
            Level = level;
        }

        public void AddAppender(Appender appender)
        {
            Appenders.AddFirst(appender);
        }

        public void Debug(string message)
        {
            Log(Level.Debug, message);
        }

        public void Info(string message)
        {
            Log(Level.Info, message);
        }

        public void Warn(string message)
        {
            Log(Level.Warn, message);
        }

        public void Error(string message)
        {
            Log(Level.Error, message);
        }

        public void Fatal(string message)
        {
            Log(Level.Fatal, message);
        }
    }
}