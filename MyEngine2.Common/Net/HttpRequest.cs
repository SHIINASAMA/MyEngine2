namespace MyEngine2.Common.Net
{
    /// <summary>
    /// Http 请求头
    /// </summary>
    public class HttpRequest : HttpPackage
    {
        /// <summary>
        /// Http 版本，默认为 HTTP/1.1
        /// </summary>
        public string Version { get; set; } = "HTTP/1.1";

        /// <summary>
        /// 原始请求 Url，即包含资源路径和参数
        /// </summary>
        public string RawUrl
        {
            get { return QueryString.GenerateRawUrl(); }
            set { QueryString.Reset(value); }
        }

        /// <summary>
        /// Http 方法，默认值为 Nonsupport
        /// </summary>
        public HttpMethod Method { get; set; } = HttpMethod.Nonsupport;

        /// <summary>
        /// 获取资源路径
        /// </summary>
        public string Url
        { get { return QueryString.Url; } }

        /// <summary>
        /// Http 查询字符串
        /// </summary>
        public QueryString QueryString { get; } = new();

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="version">Http 版本</param>
        /// <param name="rawUrl">Http 原始 Url</param>
        /// <param name="method">Http 方法</param>
        public HttpRequest(string version, string rawUrl, HttpMethod method)
        {
            Version = version;
            RawUrl = rawUrl;
            Method = method;

            QueryString.Reset(rawUrl);
        }

        /// <summary>
        /// 将常规属性重置为默认值
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            Version = "HTTP/1.1";
            RawUrl = "/";
            Method = HttpMethod.Nonsupport;
        }
    }
}