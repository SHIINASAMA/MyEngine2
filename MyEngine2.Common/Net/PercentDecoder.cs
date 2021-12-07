using System.Text;

namespace MyEngine2.Common.Net
{
    public class PercentDecoder
    {
        /// <summary>
        /// 解码字符串
        /// </summary>
        /// <param name="content">ASCII 编码的字符串</param>
        /// <param name="bufferSize">使用的缓存大小</param>
        /// <returns>UTF8 编码的字符串</returns>
        public static string Decode(string content, int bufferSize = 8192)
        {
            byte[] bytes = new byte[bufferSize];
            char indexChar;
            // char[] encodedChar = new char[2];
            string encodedChar;
            int indexByte = 0;
            for (int index = 0; index < content.Length;)
            {
                indexChar = content[index];
                if (indexChar.Equals('%'))
                {
                    // content.CopyTo(index + 1, encodedChar, 0, 2);
                    encodedChar = content.Substring(index + 1, 2);
                    bytes[indexByte] = Convert.ToByte(encodedChar, 16);
                    indexByte += 1;
                    index += 3;
                }
                else
                {
                    bytes[indexByte] = (byte)indexChar;
                    indexByte += 1;
                    index += 1;
                }
            }
            return Encoding.UTF8.GetString(bytes, 0, indexByte);
        }
    }
}