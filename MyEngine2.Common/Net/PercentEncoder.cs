using System.Text;

namespace MyEngine2.Common.Net
{
    public class PercentEncoder
    {
        /// <summary>
        /// <para>保留字符硬编码</para>
        /// </summary>
        public static readonly Dictionary<char, string> ReservedCharacters = new Dictionary<char, string>
        {
            { ' ', "%20" },
            { '!', "%21" },
            { '*', "%2A" },
            { '\"', "%22"},
            { '\'', "%27"},
            { '(', "%28" },
            { ')', "%29" },
            { ';', "%3B" },
            { ':', "%3A" },
            { '@', "%40" },
            { '&', "%26" },
            { '=', "%3D" },
            { '+', "%2B" },
            { '$', "%24" },
            { ',', "%2C" },
            { '/', "%2F" },
            { '?', "%3F" },
            { '%', "%25" },
            { '#', "#23" },
            { '[', "%5B" },
            { ']', "%5D" }
        };

        /// <summary>
        /// <para>编码字符串</para>
        /// </summary>
        /// <param name="content">UTF8 编码的字符串</param>
        /// <returns>ASCII 编码的字符串</returns>
        public static string Encode(string content)
        {
            StringBuilder stringBuilder = new();
            char[] indexChar = new char[1];
            for (int index = 0; index < content.Length;)
            {
                indexChar[0] = content[index];
                if (indexChar[0] >= 0x00 && indexChar[0] <= 0x7F)
                {
                    if (ReservedCharacters.ContainsKey(indexChar[0]))
                    {
                        // ASCII 保留字符 - 编码
                        stringBuilder.Append(ReservedCharacters[indexChar[0]]);
                        index += 1;
                    }
                    else
                    {
                        // 普通 ASCII 字符 - 直接输出
                        stringBuilder.Append(indexChar);
                        index += 1;
                    }
                }
                else
                {
                    // 非 ASCII 字符 - 编码
                    byte[] bytes = Encoding.UTF8.GetBytes(indexChar);
                    foreach (byte bt in bytes)
                    {
                        stringBuilder.Append(String.Format("%{0:X2}", bt));
                    }
                    index += 1;
                }
            }
            return stringBuilder.ToString();
        }
    }
}