namespace MyEngine2.Common.Logger
{
    public class ConsoleAppender : Appender
    {
        public ConsoleAppender(Formatter formatter) : base(formatter)
        {
        }

        public override void Append(Event @event)
        {
            Console.WriteLine(Formatter.Format(@event));
        }
    }
}