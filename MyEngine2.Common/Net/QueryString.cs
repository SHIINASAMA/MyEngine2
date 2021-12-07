namespace MyEngine2.Common.Net
{
    public class QueryString : Dictionary<string, string>
    {
        public string Url { get; set; } = "/";

        public QueryString(string rawUrl)
        {
            int index = rawUrl.IndexOf('?');
            if (index != -1)
            {
                Url = rawUrl.Substring(0, index);
                if (Url.IndexOf('%') != -1)
                {
                    Url = PercentDecoder.Decode(Url);
                }
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
    }
}