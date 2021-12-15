namespace MyEngine2.Common.Net
{
    /// <summary>
    /// Http 断点续传区间
    /// </summary>
    public class HttpRange
    {
        /// <summary>
        /// 起始位置,若为 -1 则需要根据具体文件重新计算
        /// </summary>
        public int Start { get; set; } = 0;

        /// <summary>
        /// 结束位置
        /// </summary>
        /// <see cref="Start"/>
        public int End { get; set; } = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        public HttpRange(int start, int end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// <para>转换为 Content-Range 字段所需字符串格式</para>
        /// <para>"bytes {Start}-{End}/{End}-{Start}+1"</para>
        /// <para>注意，该函数不会自动转义数据为 -1 的字段</para>
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return string.Format("bytes {0}-{1}/{2}", Start, End, End - Start + 1);
        }
    }
}