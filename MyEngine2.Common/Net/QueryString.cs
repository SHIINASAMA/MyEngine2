using System.Text;

namespace MyEngine2.Common.Net
{
    /// <summary>
    /// Http 查询字符串
    /// </summary>
    public class QueryString : Dictionary<string, string>
    {
        /// <summary>
        /// 资源地址
        /// </summary>
        public string Url { get; set; } = "/";

        /// <summary>
        /// 重新解析原始 Url
        /// </summary>
        /// <param name="rawUrl">原始 Url</param>
        public void Reset(string rawUrl)
        {
            base.Clear();
            int index = rawUrl.IndexOf('?');
            if (index != -1)
            {
                Url = rawUrl[..index];
                if (Url.IndexOf('%') != -1)
                {
                    Url = PercentDecoder.Decode(Url);
                }
            }
            else
            {
                // 无查询字符串 - 退出
                Url = rawUrl;
                return;
            }

            string queryString = rawUrl.Substring(index + 1);
            if (queryString.IndexOf('%') != -1)
            {
                queryString = PercentDecoder.Decode(queryString);
            }
            string[] keyValues = queryString.Split('&');
            if (keyValues.Length == 0)
            {
                // 无查询字符串 - 忽略并继续
                return;
            }

            string[] keyValue;
            foreach (string pair in keyValues)
            {
                keyValue = pair.Split('=');
                if (keyValue.Length > 2 || keyValue.Length == 0)
                {
                    // 查询字符串参数错误 -  忽略并继续
                    return;
                }
                Add(keyValue[0], keyValue.Length == 1 ? "" : keyValue[1]);
            }
        }

        /// <summary>
        /// 从数据中生成原始 Url
        /// </summary>
        /// <returns></returns>
        public string GenerateRawUrl()
        {
            StringBuilder stringBuilder = new();
            bool first = true;
            foreach (var pair in this)
            {
                if (!first)
                {
                    stringBuilder.Append('&');
                }
                else
                {
                    first = false;
                }
                stringBuilder.Append(
                    string.Format("{0}={1}",
                    PercentEncoder.Encode(pair.Key),
                    PercentEncoder.Encode(pair.Value))
                    );
            }
            return String.Format("{0}?{1}", Url, stringBuilder.ToString());
        }
    }
}