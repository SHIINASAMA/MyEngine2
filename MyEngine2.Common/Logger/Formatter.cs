using System.Text;

namespace MyEngine2.Common.Logger
{
    public class Formatter
    {
        public string Parttern { get; }
        public string TimeParttern { get; }

        /**
         * %% -> %
         * %lv ->  level
         * %tm -> time
         * %m -> message
         * %ff -> full file name
         * %fn -> file name
         * %ln -> line number
         * %tn -> thread name
         * %ti -> thread id
         */

        public Formatter(string parttern = "[%lv] %tm %m", string timeParttern = "HH:mm:ss")
        {
            Parttern = parttern;
            TimeParttern = timeParttern;
        }

        public string Format(Event @event)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < Parttern.Length;)
            {
                if (Parttern[index].Equals('%'))
                {
                    switch (Parttern[++index])
                    {
                        case '%':
                            stringBuilder.Append('%');
                            index++;
                            break;
                        case 'm':
                            stringBuilder.Append(@event.Message);
                            index++;
                            break;
                        case 'f':
                            if (Parttern[index + 2].Equals('f'))
                            {
                                stringBuilder.Append(@event.FullFileName);
                            }
                            else if (Parttern[index + 2].Equals('n'))
                            {
                                stringBuilder.Append(@event.FileName);
                            }
                            else
                            {
                                stringBuilder.Append("%f" + Parttern[index + 2]);
                            }
                            index += 2;
                            break;
                        case 'l':
                            if (Parttern[index + 1].Equals('v'))
                            {
                                stringBuilder.Append(LevelToString(@event.Level));
                            }
                            else if (Parttern[index + 1].Equals('n'))
                            {
                                stringBuilder.Append(@event.LineNumber);
                            }
                            else
                            {
                                stringBuilder.Append("%l" + Parttern[index + 1]);
                            }
                            index += 2;
                            break;
                        case 't':
                            if (Parttern[index + 1].Equals('m'))
                            {
                                stringBuilder.Append(@event.Time.ToString(TimeParttern));
                            }
                            else if (Parttern[index + 1].Equals('n'))
                            {
                                stringBuilder.Append(@event.ThreadName);
                            }
                            else if (Parttern[index + 1].Equals('i'))
                            {
                                stringBuilder.Append(@event.ThreadId);
                            }
                            else
                            {
                                stringBuilder.Append("%t" + Parttern[index + 1]);
                            }
                            index += 2;
                            break;
                        default:
                            stringBuilder.Append("%" + Parttern[index + 1]);
                            index += 2;
                            break;
                    }
                }
                else
                {
                    stringBuilder.Append(Parttern[index]);
                    index++;
                }
            }
            return stringBuilder.ToString();
        }

        public static string LevelToString(Level level)
        {
            if (level == Level.INFO || level == Level.WARN)
            {
                return level.ToString() + ' ';
            }
            else
            {
                return level.ToString();
            }
        }
    }
}