namespace MyEngine2.Common.Net
{
    /// <summary>
    /// Http 报头字段包
    /// </summary>
    public class HttpPackage : Dictionary<string, string>
    {
        public HttpPackage() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public string Host
        {
            get { return GetHeader("Host"); }
            set { SetHeader("Host", value); }
        }

        public string Location
        {
            get { return GetHeader("Location"); }
            set { SetHeader("Location", value); }
        }

        public string Connection
        {
            get { return GetHeader("Connection"); }
            set { SetHeader("Connection", value); }
        }

        public string Accept
        {
            get { return GetHeader("Accept"); }
            set { SetHeader("Accept", value); }
        }

        public string Server
        {
            get { return GetHeader("Server"); }
            set { SetHeader("Server", value); }
        }

        public string Pragma
        {
            get { return GetHeader("Pragma"); }
            set { SetHeader("Pragma", value); }
        }

        public string ContentType
        {
            get { return GetHeader("Content-Type"); }
            set { SetHeader("ContentType", value); }
        }

        public string ContentLength
        {
            get { return GetHeader("Content-Length"); }
            set { SetHeader("Content-Length", value); }
        }

        public string UserAgent
        {
            get { return GetHeader("UserAgent"); }
            set { SetHeader("UserAgent", value); }
        }

        public string Date
        {
            get { return GetHeader("Date"); }
            set { SetHeader("Date", value); }
        }

        public string Range
        {
            get { return GetHeader("Range"); }
            set { SetHeader("Range", value); }
        }

        /// <summary>
        /// 设置报头
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SetHeader(string key, string value)
        {
            base[key] = value;
        }

        /// <summary>
        /// 获取报头
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public string GetHeader(string key)
        {
            return base[key] ?? "";
        }
    }
}