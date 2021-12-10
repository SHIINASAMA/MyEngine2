namespace MyEngine2.Common.Logger
{
    public class ConsoleAppender : Appender
    {
        public ConsoleAppender(Level level, Formatter formatter) : base(level, formatter)
        {
        }

        public override void Append(Event @event)
        {
            Console.WriteLine(Formatter.Format(@event));
        }
    }
}