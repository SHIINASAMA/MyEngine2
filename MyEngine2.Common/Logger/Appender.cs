namespace MyEngine2.Common.Logger
{
    public abstract class Appender
    {
        public Formatter Formatter { get; }

        public Appender(Formatter formatter)
        {
            Formatter = formatter;
        }

        public abstract void Append(Event @event);
    }
}