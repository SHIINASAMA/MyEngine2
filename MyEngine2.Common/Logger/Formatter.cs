using System.Text;

namespace MyEngine2.Common.Logger
{
    /// <summary>
    /// 日志格式化器
    /// </summary>
    public class Formatter
    {
        /// <summary>
        /// 日志格式化匹配字符串
        /// </summary>
        public string Pattern { get; }

        /// <summary>
        /// 日志日期格式化匹配字符串
        /// </summary>
        public string TimePattern { get; }

        /// <summary>
        /// <para>对应 pattern 而言，可以使用以下占位符</para>
        /// <para>%% -> %</para>
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

        /// <summary>
        /// 格式化日志
        /// </summary>
        /// <param name="event">日志事件</param>
        /// <returns>格式化后字符串</returns>
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

        /// <summary>
        /// 日志等级转字符串
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <returns>日志等级字符串</returns>
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