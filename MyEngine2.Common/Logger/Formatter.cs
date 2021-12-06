using System.Text;

namespace MyEngine2.Common.Logger
{
    public class Formatter
    {
        public string Pattern { get; }
        public string TimePattern { get; }

        /// <summary>
        /// <para>对应 pattern 而言，可以使用以下占位符</para>
        /// <para>% -> %\n</para>
        /// <para>%lv ->  level</para>
        /// <para>%tm -> time</para>
        /// <para>%m -> message</para>
        /// <para>%ff -> full file name</para>
        /// <para>%fn -> file name</para>
        /// <para>%ln -> line number</para>
        /// <para>%tn -> thread name</para>
        /// <para>%ti -> thread id</para>
        /// </summary>
        public Formatter(string pattern = "[%lv] %tm %m", string timePattern = "HH:mm:ss")
        {
            Pattern = pattern;
            TimePattern = timePattern;
        }

        public string Format(Event @event)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < Pattern.Length;)
            {
                if (Pattern[index].Equals('%'))
                {
                    switch (Pattern[++index])
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
                            if (Pattern[index + 1].Equals('f'))
                            {
                                stringBuilder.Append(@event.FullFileName);
                            }
                            else if (Pattern[index + 1].Equals('n'))
                            {
                                stringBuilder.Append(@event.FileName);
                            }
                            else
                            {
                                stringBuilder.Append("%f" + Pattern[index + 1]);
                            }
                            index += 2;
                            break;

                        case 'l':
                            if (Pattern[index + 1].Equals('v'))
                            {
                                stringBuilder.Append(LevelToString(@event.Level));
                            }
                            else if (Pattern[index + 1].Equals('n'))
                            {
                                stringBuilder.Append(@event.LineNumber);
                            }
                            else
                            {
                                stringBuilder.Append("%l" + Pattern[index + 1]);
                            }
                            index += 2;
                            break;

                        case 't':
                            if (Pattern[index + 1].Equals('m'))
                            {
                                stringBuilder.Append(@event.Time.ToString(TimePattern));
                            }
                            else if (Pattern[index + 1].Equals('n'))
                            {
                                stringBuilder.Append(@event.ThreadName);
                            }
                            else if (Pattern[index + 1].Equals('i'))
                            {
                                stringBuilder.Append(@event.ThreadId);
                            }
                            else
                            {
                                stringBuilder.Append("%t" + Pattern[index + 1]);
                            }
                            index += 2;
                            break;

                        default:
                            stringBuilder.Append("%" + Pattern[index + 1]);
                            index += 2;
                            break;
                    }
                }
                else
                {
                    stringBuilder.Append(Pattern[index]);
                    index++;
                }
            }
            return stringBuilder.ToString();
        }

        public static string LevelToString(Level level)
        {
            if (level == Level.Info || level == Level.Warn)
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