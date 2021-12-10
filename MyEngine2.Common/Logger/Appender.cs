namespace MyEngine2.Common.Logger
{
    public abstract class Appender
    {
        public Formatter Formatter { get; }

        public Level Level { get; }

        public Appender(Level level, Formatter formatter)
        {
            Level = level;
            Formatter = formatter;
        }

        public void PreAppend(Event @event)
        {
            if (Level <= @event.Level)
            {
                Append(@event);
            }
        }

        public abstract void Append(Event @event);
    }
}